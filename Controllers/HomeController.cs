using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TeamRedInternalProject.Models;

namespace TeamRedInternalProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ConcertContext _db;

        public HomeController(ILogger<HomeController> logger, ConcertContext db)
        {
            _logger = logger;
            _db = db;   
        }

        public IActionResult Index()
        {
            var artists = _db.Artists;
            return View();
        }

        public IActionResult Privacy()
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