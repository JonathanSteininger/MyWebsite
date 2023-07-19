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

        // GET: ProjectTags/Create
        public IActionResult Create()
        {
            ViewData["ProjectKey"] = new SelectList(_context.Project, "ProjectKey", "Title");
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
            bool exists = await _context.ProjectTag.ContainsAsync(projectTag);
            if (ModelState.IsValid && !exists)
            {
                _context.Add(projectTag);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectKey"] = new SelectList(_context.Project, "ProjectKey", "Title", projectTag.ProjectKey);
            ViewData["TagID"] = new SelectList(_context.Tag, "TagID", "TagName", projectTag.TagID);
            return View(projectTag);
        }
        // GET: ProjectTags/Delete/5
        public async Task<IActionResult> Delete(int? key1, int? key2)
        {
            if (key1 == null || key2 == null || _context.ProjectTag == null)
            {
                return NotFound();
            }

            var projectTag = await _context.ProjectTag
                .Include(p => p.Project)
                .Include(p => p.Tag)
                .Where(m => m.ProjectKey == key1 && m.TagID == key2)
                .FirstOrDefaultAsync();
            if (projectTag == null)
            {
                return NotFound();
            }

            return View(projectTag);
        }

        // POST: ProjectTags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? ProjectKey, int? TagID)
        {
            if(ProjectKey == null || TagID == null)
            {
                return Problem("One of the keys were null.");
            }
            if (_context.ProjectTag == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ProjectTag'  is null.");
            }
            //var projectTag = await _context.ProjectTag
            //  .FirstOrDefaultAsync(m => m.ProjectKey == ProjectKey && m.TagID == TagID);

            var projectTag = await _context.ProjectTag
                .Where(k => k.ProjectKey == ProjectKey && k.TagID  == TagID)
                .FirstOrDefaultAsync();
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
