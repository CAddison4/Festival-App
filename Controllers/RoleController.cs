using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TeamRedInternalProject.Models;
using TeamRedInternalProject.Repositories;
using TeamRedInternalProject.ViewModel;

namespace TeamRedInternalProject.Controllers
{
    public class RoleController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserRepo _userRepo;
        private readonly UserRoleRepo _userRoleRepo;
        private IServiceProvider _serviceProvider;
        private readonly ConcertContext _db;
        public RoleController(ILogger<UserController> logger, ConcertContext db, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider= serviceProvider;
            _userRepo = new UserRepo();
            _userRoleRepo = new UserRoleRepo(serviceProvider, db);
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
        public async Task<ActionResult> Create(string email)
        {
            ViewBag.SelectedUser = email;

            RoleRepo roleRepo = new RoleRepo(_serviceProvider);
            var roles = roleRepo.GetAllRoles().ToList();
            if (roles == null || roles.Count == 0)
            {
                await roleRepo.CreateInitialRoles();
                roles = roleRepo.GetAllRoles().ToList();
            }
            
            var preRoleList = roles.Select(r => 
                new SelectListItem {  Value = r.RoleName, Text = r.RoleName }).ToList();

            var roleList = new SelectList(preRoleList, "Value", "Text");

            ViewBag.RoleSelectList = roleList;

            List<User>userList = _userRepo.GetUsers();

            var preUserList = userList.Select( u=> new SelectListItem
            {
                Value = u.Email,
                Text = u.Email
            }).ToList();

            SelectList userSelectList = new SelectList(preUserList, "Value", "Text");

            ViewBag.UserSelectList = userSelectList;
            return View();
        }

        // Assigns role to user.
        [HttpPost]
        public async Task<IActionResult> Create(UserRoleVM userRole)
        {
            string message = string.Empty;

            if (ModelState.IsValid)
            {
             await _userRoleRepo.AddUserRole(userRole.Email, userRole.Role);
             _userRoleRepo.UpdateUser(userRole.Email);

               }
            try
            {
                return RedirectToAction("Index", "Role", new { email = userRole.Email });
            }
            catch
            {
                message = "Error in updating";
                return View(message);
            }
        }
    }
}
