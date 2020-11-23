using FvpWebApp.Data;
using FvpWebApp.Infrastructure;
using FvpWebApp.Models;
using FvpWebAppModels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

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
            ViewBag.Months = ConstData.MonthsSelectList();
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

        [HttpPatch]
        public async Task<IActionResult> SetValidAsPerson([FromBody]int documentId)
        {
            try
            {
                var transaction = await _context.Database.BeginTransactionAsync();
                var document = await _context.Documents.FirstOrDefaultAsync(d => d.DocumentId == documentId);
                document.DocumentStatus = DocumentStatus.Accepted;
                await _context.SaveChangesAsync();
                var contractor = await _context.Contractors.FirstOrDefaultAsync(c => c.ContractorId == document.ContractorId);
                contractor.ContractorStatus = ContractorStatus.Accepted;
                contractor.VatId = "BRAK";
                await _context.SaveChangesAsync();
                await transaction.CommitAsync().ConfigureAwait(false);
                return new JsonResult(new { Status = true, Message = "OK"});
            }
            catch (Exception ex)
            {
                return new JsonResult(new { Status = false, Message = ex.Message });
            }
        }

        [HttpPatch]
        public async Task<IActionResult> SetInvalid([FromBody] int documentId)
        {
            try
            {
                var transaction = await _context.Database.BeginTransactionAsync();
                var document = await _context.Documents.FirstOrDefaultAsync(d => d.DocumentId == documentId);
                document.DocumentStatus = DocumentStatus.Invalid;
                await _context.SaveChangesAsync();
                var contractor = await _context.Contractors.FirstOrDefaultAsync(c => c.ContractorId == document.ContractorId);
                contractor.ContractorStatus = ContractorStatus.Invalid;
                await _context.SaveChangesAsync();
                await transaction.CommitAsync().ConfigureAwait(false);
                return new JsonResult(new { Status = true, Message = "OK" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { Status = false, Message = ex.Message });
            }
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
                select new DocumentView(d, s, c)).ToListAsync();

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

        [HttpPost]
        [Route("DocumentsView/ChangeContractor")]
        public async Task<IActionResult> ChangeContractor([FromBody] RequestData data)
        {

            var document = await _context.Documents.FirstOrDefaultAsync(d => d.DocumentId == data.DocumentId);
            if (document == null)
                return new StatusCodeResult((int)HttpStatusCode.BadRequest);
            var contractor = await _context.Contractors.FirstOrDefaultAsync(c => c.ContractorId == data.ContractorId && c.SourceId == document.SourceId);
            if (contractor != null)
            {
                try
                {
                    if (data.ChangeAllDocuments)
                    {
                        var documents = await _context.Documents.Where(d => d.ContractorId == document.ContractorId && d.SourceId == document.SourceId).ToListAsync();
                        documents.ForEach(d => d.ContractorId = data.ContractorId);
                        _context.UpdateRange(documents);
                    }
                    else
                    {
                        document.ContractorId = data.ContractorId;
                        _context.Update(document);
                    }
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    return new StatusCodeResult((int)HttpStatusCode.BadRequest);
                }
                return new StatusCodeResult((int)HttpStatusCode.OK);
            }
            return new StatusCodeResult((int)HttpStatusCode.NotFound);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeDocumentStatus([FromBody] int documentId, [FromBody] DocumentStatus documentStatus)
        {
            var document = await _context.Documents.FirstOrDefaultAsync(d => d.DocumentId == documentId);
            if (document != null)
            {
                document.DocumentStatus = documentStatus;
                try
                {
                    _context.Update(document);
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    return new StatusCodeResult((int)HttpStatusCode.BadRequest);
                }
                return new StatusCodeResult((int)HttpStatusCode.OK);
            }
            return new StatusCodeResult((int)HttpStatusCode.NotFound);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeContractorStatus([FromBody] int contractorId, [FromBody] ContractorStatus contractorStatus)
        {
            var contractor = await _context.Contractors.FirstOrDefaultAsync(c => c.ContractorId == contractorId);
            if (contractor != null)
            {
                contractor.ContractorId = contractorId;
                try
                {
                    _context.Update(contractor);
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    return new StatusCodeResult((int)HttpStatusCode.BadRequest);
                }
                return new StatusCodeResult((int)HttpStatusCode.OK);
            }
            return new StatusCodeResult((int)HttpStatusCode.NotFound);
        }
    }
}
