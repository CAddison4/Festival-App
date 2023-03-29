using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TeamRedInternalProject.Models;
using TeamRedInternalProject.Repositories;
using TeamRedInternalProject.ViewModel;

// Not logged in Users, Artists, Index/Homepage, Login/Register 
namespace TeamRedInternalProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ConcertContext _db;
        private readonly ArtistRepo _artistRepo;
        private readonly FestivalRepo _festivalRepo;

        public HomeController(ILogger<HomeController> logger, ConcertContext db)
        {
            _logger = logger;
            _db = db;
            _artistRepo = new ArtistRepo(db);
            _festivalRepo = new FestivalRepo();
    }

        public IActionResult Index()
        {
            List<Artist> artists = _artistRepo.GetArtistsAtCurrentFestival();

            Festival currentFestival = _db.Festivals.Where(f => f.IsCurrent).First();

            IndexPageVM indexPageVM = new IndexPageVM() { Artists = artists, CurrentFestival = currentFestival};

            return View(indexPageVM);
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