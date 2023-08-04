using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyActualWebsite.Data;
using MyActualWebsite.Models;

namespace MyActualWebsite.Controllers
{
    public class StatBarCatagoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StatBarCatagoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: StatBarCatagories
        public async Task<IActionResult> Index()
        {
              return _context.StatBarCatagory != null ? 
                          View(await _context.StatBarCatagory.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.StatBarCatagory'  is null.");
        }

        // GET: StatBarCatagories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.StatBarCatagory == null)
            {
                return NotFound();
            }

            var statBarCatagory = await _context.StatBarCatagory
                .FirstOrDefaultAsync(m => m.StatBarCatagoryID == id);
            if (statBarCatagory == null)
            {
                return NotFound();
            }

            return View(statBarCatagory);
        }

        // GET: StatBarCatagories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StatBarCatagories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StatBarCatagoryID,Name")] StatBarCatagory statBarCatagory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(statBarCatagory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(statBarCatagory);
        }

        // GET: StatBarCatagories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.StatBarCatagory == null)
            {
                return NotFound();
            }

            var statBarCatagory = await _context.StatBarCatagory.FindAsync(id);
            if (statBarCatagory == null)
            {
                return NotFound();
            }
            return View(statBarCatagory);
        }

        // POST: StatBarCatagories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StatBarCatagoryID,Name")] StatBarCatagory statBarCatagory)
        {
            if (id != statBarCatagory.StatBarCatagoryID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(statBarCatagory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StatBarCatagoryExists(statBarCatagory.StatBarCatagoryID))
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
            return View(statBarCatagory);
        }

        // GET: StatBarCatagories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.StatBarCatagory == null)
            {
                return NotFound();
            }

            var statBarCatagory = await _context.StatBarCatagory
                .FirstOrDefaultAsync(m => m.StatBarCatagoryID == id);
            if (statBarCatagory == null)
            {
                return NotFound();
            }

            return View(statBarCatagory);
        }

        // POST: StatBarCatagories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.StatBarCatagory == null)
            {
                return Problem("Entity set 'ApplicationDbContext.StatBarCatagory'  is null.");
            }
            var statBarCatagory = await _context.StatBarCatagory.FindAsync(id);
            if (statBarCatagory != null)
            {
                _context.StatBarCatagory.Remove(statBarCatagory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StatBarCatagoryExists(int id)
        {
          return (_context.StatBarCatagory?.Any(e => e.StatBarCatagoryID == id)).GetValueOrDefault();
        }
    }
}
