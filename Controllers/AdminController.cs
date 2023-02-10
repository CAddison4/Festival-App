using Microsoft.AspNetCore.Mvc;
using TeamRedInternalProject.Models;

namespace TeamRedInternalProject.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;

        public AdminController(ILogger<AdminController> logger)
        {
            _logger = logger;
        }

        //Admin Reports 
        public IActionResult Index()
        {
            return View();
        }




    }
}
