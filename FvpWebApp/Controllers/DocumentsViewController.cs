using C2FKInterface.Data;
using C2FKInterface.Services;
using FvpWebApp.Data;
using FvpWebApp.Infrastructure;
using FvpWebApp.Models;
using FvpWebApp.Services;
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
            ViewBag.SourceId = id;
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
                select new DocumentView(d, s, c)).ToListAsync();

            return new JsonResult(new { data = documents });
        }

        [HttpPatch]
        public async Task<IActionResult> SetValidAsPerson([FromBody] int documentId)
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
                return new JsonResult(new { Status = true, Message = "OK" });
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
                _context.Update(document);
                await _context.SaveChangesAsync();
                var contractor = await _context.Contractors.FirstOrDefaultAsync(c => c.ContractorId == document.ContractorId);
                contractor.ContractorStatus = ContractorStatus.Invalid;
                _context.Update(contractor);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync().ConfigureAwait(false);
                return new JsonResult(new { Status = true, Message = "OK" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { Status = false, Message = ex.Message });
            }
        }

        [HttpPatch]
        public async Task<IActionResult> SetValid([FromBody] int documentId)
        {
            try
            {
                var document = await _context.Documents.FirstOrDefaultAsync(d => d.DocumentId == documentId);
                if (document != null && document.DocumentStatus == DocumentStatus.SentToC2FK)
                {
                    return new JsonResult(new { Status = false, Message = "Dokument wysłany do FK - operacja niedozwolona." });
                }
                var transaction = await _context.Database.BeginTransactionAsync();
                document.DocumentStatus = DocumentStatus.Accepted;
                _context.Update(document);
                await _context.SaveChangesAsync();
                var contractor = await _context.Contractors.FirstOrDefaultAsync(c => c.ContractorId == document.ContractorId);
                contractor.ContractorStatus = ContractorStatus.Accepted;
                _context.Update(contractor);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync().ConfigureAwait(false);
                return new JsonResult(new { Status = true, Message = "OK" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { Status = false, Message = ex.Message });
            }
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
        [Route("DocumentsView/CheckContractorByApi")]
        public async Task<IActionResult> CheckContractorByApi([FromBody] Contractor contractor)
        {
            ApiService apiService = new ApiService();
            var token = await apiService.ApiLogin();
            var countries = await apiService.GetCountriesAsync(token);
            var ueCountries = await _context.Countries.ToArrayAsync();

            if (contractor.CountryCode.ToUpper() == "PL")
            {
                var response = await apiService.GetGusDataAsync(contractor.VatId, token);
                if (response.Count > 0)
                {
                    contractor.Name = response[0].Name;
                    contractor.VatId = response[0].VatNumber;
                    contractor.Street = response[0].Street;
                    contractor.EstateNumber = response[0].EstateNumber;
                    contractor.Firm = Firm.FirmaPolska;
                    contractor.QuartersNumber = response[0].QuartersNumber;
                    contractor.City = response[0].City;
                    contractor.Province = response[0].Province;
                    contractor.Email = response[0].Email;
                    contractor.PostalCode = response[0].PostalCode;
                    contractor.Regon = response[0].Regon;
                    return new JsonResult(new { Origin = "GUS", Valid = true, Data = contractor });
                }

            }
            else if (ueCountries.Select(c => c.Symbol).Contains(contractor.CountryCode.ToUpper()))
            {
                var viesRequest = new ViesSimpleRequest
                {
                    ContractorPrefix = contractor.CountryCode.ToUpper(),
                    ContractorSuffix = contractor.VatId.Substring(2, contractor.VatId.Length - 2),
                };
                var response = await apiService.GetViesDataAsync(viesRequest, token);
                var viesContractor = new Contractor { Name = string.IsNullOrEmpty(response.Name) ? "" : response.Name };
                return new JsonResult(new { Origin = "VIES", Valid = response.Status, Data = viesContractor });
            }
            else if (countries.Select(c => c.Symbol).Contains(contractor.CountryCode.ToUpper()))
            {
                return new JsonResult(new
                {
                    Origin = "WORLD",
                    Valid = true,
                    Data = contractor
                });
            }

            return new JsonResult(new { Origin = "NONE", Valid = false, Data = new Contractor() });
        }

        [HttpPost]
        [Route("DocumentsView/ChangeContractorData")]
        public async Task<IActionResult> ChangeContractorData([FromBody] Contractor contractor)
        {
            var contractorToChange = await _context.Contractors.FirstOrDefaultAsync(c => c.ContractorId == contractor.ContractorId);
            if (contractorToChange != null)
            {
                try
                {
                    contractorToChange.ContractorStatus = contractor.ContractorStatus;
                    contractorToChange.Name = contractor.Name;
                    contractorToChange.Street = contractor.Street;
                    contractorToChange.EstateNumber = contractor.EstateNumber;
                    contractorToChange.QuartersNumber = contractor.QuartersNumber;
                    contractorToChange.City = contractor.City;
                    contractorToChange.PostalCode = contractor.PostalCode;
                    contractorToChange.Province = contractor.Province;
                    contractorToChange.VatId = contractor.VatId;
                    contractorToChange.Regon = contractor.Regon;
                    contractorToChange.GusContractorEntriesCount = 1;
                    contractorToChange.Firm = contractor.Firm;
                    contractorToChange.CountryCode = contractor.CountryCode;
                    contractorToChange.CheckDate = DateTime.Now;
                    var forbiddenStatuses = new DocumentStatus[] { DocumentStatus.SentToC2FK, DocumentStatus.DoNotSentToErp, DocumentStatus.Accepted };
                    var documents = await _context.Documents
                        .Where(d => d.ContractorId == contractor.ContractorId && !forbiddenStatuses.Contains(d.DocumentStatus))
                        .ToListAsync();
                    documents.ForEach(d => d.DocumentStatus = DocumentStatus.Valid);
                    _context.Update(contractorToChange);
                    _context.UpdateRange(documents);
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
        [Route("DocumentsView/SetDocumentValid")]
        public async Task<IActionResult> SetDocumentValid([FromBody] int documentId)
        {

            var document = await _context.Documents.FirstOrDefaultAsync(d => d.DocumentId == documentId);
            if (document == null)
                return new StatusCodeResult((int)HttpStatusCode.BadRequest);
            var contractor = await _context.Contractors.FirstOrDefaultAsync(c => c.ContractorId == document.ContractorId && c.SourceId == document.SourceId);
            if (contractor != null)
            {
                try
                {
                    contractor.ContractorStatus = ContractorStatus.Accepted;
                    document.DocumentStatus = DocumentStatus.Accepted;
                    _context.Update(contractor);
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

        public ActionResult DuplicatedDocuments(int id, int month, int year)
        {
            ViewBag.DocId = id;
            ViewBag.Month = month;
            ViewBag.Year = year;
            return View();
        }

        public ActionResult NotPresentDocuments(int id, int month, int year)
        {
            ViewBag.DocId = id;
            ViewBag.Month = month;
            ViewBag.Year = year;
            return View();
        }

        [HttpGet]
        [Route("DocumentsView/GetFkNotPresentDocuments/{sourceId}/{year}/{month}")]
        public async Task<IActionResult> GetFkNotPresentDocuments(int sourceId, int year, int month)
        {

            var target = await _context.Targets.FirstOrDefaultAsync(t => t.Sources.Select(s => s.SourceId).Contains(sourceId));
            var docSettings = await _context.TargetDocumentsSettings.FirstOrDefaultAsync(t => t.SourceId == sourceId);
            DbConnectionSettings dbConnectionSettings = new DbConnectionSettings(target.DatabaseAddress, target.DatabaseUsername, target.DatabasePassword, target.DatabaseName);
            C21DocumentService c21DocumentService = new C21DocumentService(dbConnectionSettings, null);
            var fkfDocuments = (await c21DocumentService.GetFKDocuments(year, month, docSettings.DocumentShortcut));
            if (fkfDocuments == null)
                return Ok(new { data = new List<DocumentView>() });
            _context.Database.SetCommandTimeout(0);
            var systemDocuments = await _context.Documents.Where(d => d.SourceId == sourceId && d.DocumentDate.Month == month).ToListAsync();
            var notPresentDocs = systemDocuments.Where(d => !fkfDocuments.Select(d => d.tresc).Contains(d.DocumentNumber) && d.DocumentDate.Year == year).ToList();

            var sourcesIds = await _context.Sources.Select(s => s.SourceId).ToListAsync();
            if (sourceId > 0)
                sourcesIds = new List<int> { sourceId };
            var contractors = await _context.Contractors.Where(s=>s.SourceId == sourceId).ToListAsync();
            var source = await _context.Sources.FirstOrDefaultAsync(i=>i.SourceId == sourceId);
            var documents = (
                    from d in notPresentDocs
                    from c in contractors
                    where 
                        d.ContractorId == c.ContractorId 
                        && d.DocumentDate.Month == month
                        && d.DocumentDate.Year == year
                    orderby d.DocumentDate
                    select new DocumentView(d, source, c)).ToList();

            return Ok(new { data = documents });
        }

        [HttpGet]
        [Route("DocumentsView/GetFkDuplicatedDocuments/{sourceId}/{year}/{month}")]
        public async Task<IActionResult> GetFkDuplicatedDocuments(int sourceId, int year, int month)
        {
            var target = await _context.Targets.FirstOrDefaultAsync(t => t.Sources.Select(s => s.SourceId).Contains(sourceId));
            var docSettings = await _context.TargetDocumentsSettings.FirstOrDefaultAsync(t => t.SourceId == sourceId);
            DbConnectionSettings dbConnectionSettings = new DbConnectionSettings(target.DatabaseAddress, target.DatabaseUsername, target.DatabasePassword, target.DatabaseName);
            C21DocumentService c21DocumentService = new C21DocumentService(dbConnectionSettings, null);
            var fkfDocuments = (await c21DocumentService.GetFKDocuments(year, month, docSettings.DocumentShortcut));
            _context.Database.SetCommandTimeout(0);
            var systemDocuments = await _context.Documents.Where(d => d.SourceId == sourceId && d.DocumentDate.Month == month).ToListAsync();

            var duplicates = fkfDocuments.GroupBy(d => new { d.tresc }).Select(d => new
            {
                d.Key.tresc,
                cnt = d.Count()
            }).ToList();
            duplicates = duplicates.Where(d => d.cnt > 1).ToList();
            var duplicatedDocs = systemDocuments.Where(d => duplicates.Select(d => d.tresc).Contains(d.DocumentNumber)).ToList();
            var sourcesIds = await _context.Sources.Select(s => s.SourceId).ToListAsync();
            if (sourceId > 0)
                sourcesIds = new List<int> { sourceId };
            var contractors = await _context.Contractors.ToListAsync();
            var sources = await _context.Sources.ToListAsync();
            var documents = (
                    from d in duplicatedDocs
                    from c in contractors
                    from s in sources
                    where d.ContractorId == c.ContractorId && d.SourceId == s.SourceId
                    && sourcesIds.Contains((int)d.SourceId) && d.DocumentDate.Month == month
                    && d.DocumentDate.Year == year
                    orderby d.DocumentDate
                    select new DocumentView(d, s, c)).ToList();
            return Ok(new { data = documents });
        }

        [HttpDelete]
        [Route("DocumentsView/DeleteData")]
        public async Task<IActionResult> DeleteData([FromBody] int id)
        {
            try
            {
                var document = await _context.Documents.FirstOrDefaultAsync(t => t.DocumentId == id).ConfigureAwait(false);
                var documents = await _context.Documents.Where(t => t.TaskTicketId == document.TaskTicketId).ToListAsync();
                var ticket = await _context.TaskTickets.FirstOrDefaultAsync(t => t.TaskTicketId == document.TaskTicketId).ConfigureAwait(false);
                var tickets = await _context.TaskTickets.Where(t => t.DateFrom == ticket.DateFrom && t.DateTo == ticket.DateTo && t.SourceId == ticket.SourceId).ToListAsync().ConfigureAwait(false);
                var docVats = await _context.DocumentVats.Where(v => documents.Select(i => i.DocumentId).ToList().Contains((int)v.DocumentId)).ToListAsync().ConfigureAwait(false);
                using var transaction = _context.Database.BeginTransaction();
                _context.RemoveRange(docVats);
                _context.RemoveRange(documents);
                _context.RemoveRange(tickets);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Error");

            }
            return Ok(id);
        }

        [HttpDelete]
        [Route("DocumentsView/DeleteDataFromPeriod/{period}")]
        public async Task<IActionResult> DeleteDataFromPeriod(string period, [FromBody] int id)
        {
            if (period == null) 
                return BadRequest("The criteria are null");
            var criteria = period.Split(';');
            if (criteria.Count() != 3)
            {
                return BadRequest("The criteria were not set correctly");
            }
            int source = int.Parse(criteria[0]);
            int month = int.Parse(criteria[1]);
            int year = int.Parse(criteria[2]);


            try
            {
                var ticket = await _context.TaskTickets.FirstOrDefaultAsync(t => t.SourceId == source && t.DateFrom.Year == year && t.DateFrom.Month == month).ConfigureAwait(false);
                var documents = await _context.Documents.Where(t => t.TaskTicketId == ticket.TaskTicketId).ToListAsync();
                var tickets = await _context.TaskTickets.Where(t => t.DateFrom == ticket.DateFrom && t.DateTo == ticket.DateTo && t.SourceId == ticket.SourceId).ToListAsync().ConfigureAwait(false);
                var docVats = await _context.DocumentVats.Where(v => documents.Select(i => i.DocumentId).ToList().Contains((int)v.DocumentId)).ToListAsync().ConfigureAwait(false);
                using var transaction = _context.Database.BeginTransaction();
                _context.RemoveRange(docVats);
                _context.RemoveRange(documents);
                _context.RemoveRange(tickets);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Error");
            }

            return Ok(id);
        }
    }
}
