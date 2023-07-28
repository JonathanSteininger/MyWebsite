﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyActualWebsite.Data;
using MyActualWebsite.Data.Migrations;
using MyActualWebsite.Models;
using NuGet.Protocol;
using System.Diagnostics;
using System.IO;

namespace MyActualWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private ApplicationDbContext _context;

        const string DEFAULT_ICON_PATH = "c.png";

        private IWebHostEnvironment _webHostEnvironment;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment env, ApplicationDbContext context)
        {
            _webHostEnvironment = env;
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            List<StatBar> bars = new List<StatBar>();
            List<Project> featuredProjects = new List<Project>();
            List<Experience> experiences = new List<Experience>();
            if(_context == null)
            {
                return View(new HomeIndexTransferModel(bars, featuredProjects, experiences));
            }

            if(_context.Project != null)
            {
                featuredProjects = _context.Project.Where(w => w.Featured)
                    .Include(w => w.Tags)
                    .ThenInclude(w => w.TagCatagory)
                    .ToList();
                featuredProjects.Sort((a, b) => {
                    if (a.EndDate == null && b.EndDate == null)
                    {
                        return b.StartDate.Value.CompareTo(a.StartDate.Value);
                    }
                    if (a.EndDate == null) return -1;
                    if (b.EndDate == null) return 1;
                    return b.EndDate.Value.CompareTo(a.EndDate.Value);
                });
            }
            if(_context.StatBar != null)
            {
                bars = _context.StatBar.ToList();
                bars.Sort((b,a) => a.Precentage.CompareTo(b.Precentage));
                CheckPaths(bars);
            }
            if(_context.Experience != null)
            {
                experiences = _context.Experience.Where(w => w.Shown).ToList();
                experiences.Sort((a,b) => {
                    if (a.EndDate == null && b.EndDate == null)
                    {
                        return b.StartDate.CompareTo(a.StartDate);
                    }
                    if (a.EndDate == null) return -1;
                    if (b.EndDate == null) return 1;
                    return b.EndDate.Value.CompareTo(a.EndDate.Value);
                });
            }
            return View(new HomeIndexTransferModel(bars, featuredProjects, experiences));
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Owner")]
        [Route("/Home/Mail")]
        [Route("/Home/Mail/{amount?}")]
        public async Task<IActionResult> Mail(int? amount)
        {
            int AmountShown = 50;
            if (amount != null)
            {
                AmountShown = (int)amount;
            }
            if (_context == null || _context.Mail == null)
            {
                return View(new List<Mail>());
            }
            List<Mail> temp = await _context.Mail.ToListAsync();
            temp.Sort((b, a) => a.MailID.CompareTo(b.MailID));

            if(temp.Count > AmountShown)
            {
                temp.RemoveRange(AmountShown, temp.Count - AmountShown);
            }

            foreach(Mail mail in temp)
            {
                if (mail.Read) continue;
                mail.IsNotRead = true;
                mail.Read = true;
                _context.Update(mail);
            }

            await _context.SaveChangesAsync();
            return View(temp);
        }


        [HttpPost]
        public async Task<IActionResult> CreateMail([Bind("MailID,Name,Address,Body")] Mail mail)
        {
            if (ModelState.IsValid)
            {
                mail.DateSent = DateTime.Now;
                _context.Add(mail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(MailSent));
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult MailSent()
        {
            return View();
        }

        private void CheckPaths(List<StatBar> bars)
        {
            foreach (StatBar bar in bars)
            {

                string wwwrootPath = _webHostEnvironment.WebRootPath;
                string path = Path.Combine(wwwrootPath, $"Content\\Images\\icons\\{bar.IconPath}");
                if (System.IO.File.Exists(path)) continue;
                bar.IconPath = DEFAULT_ICON_PATH;
            }
        }

        public async Task<IActionResult> AboutMe()
        {
            if(_context == null || _context.FAQ == null)
            {
                return View(new List<FAQ>());
            }

            return View(await _context.FAQ.ToListAsync());
        }

        public async Task<IActionResult> Project(int? id)
        {
            if (id == null || _context == null || _context.Project == null)
            {
                return Redirect("Home/index");
            }
            Project project = await _context.Project.Include(w => w.Tags).ThenInclude(w => w.TagCatagory).FirstOrDefaultAsync(w => w.ProjectKey == id);
            if (project == null || project == default)
            {
                return Redirect("Home/index");
            }
            return View(project);
        }

        public IActionResult Contacts()
        {
            return View();
        }
        public IActionResult Experience()
        {
            return View();
        }
        public IActionResult FAQ()
        {
            return View();
        }
        [HttpGet]
        [Route("Home/Portfolio")]
        [Route("Home/Portfolio/{tags?}")]
        public async Task<IActionResult> Portfolio(HomePortfolioTransferModel? tags)
        {
            if (tags == null) tags = new HomePortfolioTransferModel();
            Tag[] tempAllTags = await _context.Tag.Include(w => w.TagCatagory).ToArrayAsync();

            Dictionary<int, TagCheckBoxStorage> allTags = new Dictionary<int, TagCheckBoxStorage>();
            foreach (Tag tag in tempAllTags)
            {
                allTags.Add(tag.TagID, new TagCheckBoxStorage() { Tag = tag, IsChecked = false });
            }

            if (tags != null && tags.TagsSelection.Count != 0)
            {
                foreach (KeyValuePair<int, TagCheckBoxStorage> tag in tags.TagsSelection)
                {
                    if (allTags.ContainsKey(tag.Key))
                    {
                        allTags[tag.Key].IsChecked = tag.Value.IsChecked;
                    }
                }
                
            }

            List<Project> projects = new List<Project>();
            if (_context != null && _context.Project != null)
            {
                projects = _context.Project.Include(x => x.Tags)
                    .ThenInclude(x => x.TagCatagory)
                    .ToList()
                    .FindAll(item => CheckProjectTags(item, allTags));
            }
            projects.Sort((a, b) => {
                if (a.EndDate == null && b.EndDate == null)
                {
                    return b.StartDate.Value.CompareTo(a.StartDate.Value);
                }
                if (a.EndDate == null) return -1;
                if (b.EndDate == null) return 1;
                return b.EndDate.Value.CompareTo(a.EndDate.Value);
            });
            if (tags != null && tags.TagsSelection.Count != 0)
            {
                foreach (Project proj in projects)
                {
                    foreach (Tag tag in proj.Tags)
                    {
                        if (allTags.ContainsKey(tag.TagID))
                        {
                            tag.isChecked = allTags[tag.TagID].IsChecked;
                        }
                    }
                }

            }
            

            return View(new HomePortfolioTransferModel() { Projects = projects, TagsSelection = allTags });
        }
        /// <summary>
        /// Runs return true if all the tags that are true are stored inside the project.
        /// 
        /// if a single true tag is missing, it will return false.
        /// </summary>
        /// <param name="project">the project being checked</param>
        /// <param name="tags">all tags, the value isChecked needs to be true if you want to check if its in the project tags</param>
        /// <returns>if the chosen tags are stored inside the project's tags</returns>
        private bool CheckProjectTags(Project project, Dictionary<int,TagCheckBoxStorage> tags)
        {
            if (tags == null) return true;
            foreach (KeyValuePair<int, TagCheckBoxStorage> tag in tags)
            {
                if (tag.Value.IsChecked)
                {
                    bool ContainsTag = false;
                    foreach(Tag t in project.Tags)
                    {
                        if(t.TagID == tag.Value.Tag.TagID)
                        {
                            ContainsTag = true;
                            break;
                        }
                    }
                    if (!ContainsTag) return false;
                }
            }
            return true;
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}