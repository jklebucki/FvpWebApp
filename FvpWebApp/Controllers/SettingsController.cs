﻿using C2FKInterface.Data;
using C2FKInterface.Services;
using FvpWebApp.Data;
using FvpWebApp.Models;
using FvpWebApp.Services;
using FvpWebAppModels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FvpWebApp.Controllers
{
    public class SettingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SettingsController(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Targets()
        {
            return View();
        }

        public ActionResult Sources()
        {
            return View();
        }
        [Route("[controller]/getTargets")]
        public async Task<IActionResult> GetTargets()
        {
            var data = await _context.Targets.ToListAsync();
            return new JsonResult(new { data = data });
        }

        [Route("[controller]/getTargetsView")]
        public async Task<IActionResult> GetTargetsView()
        {
            var data = await _context.Targets.Select(t => new { t.TargetId, t.Descryption }).ToListAsync();
            return new JsonResult(data);
        }

        public async Task<IActionResult> GetTarget(int id)
        {
            var data = await _context.Targets.FirstOrDefaultAsync(t => t.TargetId == id);
            return new JsonResult(new { target = data });
        }
        public async Task<IActionResult> GetSource(int id)
        {
            var source = await _context.Sources.FirstOrDefaultAsync(s => s.SourceId == id);
            var targetDocumentSettings = await _context.TargetDocumentsSettings.FirstOrDefaultAsync(t => t.SourceId == source.SourceId);
            if (targetDocumentSettings == null)
                targetDocumentSettings = new TargetDocumentSettings();
            var vatRegisters = await _context.VatRegisters.Where(v => v.TargetDocumentSettingsId == targetDocumentSettings.TargetDocumentSettingsId).ToListAsync();
            if (vatRegisters == null)
                vatRegisters = new List<VatRegister>();
            var accountingRecords = await _context.AccountingRecords.Where(a => a.SourceId == source.SourceId).ToListAsync();
            if (accountingRecords == null)
                accountingRecords = new List<AccountingRecord>();
            return new JsonResult(new
            {
                Source = source,
                TargetDocumentSettings = targetDocumentSettings,
            });
        }

        private DbConnectionSettings GetDbSettings(Target target)
        {
            return new DbConnectionSettings(
                    target.DatabaseAddress,
                    target.DatabaseUsername,
                    target.DatabasePassword,
                    target.DatabaseName);
        }

        [Route("[controller]/getvatregisters/{id}")]
        public async Task<IActionResult> GetVatRegisters(int id)
        {
            var target = await _context.Targets.FirstOrDefaultAsync(t => t.TargetId == id);
            C21DocumentService documentService = new C21DocumentService(GetDbSettings(target), null);
            var vatDefs = await documentService.GetAllVatRegistersDefs();
            var defList = from v in vatDefs
                          select new
                          {
                              VatRegisterId = v.id,
                              Name = v.rNazwa
                          };
            return new JsonResult(defList);
        }

        [HttpPatch]
        [Route("[controller]/clearcontractors")]
        public async Task<IActionResult> ClearContractors([FromBody] int id)
        {
            SourceService sourceService = new SourceService(_context);
            var message = await sourceService.ClearContractors(id);
            return new JsonResult(new { message = message.MessageText, isError = message.IsError });
        }

        [Route("[controller]/getdocumentdef/{targetId}/{documentShortcut}")]
        public async Task<IActionResult> GetDocumentDef(int targetId, string documentShortcut)
        {
            var target = await _context.Targets.FirstOrDefaultAsync(t => t.TargetId == targetId);
            C21DocumentService documentService = new C21DocumentService(GetDbSettings(target), null);
            var year = await documentService.GetYearId(DateTime.Now);
            var documentDef = await documentService.GetDocumentDefinition(documentShortcut, year.rokId);

            return new JsonResult(new { docExist = documentDef != null ? true : false, documentDef });
        }

        [HttpPut]
        public async Task<IActionResult> PutTarget([FromBody] Target target)
        {
            try
            {
                var targetToChange = await _context.Targets.FirstOrDefaultAsync(t => t.TargetId == target.TargetId);
                targetToChange.Descryption = target.Descryption;
                targetToChange.DatabaseName = target.DatabaseName;
                targetToChange.DatabaseAddress = target.DatabaseAddress;
                targetToChange.DatabaseUsername = target.DatabaseUsername;
                targetToChange.DatabasePassword = target.DatabasePassword;
                _context.Update(targetToChange);
                await _context.SaveChangesAsync();
                return Ok(new { Status = true, Message = "OK" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Status = true, Message = ex.InnerException != null ? ex.InnerException.Message : ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostTarget([FromBody] Target target)
        {
            try
            {
                await _context.AddAsync(target);
                await _context.SaveChangesAsync();
                return Ok(new { Status = true, Message = "OK" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Status = true, Message = ex.InnerException != null ? ex.InnerException.Message : ex.Message });
            }
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateSource([FromBody] SourceAggregate sourceAggregate)
        {
            if (sourceAggregate != null)
                try
                {
                    if (sourceAggregate.Source.SourceId == 0)
                    {
                        //add new source
                        return await Task.FromResult(Ok(new { Status = true, Message = "Dodano nowe żródło danych!" }));
                    }
                    else
                    {
                        //update source
                        SourceService sourceService = new SourceService(_context);
                        var message = await sourceService.UpdateSource(sourceAggregate);
                        if (message.IsError)
                            throw new Exception(message.MessageText);
                        return await Task.FromResult(Ok(new { Status = true, Message = "Zaktualizowano dane!" }));
                    }

                }
                catch (Exception ex)
                {
                    return await Task.FromResult(BadRequest(new { Status = true, Message = ex.InnerException != null ? ex.InnerException.Message : ex.Message }));
                }
            else
                return await Task.FromResult(BadRequest(new { Status = true, Message = "Niepoprawne dane!" }));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTarget([FromBody] SimpleRequest payload)
        {

            try
            {
                var targetToRemove = await _context.Targets.FirstOrDefaultAsync(t => t.TargetId == payload.Id);
                _context.Targets.Remove(targetToRemove);
                await _context.SaveChangesAsync();
                return Ok(new { Message = "OK" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.InnerException != null ? ex.InnerException.Message : ex.Message });
            }
        }
    }
}
