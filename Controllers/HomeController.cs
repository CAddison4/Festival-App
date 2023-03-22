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
        private readonly ArtistRepo _artistRepo;

        public HomeController(ILogger<HomeController> logger, ConcertContext db)
        {
            _logger = logger;
            _db = db;
            _artistRepo = new ArtistRepo(db);
    }

        public IActionResult Index()
        {
            List<Artist> artists = _artistRepo.GetArtistsAtCurrentFestival();
            return View(artists);
        }

        //list of Artists Performing
        public IActionResult Artists()
        {
            
            List<Artist> artists = _artistRepo.GetArtistsAtCurrentFestival();
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