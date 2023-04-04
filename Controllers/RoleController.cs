using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TeamRedInternalProject.Models;
using TeamRedInternalProject.Repositories;
using TeamRedInternalProject.ViewModel;
using TeamRedInternalProject.Utilities;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.IdentityModel.Tokens;

namespace TeamRedInternalProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly ILogger<TicketController> _logger;
        private readonly UserRepo _userRepo;
        private readonly UserRoleRepo _userRoleRepo;
        private IServiceProvider _serviceProvider;
        private readonly ConcertContext _db;
        public RoleController(ILogger<TicketController> logger, ConcertContext db, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider= serviceProvider;
            _userRepo = new UserRepo();
            _userRoleRepo = new UserRoleRepo(serviceProvider, db);
            _userRepo = new UserRepo();
            _db = db;
        }
        // GET: RoleController
        public ActionResult Index(string searchString, int? page)
        {
            int pageSize = 6;
            List<User> users = _userRepo.GetUsers();
            List<User> searchedUsers = _db.Users.Where(u => u.Email.Contains(searchString)).ToList();
            if (String.IsNullOrEmpty(searchString))
            {
               users = users.Where(u => u.Admin == true).ToList();
                return View(PaginatedList<User>.Create(users, page ?? 1, pageSize)); 
            }
            else
            {

                return View(PaginatedList<User>.Create(searchedUsers, page ?? 1, pageSize));
            }
        }

        //This is erroing

        [HttpPost]
        public async Task<IActionResult> Index(string email, bool IsAdmin)
        {
            string message = string.Empty;

            if (!string.IsNullOrEmpty(email))
            {


                if (IsAdmin)
                {
                    await _userRoleRepo.AddUserRole(email, "Admin");
                    _userRoleRepo.UpdateUser(email);
                } 
                
                if (!IsAdmin)
                {

                    await _userRoleRepo.RemoveUserRole(email, "Admin");
                }

            }

            try
            {
                return RedirectToAction("Index", "Role", new { email = email });
            }
            catch
            {
                message = "Error in updating";
                return View(message);
            }
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
