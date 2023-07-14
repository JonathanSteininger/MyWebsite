using Microsoft.AspNetCore.Mvc;
using MyActualWebsite.Data;
using MyActualWebsite.Models;
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
            List<Models.StatBar> bars = new List<StatBar>();
            if(_context != null && _context.StatBar != null)
            {
                bars = _context.StatBar.ToList();
            }
            if(bars == null) bars = new List<Models.StatBar>();
            bars.Sort((b,a) => a.Precentage.CompareTo(b.Precentage));
            CheckPaths(bars);
            return View(bars);
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
        public IActionResult Portfolio()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}