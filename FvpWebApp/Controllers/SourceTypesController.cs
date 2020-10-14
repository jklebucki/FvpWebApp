using FvpWebApp.Data;
using FvpWebAppModels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FvpWebApp.Controllers
{
    public class SourceTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SourceTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SourceTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.SourceTypes.OrderBy(o => o.Descryption).ToListAsync());
        }

        // GET: SourceTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sourceType = await _context.SourceTypes
                .FirstOrDefaultAsync(m => m.SourceTypeId == id);
            if (sourceType == null)
            {
                return NotFound();
            }

            return View(sourceType);
        }

        // GET: SourceTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SourceTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SourceTypeId,Descryption")] SourceType sourceType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sourceType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sourceType);
        }

        // GET: SourceTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sourceType = await _context.SourceTypes.FindAsync(id);
            if (sourceType == null)
            {
                return NotFound();
            }
            return View(sourceType);
        }

        // POST: SourceTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SourceTypeId,Descryption")] SourceType sourceType)
        {
            if (id != sourceType.SourceTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sourceType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SourceTypeExists(sourceType.SourceTypeId))
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
            return View(sourceType);
        }

        // GET: SourceTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sourceType = await _context.SourceTypes
                .FirstOrDefaultAsync(m => m.SourceTypeId == id);
            if (sourceType == null)
            {
                return NotFound();
            }

            return View(sourceType);
        }

        // POST: SourceTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sourceType = await _context.SourceTypes.FindAsync(id);
            _context.SourceTypes.Remove(sourceType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SourceTypeExists(int id)
        {
            return _context.SourceTypes.Any(e => e.SourceTypeId == id);
        }
    }
}
