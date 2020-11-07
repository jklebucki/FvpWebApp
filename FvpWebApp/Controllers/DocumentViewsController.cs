using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FvpWebApp.Data;
using FvpWebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace FvpWebApp.Controllers
{
    public class DocumentViewsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DocumentViewsController(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> GetDocuments()
        {
            var documents = await (
                from d in _context.Documents
                from c in _context.Contractors
                from s in _context.Sources
                where d.ContractorId == c.ContractorId && d.SourceId == s.SourceId
                select new DocumentView
                {
                    DocumentId = d.DocumentId,
                    DocumentDate = d.DocumentDate,
                    SourceDescription = s.Description,
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

            return View("Documents",documents);
        }

        public ActionResult Details(int id)
        {
            return View();
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
