using Microsoft.AspNetCore.Mvc;
using TeamRedInternalProject.Models;
using TeamRedInternalProject.Repositories;

namespace TeamRedInternalProject.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserRepo _userRepo;
        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
            _userRepo = new UserRepo();
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


        // edit user profile get method
        // get user email from session,
        public IActionResult EditProfile()
        {
            string email = User.Identity.Name;
            User user = _userRepo.GetUsersByEmail(email);

            return View(user);
        }

        [HttpPost]
        // Post edit user profile
        public IActionResult EditProfile(User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var updatedUser = _userRepo.EditUser(user);
                    //Change to redirect to MyTickets
                    //We need to get tickets for user and pass to view
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.Message = ex.Message;
                    return View(user);
                }
            }
            ViewBag.Message = "input fields invalid";
            return View(user);
        }
    }
}
