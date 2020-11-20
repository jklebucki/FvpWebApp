using FvpWebApp.Data;
using FvpWebAppModels.Models;
using Microsoft.AspNetCore.Http;
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

        public async Task<IActionResult> GetSource(int id)
        {
            var source = await _context.Sources.FirstOrDefaultAsync(s => s.SourceId == id);
            var target = await _context.Targets.FirstOrDefaultAsync(t => t.TargetId == source.TargetId);
            if (target == null)
                target = new Target();
            else
                target.Sources = new List<Source>();
            var targetDocumentSettings = await _context.TargetDocumentsSettings.FirstOrDefaultAsync(t => t.SourceId == source.SourceId);
            if (targetDocumentSettings == null)
                targetDocumentSettings = new TargetDocumentSettings();
            var vatRegisters = await _context.VatRegisters.Where(v => v.TargetDocumentSettingsId == targetDocumentSettings.TargetDocumentSettingsId).ToListAsync();
            if (vatRegisters == null)
                vatRegisters = new List<VatRegister>();
            return new JsonResult(new
            {
                Source = source,
                Target = target,
                TargetDocumentSettings = targetDocumentSettings,
                VarRegisters = vatRegisters
            });
        }

        [HttpPost]
        public ActionResult CreateTarget(IFormCollection collection)
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

        [HttpPost]
        public ActionResult CreateSource(IFormCollection collection)
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

        [HttpPost]
        public ActionResult UpdateTarget(IFormCollection collection)
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

        [HttpPost]
        public ActionResult UpdateSource(IFormCollection collection)
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
