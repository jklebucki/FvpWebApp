using FvpWebApp.Data;
using FvpWebApp.Infrastructure;
using FvpWebApp.Models;
using FvpWebApp.Services;
using FvpWebAppModels.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FvpWebApp.Controllers
{
    public class DocumentsImportExportController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DocumentsImportExportController(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IActionResult Index()
        {
            var month = DateTime.Now.Month;
            if (month == 1)
                month = 12;
            else
                month = month - 1;
            var year = DateTime.Now.Year;
            ViewBag.Month = month;
            if (month == 1)
                year = year - 1;
            ViewBag.Year = year;
            ViewBag.Months = ConstData.MonthsSelectList();
            return View();
        }

        public async Task<IActionResult> GetSources()
        {
            return new JsonResult(new { Data = await _context.Sources.ToListAsync() });
        }

        public async Task<IActionResult> GetPendingTickets()
        {
            var ticets = await _context.TaskTickets.Where(t => t.TicketStatus == TicketStatus.Pending).ToListAsync();
            return new JsonResult(new { Data = ticets });
        }


        [HttpPost]
        public async Task<IActionResult> CreateTicets([FromBody] CreateTicketRequest requestData)
        {
            var response = new CreateTicetsResponse { TicketsCreated = false };
            if (DatesFromMonth.DateTo(requestData) >= DateTime.Now)
            {
                response.Message = "Jeszcze nie można przetwarzać danych z tego zakresu - miesiąc musi być zakończony.";
                return new JsonResult(response);
            }
            if (requestData == null)
            {
                response.Message = "Błędne dane zapytania";
                return new JsonResult(response);
            }

            if (!await CheckDataAlreadyTaken(requestData))
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    if (requestData.TicketsGroup == "import")
                    {
                        _context.TaskTickets.AddRange(TicketsGenerator.ImportTickets(requestData));
                    }
                    else if (requestData.TicketsGroup == "export")
                    {
                        _context.TaskTickets.AddRange(TicketsGenerator.ExportTickets(requestData));
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }

                response.TicketsCreated = true;
                response.Message = "Proces rozpoczęty!";
            }
            else
                response.Message = "Dane z tego okresu były już przetwarzane lub trwa ich przetarzanie";
            return new JsonResult(response);
        }

        public async Task<bool> CheckDataAlreadyTaken(CreateTicketRequest createTicketRequest)
        {
            var dateFrom = DatesFromMonth.DateFrom(createTicketRequest);
            var dateTo = DatesFromMonth.DateTo(createTicketRequest);
            List<TaskTicket> tickets = new List<TaskTicket>();
            if (createTicketRequest.TicketsGroup == "import")
                tickets = await _context.TaskTickets.Where(
                    t => t.SourceId == createTicketRequest.SourceId
                    && t.TicketType == TicketType.ImportDocuments
                    && t.TicketStatus != TicketStatus.Failed
                    && ((t.DateFrom <= dateFrom && t.DateTo >= dateFrom) || (t.DateFrom <= dateTo && t.DateTo >= dateTo))).ToListAsync();
            //else
            //    tickets = await _context.TaskTickets.Where(
            //        t => t.SourceId == createTicketRequest.SourceId
            //        && t.TicketType == TicketType.ExportDocumentsToErp
            //        && t.TicketStatus != TicketStatus.Failed
            //        && ((t.DateFrom <= dateFrom && t.DateTo >= dateFrom) || (t.DateFrom <= dateTo && t.DateTo >= dateTo))).ToListAsync();
            return tickets.Count > 0 ? true : false;
        }

        public async Task<IActionResult> GetPendingTasks(int id)
        {
            List<TaskTicket> pendingTasks;
            if (id == 0)
                pendingTasks = await _context.TaskTickets.Where(t => t.TicketStatus == TicketStatus.Added || t.TicketStatus == TicketStatus.Pending).ToListAsync();
            else
                pendingTasks = await _context.TaskTickets.ToListAsync();
            var sources = await _context.Sources.ToListAsync();
            var pendingTasksView = from t in pendingTasks
                                   from s in sources
                                   where t.SourceId == s.SourceId
                                   orderby t.TaskTicketId descending
                                   select new
                                   {
                                       TicketId = t.TaskTicketId,
                                       Source = s.Description,
                                       Year = t.DateFrom.Year,
                                       Month = t.DateFrom.Month,
                                       TicketStatus = t.TicketStatus,
                                       TicketType = t.TicketType,
                                       StartDate = t.CreatedAt,
                                       EndDate = t.StatusChangedAt,
                                   };
            return new JsonResult(new { Data = pendingTasksView });
        }

        [HttpPost]
        [Route("[controller]/BpFileUploadAsync")]
        public async Task<IActionResult> BpFileUploadAsync(List<IFormFile> files, int sourceId, int year, int month, string ticketsGroup)
        {

            List<string[]> jpkVdek = new List<string[]>();
            List<string[]> jpkVat = new List<string[]>();
            List<string[]> jpk = new List<string[]>();
            string errorsJpkVdek = "";
            string errorsJpkVat = "";
            int lineNo = 1;

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    using (var reader = new StreamReader(file.OpenReadStream(), Encoding.GetEncoding(1250)))
                    {
                        while (reader.Peek() >= 0)
                        {
                            var line = await reader.ReadLineAsync();
                            if (line.Length > 0)
                            {
                                var fields = line.Split("|");
                                if (fields[2] == "JPK_VDEK")
                                {
                                    if (fields.Length != 69)
                                        errorsJpkVdek += $"{lineNo},";
                                    else if (fields[9] != "SPRZEDAZ NIEFAKTUROWA")
                                        jpkVdek.Add(fields);
                                }
                                if (fields[2] == "JPK_VAT")
                                {
                                    if (fields.Length != 43)
                                        errorsJpkVat += $"{lineNo},";
                                    else if (fields[9] != "brak")
                                        jpkVat.Add(fields);
                                }

                            }
                        }
                    }
                }
            }

            var status = true;
            var message = (errorsJpkVdek.Length > 0 ? $"Błędy w JPK_VDEK: {errorsJpkVdek}\t" : "")
                + (errorsJpkVat.Length > 0 ? $"Błędy w JPK_VAT: {errorsJpkVat}\t" : "");
            if (jpkVdek.Count == jpkVat.Count)
            {
                try
                {
                    foreach (var jpkVdekRow in jpkVdek)
                    {
                        var jpkVatRow = jpkVat.FirstOrDefault(r => r[10] == jpkVdekRow[10]);
                        var concatedJpkRow = new string[jpkVdekRow.Length + jpkVatRow.Length];
                        jpkVdekRow.CopyTo(concatedJpkRow, 0);
                        jpkVatRow.CopyTo(concatedJpkRow, jpkVdekRow.Length);
                        jpk.Add(concatedJpkRow);
                    }
                }
                catch (Exception ex)
                {
                    status = false;
                    message += ex.Message;
                }
            }
            else
            {
                status = false;
                message += "Pliki nie są poprawne.";
            }
            var docs = ConvertFileToDb.Documents(jpk, sourceId);

            DocumentsImportService documentsImportService = new DocumentsImportService(_context);
            var serviceResponse = await documentsImportService
                .InsertDocumentsAsync(
                    docs,
                    new CreateTicketRequest
                    {
                        TicketsGroup = ticketsGroup,
                        Year = year,
                        Month = month,
                        SourceId = sourceId
                    });
            if (!serviceResponse.Valid)
                message = serviceResponse.Message;
            message = string.IsNullOrEmpty(message) ? "Pliki poprawnie przesłane. Proces importu rozpoczęty." : message;
            return new JsonResult(new { Status = status, Message = message });
        }
    }
}
