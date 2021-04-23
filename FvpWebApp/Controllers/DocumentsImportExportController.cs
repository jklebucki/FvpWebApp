using FvpWebApp.Data;
using FvpWebApp.Infrastructure;
using FvpWebApp.Models;
using FvpWebApp.Services;
using FvpWebAppModels.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var year = DateTime.Now.Year;
            if (month == 1)
            {
                year = year - 1;
                month = 12;
            }
            else
                month = month - 1;
            ViewBag.Month = month;
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

        public async Task<IActionResult> RepairData()
        {
            int[] ids = new int[] {
                41467, 41481, 41523, 41536, 41539, 41557, 41558, 41578, 41655, 41656, 41657, 41658, 41659, 41660, 41661, 41662, 41663, 41706, 41725,
                41753, 41765, 41779, 42036, 42289, 42420, 43148, 43149, 43150, 43182, 43334, 43898, 43899, 43900, 44021,
                19060,19135,19219,19243,19279,19301,19303,19310,19360,19488,19500,19909,21037};

            var documents = await _context.Documents.Where(d => ids.Contains(d.DocumentId)).ToListAsync();
            //documents.ForEach(d => d.DocContractorId = d.DocContractorName.GetHashCode().ToString());

            //var newContractors = documents
            //    .GroupBy(g => new
            //    {
            //        g.SourceId,
            //        g.DocContractorId,
            //        g.DocContractorName,
            //        g.DocContractorStreetAndNumber,
            //        g.DocContractorCity,
            //        g.DocContractorCountryCode,
            //        g.DocContractorPostCode,
            //        g.DocContractorVatId,
            //        g.DocContractorFirm,
            //    })
            //    .Select(c => new Contractor
            //    {
            //        SourceId = c.Key.SourceId,
            //        ContractorSourceId = c.Key.DocContractorId,
            //        Name = c.Key.DocContractorName,
            //        Street = c.Key.DocContractorStreetAndNumber,
            //        City = c.Key.DocContractorCity,
            //        CountryCode = c.Key.DocContractorCountryCode,
            //        PostalCode = c.Key.DocContractorPostCode,
            //        VatId = c.Key.DocContractorVatId,
            //        ContractorStatus = ContractorStatus.Accepted,
            //        Firm = (Firm)c.Key.DocContractorFirm
            //    }).ToList();

            //foreach (var cont in newContractors)
            //{
            //    var contractorExist = await _context.Contractors.FirstOrDefaultAsync(c => c.ContractorSourceId == cont.ContractorSourceId && c.SourceId == cont.SourceId);
            //    if (contractorExist == null)
            //        await _context.Contractors.AddAsync(cont);
            //}
            //await _context.SaveChangesAsync();

            //foreach (var doc in documents)
            //{
            //    doc.DocContractorId = doc.DocContractorName.GetHashCode().ToString();
            //    var contractor = await _context.Contractors.FirstOrDefaultAsync(c => c.ContractorSourceId == doc.DocContractorId);
            //    if (contractor == null)
            //        doc.ContractorId = null;
            //    else
            //        doc.ContractorId = contractor.ContractorId;
            //}

            //_context.UpdateRange(documents);
            //await _context.SaveChangesAsync();
            return new JsonResult(documents);
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
                    using (var reader = new StreamReader(file.OpenReadStream(), Encoding.UTF8))
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
                                    else if (fields[9] != "SPRZEDAZ NIEFAKTUROWA" && fields[9].ToUpper() != "SPRZEDAZ NIEFAKTUROWANA")
                                        jpkVdek.Add(fields);
                                }
                                if (fields[2] == "JPK_FA")
                                {
                                    if (fields.Length != 62)
                                        errorsJpkVat += $"{lineNo},";
                                    else if (fields[9] != "brak" && fields[9] != "SPRZEDAZ NIEFAKTUROWA" && fields[9].ToUpper() != "SPRZEDAZ NIEFAKTUROWANA")
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
                        var jpkVatRow = jpkVat.FirstOrDefault(r => r[8] == jpkVdekRow[10]);
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

            if (!status)
                return new JsonResult(new { Status = status, Message = message });

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
