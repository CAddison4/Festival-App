using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TeamRedInternalProject.Data;
using TeamRedInternalProject.Models;

namespace TeamRedInternalProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ConcertContext _db;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _db = new ConcertContext();
        }

        public IActionResult Index()
        {
            
            var artists = _db.Artists.ToList();
            return View(artists);
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