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

        /// <summary>
        /// Get info about the current festival and artists performing at the festival 
        /// in order to create a home page with relevant data.
        /// </summary>
        /// <returns>A home page view featuring the current festival</returns>
        public IActionResult Index()
        {
            List<Artist> artists = _artistRepo.GetArtists(); // get artists at current festival

            Festival currentFestival = _db.Festivals.Where(f => f.IsCurrent).First();

            IndexPageVM indexPageVM = new IndexPageVM() { Artists = artists, CurrentFestival = currentFestival};

            return View(indexPageVM);
        }

        /// <summary>
        /// Get a list of artists performing at the current festival to create a list view
        /// </summary>
        /// <returns>A list view of all artists performing at the current festival</returns>
        public IActionResult Artists()
        {
            List<Artist> artists = _artistRepo.GetArtists(); // get artists at current festival
            return View(artists);
        }

        // error view 
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}