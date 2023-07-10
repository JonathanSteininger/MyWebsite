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
    public class MailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Mails
        public async Task<IActionResult> Index()
        {
              return _context.Mail != null ? 
                          View(await _context.Mail.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Mail'  is null.");
        }

        // GET: Mails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Mail == null)
            {
                return NotFound();
            }

            var mail = await _context.Mail
                .FirstOrDefaultAsync(m => m.MailID == id);
            if (mail == null)
            {
                return NotFound();
            }

            return View(mail);
        }

        // GET: Mails/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Mails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MailID,Name,Address,Body")] Mail mail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mail);
        }

        // GET: Mails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Mail == null)
            {
                return NotFound();
            }

            var mail = await _context.Mail.FindAsync(id);
            if (mail == null)
            {
                return NotFound();
            }
            return View(mail);
        }

        // POST: Mails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MailID,Name,Address,Body")] Mail mail)
        {
            if (id != mail.MailID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MailExists(mail.MailID))
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
            return View(mail);
        }

        // GET: Mails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Mail == null)
            {
                return NotFound();
            }

            var mail = await _context.Mail
                .FirstOrDefaultAsync(m => m.MailID == id);
            if (mail == null)
            {
                return NotFound();
            }

            return View(mail);
        }

        // POST: Mails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Mail == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Mail'  is null.");
            }
            var mail = await _context.Mail.FindAsync(id);
            if (mail != null)
            {
                _context.Mail.Remove(mail);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MailExists(int id)
        {
          return (_context.Mail?.Any(e => e.MailID == id)).GetValueOrDefault();
        }
    }
}
