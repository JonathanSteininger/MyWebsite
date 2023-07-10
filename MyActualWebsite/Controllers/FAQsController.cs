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
    [Authorize(Roles = "admin")]
    public class FAQsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FAQsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: FAQs
        public async Task<IActionResult> Index()
        {
              return _context.FAQ != null ? 
                          View(await _context.FAQ.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.FAQ'  is null.");
        }

        // GET: FAQs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.FAQ == null)
            {
                return NotFound();
            }

            var fAQ = await _context.FAQ
                .FirstOrDefaultAsync(m => m.FAQ_ID == id);
            if (fAQ == null)
            {
                return NotFound();
            }

            return View(fAQ);
        }

        // GET: FAQs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FAQs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FAQ_ID,Question,Answer")] FAQ fAQ)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fAQ);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(fAQ);
        }

        // GET: FAQs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.FAQ == null)
            {
                return NotFound();
            }

            var fAQ = await _context.FAQ.FindAsync(id);
            if (fAQ == null)
            {
                return NotFound();
            }
            return View(fAQ);
        }

        // POST: FAQs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FAQ_ID,Question,Answer")] FAQ fAQ)
        {
            if (id != fAQ.FAQ_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fAQ);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FAQExists(fAQ.FAQ_ID))
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
            return View(fAQ);
        }

        // GET: FAQs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.FAQ == null)
            {
                return NotFound();
            }

            var fAQ = await _context.FAQ
                .FirstOrDefaultAsync(m => m.FAQ_ID == id);
            if (fAQ == null)
            {
                return NotFound();
            }

            return View(fAQ);
        }

        // POST: FAQs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.FAQ == null)
            {
                return Problem("Entity set 'ApplicationDbContext.FAQ'  is null.");
            }
            var fAQ = await _context.FAQ.FindAsync(id);
            if (fAQ != null)
            {
                _context.FAQ.Remove(fAQ);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FAQExists(int id)
        {
          return (_context.FAQ?.Any(e => e.FAQ_ID == id)).GetValueOrDefault();
        }
    }
}
