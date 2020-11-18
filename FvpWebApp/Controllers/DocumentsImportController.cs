using FvpWebApp.Data;
using FvpWebApp.Infrastructure;
using FvpWebApp.Models;
using FvpWebAppModels.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FvpWebApp.Controllers
{
    public class DocumentsImportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DocumentsImportController(ApplicationDbContext context)
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
            if (requestData == null)
            {
                response.Message = "Błędne dane zapytania";
                return new JsonResult(response);
            }

            if (!await CheckDataAlreadyTaken(requestData))
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    _context.TaskTickets.AddRange(TicetsGenerator.ImportTickets(requestData));
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }

                response.TicketsCreated = true;
                response.Message = "Proces importu rozpoczęty!";
            }
            else
                response.Message = "Import danych z tego okresu już się odbył lub trwa.";
            return new JsonResult(response);
        }



        public async Task<bool> CheckDataAlreadyTaken(CreateTicketRequest createTicketRequest)
        {
            var dateFrom = DatesFromMonth.DateFrom(createTicketRequest);
            var dateTo = DatesFromMonth.DateTo(createTicketRequest);
            var tickets = await _context.TaskTickets.Where(
                t => t.SourceId == createTicketRequest.SourceId
                && t.TicketType == TicketType.ImportDocuments
                && t.TicketStatus != TicketStatus.Failed
                && ((t.DateFrom <= dateFrom && t.DateTo >= dateFrom) || (t.DateFrom <= dateTo && t.DateTo >= dateTo))).ToListAsync();
            return tickets.Count > 0 ? true : false;
        }

        // GET: DocumentsImport/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DocumentsImport/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DocumentsImport/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DocumentsImport/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
