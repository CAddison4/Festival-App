using Microsoft.AspNetCore.Mvc;
using TeamRedInternalProject.Models;

namespace TeamRedInternalProject.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        //View Ticket Details
        public IActionResult Index()
        {
            return View();
        }

        //Buy Tickets
        public IActionResult Create()
        {
            return View();
        }

        //HTTP Post Create
        [HttpPost]

        //Ticket Confirm Details
        public IActionResult Details()
        {
            return View();
        }

        //Post Details, Payment 

        //Editing their profile
        public IActionResult Edit()
        {
            return View();
        }

        //HTTP Post Edit Profile


    }
}
