using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FvpWebApp.Data;
using FvpWebAppModels.Models;

namespace FvpWebApp.Controllers
{
    public class AccountingRecordsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountingRecordsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AccountingRecords
        public async Task<IActionResult> Index()
        {
            return View(await _context.AccountingRecords.ToListAsync());
        }

        // GET: AccountingRecords/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accountingRecord = await _context.AccountingRecords
                .FirstOrDefaultAsync(m => m.AccountingRecordId == id);
            if (accountingRecord == null) 
             {
                return NotFound();
            }

            return View(accountingRecord);
        }

        // GET: AccountingRecords/Create
        public async Task<IActionResult> Create()
        {
            var sources = await _context.Sources.ToListAsync();
            var selectListItems = sources.Select(s => new SelectListItem { Value = s.SourceId.ToString(), Text = s.Description });
            ViewBag.SourceId = selectListItems;
            return View();
        }

        // POST: AccountingRecords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountingRecordId,SourceId,RecordOrder,Account,DebitCredit,Sign")] AccountingRecord accountingRecord)
        {
            if (ModelState.IsValid)
            {
                _context.Add(accountingRecord);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(accountingRecord);
        }

        // GET: AccountingRecords/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accountingRecord = await _context.AccountingRecords.FindAsync(id);
            if (accountingRecord == null)
            {
                return NotFound();
            }
            return View(accountingRecord);
        }

        // POST: AccountingRecords/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AccountingRecordId,SourceId,RecordOrder,Account,DebitCredit,Sign")] AccountingRecord accountingRecord)
        {
            if (id != accountingRecord.AccountingRecordId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(accountingRecord);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountingRecordExists(accountingRecord.AccountingRecordId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(accountingRecord);
        }

        // GET: AccountingRecords/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accountingRecord = await _context.AccountingRecords
                .FirstOrDefaultAsync(m => m.AccountingRecordId == id);
            if (accountingRecord == null)
            {
                return NotFound();
            }

            return View(accountingRecord);
        }

        // POST: AccountingRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var accountingRecord = await _context.AccountingRecords.FindAsync(id);
            _context.AccountingRecords.Remove(accountingRecord);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccountingRecordExists(int id)
        {
            return _context.AccountingRecords.Any(e => e.AccountingRecordId == id);
        }
    }
}
