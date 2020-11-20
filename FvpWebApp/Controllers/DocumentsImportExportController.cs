using FvpWebApp.Data;
using FvpWebApp.Infrastructure;
using FvpWebApp.Models;
using FvpWebAppModels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var dateTo = DatesFromMonth.DateTo(requestData);
            if (dateTo >= DateTime.Now)
            {
                response.Message = "Jeszcze nie można importować danych z tego zakresu - miesiąc musi być zakończony.";
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
                        _context.TaskTickets.AddRange(TicetsGenerator.ImportTickets(requestData));
                    }
                    else if (requestData.TicketsGroup == "export")
                    {
                        _context.TaskTickets.AddRange(TicetsGenerator.ExportTickets(requestData));
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }

                response.TicketsCreated = true;
                response.Message = "Proces importu rozpoczęty!";
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
            else
                tickets = await _context.TaskTickets.Where(
                    t => t.SourceId == createTicketRequest.SourceId
                    && t.TicketType == TicketType.ExportDocumentsToErp
                    && t.TicketStatus != TicketStatus.Failed
                    && ((t.DateFrom <= dateFrom && t.DateTo >= dateFrom) || (t.DateFrom <= dateTo && t.DateTo >= dateTo))).ToListAsync();
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
    }
}
