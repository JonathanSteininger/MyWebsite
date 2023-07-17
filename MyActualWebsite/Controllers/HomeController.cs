using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyActualWebsite.Data;
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
            if(_context != null && _context.StatBar != null && _context.Project != null)
            {
                bars = _context.StatBar.ToList();
                featuredProjects = _context.Project.Where(w => w.Featured)
                    .Include(w => w.Tags)
                    .ThenInclude(w => w.TagCatagory)
                    .ToList();
            }
            if(bars == null) bars = new List<Models.StatBar>();
            bars.Sort((b,a) => a.Precentage.CompareTo(b.Precentage));
            CheckPaths(bars);
            return View(new HomeIndexTransferModel(bars, featuredProjects));
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
        [Route("Home/Portfolio")]
        [Route("Home/Portfolio/{tags?}")]
        public IActionResult Portfolio(List<Tag>? tags)
        {
            if (tags == null || tags.Count == 0)
            {
                return View(_context.Project.Include(x => x.Tags)
                    .ThenInclude(x => x.TagCatagory)
                    .ToList()
                    .FindAll(item => CheckProjectTags(item, tags))
                    );
            }
            List<Project> projects = new List<Project>();
            if(_context != null && _context.Project != null)
            {
                projects = _context.Project.Include(x => x.Tags)
                    .ThenInclude(x => x.TagCatagory)
                    .ToList()
                    .FindAll(item => CheckProjectTags(item, tags));
            }
            projects.Sort((a,b) => {
                if (a.EndDate == null) return -1;
                if (b.EndDate == null) return 1;
                return a.EndDate.Value.CompareTo(b.EndDate.Value);
                });
            return View(projects);
        }
        /*
        
        [Route("Home/Portfolio")]
        [Route("Home/Portfolio/{tags?}")]
        public IActionResult Portfolio(string? tags)
        {
            List<Project> projects = new List<Project>();
            if (_context != null && _context.Project != null)
            {

                projects = _context.Project.Include(x => x.Tags).ThenInclude(x => x.TagCatagory).ToList();
            }
            if (string.IsNullOrEmpty(tags))
            {
                return View(projects);
            }
            string[] TagsArray = tags.ToUpper().Split(',');

            projects = projects.FindAll(item => filterProject(item, TagsArray));

            projects.Sort((a, b) => {
                if (a.EndDate == null) return -1;
                if (b.EndDate == null) return 1;
                return a.EndDate.Value.CompareTo(b.EndDate.Value);
            });
            return View(projects);
        }
        */
        /*
        public IActionResult Portfolio(params string[] tags)
        {
            List<Project> projects = new List<Project>();
            if (_context != null && _context.Project != null)
            {
                projects = _context.Project.ToList();
            }
            if (tags == null || tags.Length == 0)
            {
                return View(projects);
            }
            NormalizeArray(ref tags);

            projects = projects.FindAll(item => filterProject(item, tags));

            projects.Sort((a, b) => {
                if (a.EndDate == null) return -1;
                if (b.EndDate == null) return 1;
                return a.EndDate.Value.CompareTo(b.EndDate.Value);
            });
            return View(projects);
        }

        private void NormalizeArray(ref string[] tags)
        {
            for(int i = 0; i < tags.Length; i++)
            {
                tags[i] = tags[i].ToUpper();
            }
        }
        */

        private bool filterProject(Project item, string[] tags)
        {
            string[] tagsCache = item.Tags.ConvertAll(item => item.TagName.ToUpper()).ToArray();
            foreach (string tag in tags)
            {
                if (tag.Length > 0 && tag[0] == '-')
                {
                    string newTag = tag.Substring(1);
                    if (tagsCache.Contains(newTag)) return false;
                } 
                if (!tagsCache.Contains(tag)) return false;

            }
            return true;
        }

        
        private bool CheckProjectTags(Project project, List<Tag> tags)
        {
            if (tags == null) return true;
            foreach (Tag tag in tags)
            {
                if(!project.Tags.Contains(tag)) return false;
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