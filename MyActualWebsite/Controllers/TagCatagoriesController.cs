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
    public class TagCatagoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TagCatagoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TagCatagories
        public async Task<IActionResult> Index()
        {
              return _context.TagCatagory != null ? 
                          View(await _context.TagCatagory.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.TagCatagory'  is null.");
        }

        // GET: TagCatagories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TagCatagory == null)
            {
                return NotFound();
            }

            var tagCatagory = await _context.TagCatagory
                .FirstOrDefaultAsync(m => m.CatagoryId == id);
            if (tagCatagory == null)
            {
                return NotFound();
            }

            return View(tagCatagory);
        }

        // GET: TagCatagories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TagCatagories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CatagoryId,CatagoryName")] TagCatagory tagCatagory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tagCatagory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tagCatagory);
        }

        // GET: TagCatagories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TagCatagory == null)
            {
                return NotFound();
            }

            var tagCatagory = await _context.TagCatagory.FindAsync(id);
            if (tagCatagory == null)
            {
                return NotFound();
            }
            return View(tagCatagory);
        }

        // POST: TagCatagories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CatagoryId,CatagoryName")] TagCatagory tagCatagory)
        {
            if (id != tagCatagory.CatagoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tagCatagory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TagCatagoryExists(tagCatagory.CatagoryId))
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
            return View(tagCatagory);
        }

        // GET: TagCatagories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TagCatagory == null)
            {
                return NotFound();
            }

            var tagCatagory = await _context.TagCatagory
                .FirstOrDefaultAsync(m => m.CatagoryId == id);
            if (tagCatagory == null)
            {
                return NotFound();
            }

            return View(tagCatagory);
        }

        // POST: TagCatagories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TagCatagory == null)
            {
                return Problem("Entity set 'ApplicationDbContext.TagCatagory'  is null.");
            }
            var tagCatagory = await _context.TagCatagory.FindAsync(id);
            if (tagCatagory != null)
            {
                _context.TagCatagory.Remove(tagCatagory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TagCatagoryExists(int id)
        {
          return (_context.TagCatagory?.Any(e => e.CatagoryId == id)).GetValueOrDefault();
        }
    }
}
