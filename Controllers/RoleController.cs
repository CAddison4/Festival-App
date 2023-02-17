using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TeamRedInternalProject.Models;
using TeamRedInternalProject.Repositories;

namespace TeamRedInternalProject.Controllers
{
    public class RoleController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserRepo _userRepo;
        private readonly UserRoleRepo _userRoleRepo;
        private IServiceProvider _serviveProvider;
        private readonly ConcertContext _db;
        public RoleController(ILogger<UserController> logger, ConcertContext db, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviveProvider= serviceProvider;
            _userRepo = new UserRepo();
            _userRoleRepo = new UserRoleRepo(_serviveProvider);
            _db = db;
        }
        // GET: RoleController
        public ActionResult Index()
        {
            var users = _userRepo.GetUsers();
            return View(users);
        }

        // GET: RoleController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // Present user with ability to assign roles to a user.
        // It gives two drop downs - the first contains the user names with
        // the requested user selected. The second drop down contains all
        // possible roles.
        public ActionResult Edit(string email)
        {
            var results = _userRepo.GetUsersByEmail(email);
            return View(results);
        }

        // Assigns role to user.
        [HttpPost]
        public async Task<IActionResult> Edit(User user, string email)
        {
            string message = string.Empty;

            if (ModelState.IsValid)
            {
                var addUR = await _userRoleRepo.AddUserRole(user.Email,
                                                            user.Admin);
                bool results = _userRoleRepo.UpdateUser(email);

                if (results)
                {
                    message = "User Updated";
                }
            }
            try
            {
                return RedirectToAction("Index", "Role", message);
            }
            catch
            {
                message = "Error in updating";
                return View(message);
            }
        }
    }
}
