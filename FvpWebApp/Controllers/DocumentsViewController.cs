using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FvpWebApp.Data;
using FvpWebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace FvpWebApp.Controllers
{
    public class DocumentsViewController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DocumentsViewController(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IActionResult> Index()
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
            var sources = await _context.Sources.ToListAsync();
            var sourcesSelectListItems = from s in sources
                                         select new SelectListItem
                                         {
                                             Value = s.SourceId.ToString(),
                                             Text = s.Description
                                         };
            ViewBag.Months = new List<SelectListItem> {
                new SelectListItem { Text = "1", Value = "1" },
                new SelectListItem { Text = "2", Value = "2" },
                new SelectListItem { Text = "3", Value = "3" },
                new SelectListItem { Text = "4", Value = "4" },
                new SelectListItem { Text = "5", Value = "5" },
                new SelectListItem { Text = "6", Value = "6" },
                new SelectListItem { Text = "7", Value = "7" },
                new SelectListItem { Text = "8", Value = "8" },
                new SelectListItem { Text = "9", Value = "9" },
                new SelectListItem { Text = "10", Value = "10" },
                new SelectListItem { Text = "11", Value = "11" },
                new SelectListItem { Text = "12", Value = "12" }
            };
            ViewBag.Sources = sourcesSelectListItems.ToList();
            return View();
        }

        public ActionResult Documents(int id, int month, int year)
        {
            ViewBag.DocId = id;
            ViewBag.Month = month;
            ViewBag.Year = year;
            return View();
        }

        public async Task<IActionResult> GetDocuments(int id, int month, int year)
        {
            var sources = await _context.Sources.Select(s => s.SourceId).ToListAsync();
            if (id > 0)
                sources = new List<int> { id };
            var documents = await (
                from d in _context.Documents
                from c in _context.Contractors
                from s in _context.Sources
                where d.ContractorId == c.ContractorId && d.SourceId == s.SourceId
                && sources.Contains((int)d.SourceId) && d.DocumentDate.Month == month
                && d.DocumentDate.Year == year
                orderby d.DocumentDate
                select new DocumentView
                {
                    DocumentId = d.DocumentId,
                    DocumentDate = d.DocumentDate,
                    SourceDescription = s.Code,
                    SaleDate = d.SaleDate,
                    DocumentNumber = d.DocumentNumber,
                    DocumentStatus = d.DocumentStatus,
                    DocumentSymbol = d.DocumentSymbol,
                    JpkV7 = d.JpkV7,
                    ContractorName = c.Name,
                    ContractorVatId = c.VatId,
                    ContractorCountryCode = c.CountryCode,
                    ContractorStatus = c.ContractorStatus,
                    Net = d.Net,
                    Vat = d.Vat,
                    Gross = d.Gross
                }).ToListAsync();

            return new JsonResult(new { data = documents });
        }

        public async Task<IActionResult> Details(int id)
        {
            var document = await _context.Documents.FirstOrDefaultAsync(d => d.DocumentId == id);
            document.DocumentVats = await _context.DocumentVats.Where(d => d.DocumentId == document.DocumentId).ToListAsync();
            var contractors = await _context.Contractors.Where(c => c.ContractorSourceId == document.DocContractorId && c.SourceId == document.SourceId).ToListAsync();
            contractors.ForEach(c => c.Documents = null);
            return new JsonResult(new DocumentFullView
            {
                Document = document,
                Contractors = contractors,
            });
        }


        public ActionResult Edit(int id)
        {
            return View();
        }

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
    }
}
