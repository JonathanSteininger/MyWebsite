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
    public class ProjectTagsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectTagsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProjectTags
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ProjectTag.Include(p => p.Project).Include(p => p.Tag);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ProjectTags/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProjectTag == null)
            {
                return NotFound();
            }

            var projectTag = await _context.ProjectTag
                .Include(p => p.Project)
                .Include(p => p.Tag)
                .FirstOrDefaultAsync(m => m.ProjectKey == id);
            if (projectTag == null)
            {
                return NotFound();
            }

            return View(projectTag);
        }

        // GET: ProjectTags/Create
        public IActionResult Create()
        {
            ViewData["ProjectKey"] = new SelectList(_context.Project, "ProjectKey", "Body");
            ViewData["TagID"] = new SelectList(_context.Tag, "TagID", "TagName");
            return View();
        }

        // POST: ProjectTags/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProjectKey,TagID")] ProjectTag projectTag)
        {
            if (ModelState.IsValid)
            {
                _context.Add(projectTag);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectKey"] = new SelectList(_context.Project, "ProjectKey", "Body", projectTag.ProjectKey);
            ViewData["TagID"] = new SelectList(_context.Tag, "TagID", "TagName", projectTag.TagID);
            return View(projectTag);
        }

        // GET: ProjectTags/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProjectTag == null)
            {
                return NotFound();
            }

            var projectTag = await _context.ProjectTag.FindAsync(id);
            if (projectTag == null)
            {
                return NotFound();
            }
            ViewData["ProjectKey"] = new SelectList(_context.Project, "ProjectKey", "Body", projectTag.ProjectKey);
            ViewData["TagID"] = new SelectList(_context.Tag, "TagID", "TagName", projectTag.TagID);
            return View(projectTag);
        }

        // POST: ProjectTags/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("ProjectKey,TagID")] ProjectTag projectTag)
        {
            if (id != projectTag.ProjectKey)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projectTag);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectTagExists(projectTag.ProjectKey))
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
            ViewData["ProjectKey"] = new SelectList(_context.Project, "ProjectKey", "Body", projectTag.ProjectKey);
            ViewData["TagID"] = new SelectList(_context.Tag, "TagID", "TagName", projectTag.TagID);
            return View(projectTag);
        }

        // GET: ProjectTags/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProjectTag == null)
            {
                return NotFound();
            }

            var projectTag = await _context.ProjectTag
                .Include(p => p.Project)
                .Include(p => p.Tag)
                .FirstOrDefaultAsync(m => m.ProjectKey == id);
            if (projectTag == null)
            {
                return NotFound();
            }

            return View(projectTag);
        }

        // POST: ProjectTags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (_context.ProjectTag == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ProjectTag'  is null.");
            }
            var projectTag = await _context.ProjectTag.FindAsync(id);
            if (projectTag != null)
            {
                _context.ProjectTag.Remove(projectTag);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectTagExists(int? id)
        {
          return (_context.ProjectTag?.Any(e => e.ProjectKey == id)).GetValueOrDefault();
        }
    }
}
