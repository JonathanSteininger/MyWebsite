using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyActualWebsite.Data;
using MyActualWebsite.Models;

namespace MyActualWebsite.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StatBarsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StatBarsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: StatBars
        public async Task<IActionResult> Index()
        {
              return _context.StatBar != null ? 
                          View(await _context.StatBar.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.StatBar'  is null.");
        }

        // GET: StatBars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.StatBar == null)
            {
                return NotFound();
            }

            var statBar = await _context.StatBar
                .FirstOrDefaultAsync(m => m.StatBarID == id);
            if (statBar == null)
            {
                return NotFound();
            }

            return View(statBar);
        }

        // GET: StatBars/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StatBars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StatBarID,Precentage,IconPath")] StatBar statBar)
        {
            if (ModelState.IsValid)
            {
                _context.Add(statBar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(statBar);
        }

        // GET: StatBars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.StatBar == null)
            {
                return NotFound();
            }

            var statBar = await _context.StatBar.FindAsync(id);
            if (statBar == null)
            {
                return NotFound();
            }
            return View(statBar);
        }

        // POST: StatBars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StatBarID,Precentage,IconPath")] StatBar statBar)
        {
            if (id != statBar.StatBarID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(statBar);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StatBarExists(statBar.StatBarID))
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
            return View(statBar);
        }

        // GET: StatBars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.StatBar == null)
            {
                return NotFound();
            }

            var statBar = await _context.StatBar
                .FirstOrDefaultAsync(m => m.StatBarID == id);
            if (statBar == null)
            {
                return NotFound();
            }

            return View(statBar);
        }

        // POST: StatBars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.StatBar == null)
            {
                return Problem("Entity set 'ApplicationDbContext.StatBar'  is null.");
            }
            var statBar = await _context.StatBar.FindAsync(id);
            if (statBar != null)
            {
                _context.StatBar.Remove(statBar);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StatBarExists(int id)
        {
          return (_context.StatBar?.Any(e => e.StatBarID == id)).GetValueOrDefault();
        }
    }
}
