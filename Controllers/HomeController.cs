using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TeamRedInternalProject.Models;
using TeamRedInternalProject.Repositories;

// Not logged in Users, Artists, Index/Homepage, Login/Register 
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
            return View();
        }

        //list of Artists Performing
        public IActionResult Artists()
        {
            ArtistRepo artistRepo = new ArtistRepo(_db);
            List<Artist> artists = artistRepo.GetArtists();
            return View(artists);
            //return View();
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