using C2FKInterface.Data;
using C2FKInterface.Services;
using FvpWebApp.Data;
using FvpWebApp.Infrastructure;
using FvpWebApp.Models;
using FvpWebApp.Services;
using FvpWebAppModels.Models;
using Microsoft.AspNetCore.Http;
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

                    var documents = await _context.Documents.Where(d => d.ContractorId == contractor.ContractorId).ToListAsync();
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
        [Route("DocumentsView/SetValid")]
        public async Task<IActionResult> SetValid([FromBody] int documentId)
        {

            var document = await _context.Documents.FirstOrDefaultAsync(d => d.DocumentId == documentId);
            if (document == null)
                return new StatusCodeResult((int)HttpStatusCode.BadRequest);
            var contractor = await _context.Contractors.FirstOrDefaultAsync(c => c.ContractorId == document.ContractorId && c.SourceId == document.SourceId);
            if (contractor != null)
            {
                try
                {
                    contractor.ContractorStatus = ContractorStatus.Valid;
                    var documents = await _context.Documents.Where(d => d.ContractorId == document.ContractorId && d.SourceId == document.SourceId).ToListAsync();
                    documents.ForEach(d => d.DocumentStatus = DocumentStatus.Valid);
                    _context.Update(contractor);
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

        [HttpGet]
        [Route("DocumentsView/GetFkNotPresentDocuments/{sourceId}/{year}/{month}")]
        public async Task<IActionResult> GetFkNotPresentDocuments(int sourceId, int year, int month)
        {
            var target = await _context.Targets.FirstOrDefaultAsync(t => t.Sources.Select(s => s.SourceId).Contains(sourceId));
            var docSettings = await _context.TargetDocumentsSettings.FirstOrDefaultAsync(t => t.SourceId == sourceId);
            DbConnectionSettings dbConnectionSettings = new DbConnectionSettings(target.DatabaseAddress, target.DatabaseUsername, target.DatabasePassword, target.DatabaseName);
            C21DocumentService c21DocumentService = new C21DocumentService(dbConnectionSettings, null);
            var fkfDocuments = (await c21DocumentService.GetFKDocuments(year, month, docSettings.DocumentShortcut));
            _context.Database.SetCommandTimeout(0);
            var systemDocuments = await _context.Documents.Where(d => d.SourceId == sourceId && d.DocumentDate.Month == month).ToListAsync();
            //var ids = systemDocuments.Select(d => d.DocumentId).ToArray();
            //var docVats = await _context.DocumentVats.Where(v => ids.Contains((int)v.DocumentId)).ToListAsync();
            var notPresentDocs = systemDocuments.Where(d => !fkfDocuments.Select(d => d.tresc).Contains(d.DocumentNumber)).ToList();
            return Ok(new { data = notPresentDocs });
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
            //var ids = systemDocuments.Select(d => d.DocumentId).ToArray();
            //var docVats = await _context.DocumentVats.Where(v => ids.Contains((int)v.DocumentId)).ToListAsync();
            var notPresentDocs = systemDocuments.Where(d => duplicates.Select(d => d.tresc).Contains(d.DocumentNumber)).ToList();
            return Ok(new { data = notPresentDocs });
        }
    }
}
