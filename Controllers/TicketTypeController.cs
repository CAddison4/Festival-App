using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TeamRedInternalProject.ViewModel;
using TeamRedInternalProject.Repositories;
using TeamRedInternalProject.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace TeamRedInternalProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TicketTypeController : Controller
    {
        private readonly ILogger<TicketTypeController> _logger;
        private readonly TicketTypeRepo _ticketTypeRepo;
        private readonly ConcertContext _db;

        public TicketTypeController(ILogger<TicketTypeController> logger, ConcertContext db)
        {  
            _logger = logger;
            _ticketTypeRepo = new TicketTypeRepo(db);
            _db = db;
        }


        /// <summary>
        /// Grabs the ticket types
        /// </summary>
        /// <returns>ActionResult Index View</returns>
            public ActionResult Index()
        {
            List<TicketType> result = _ticketTypeRepo.GetTicketTypes();
            return View(result);
        }
 
        /// <summary>
        /// Creates a new ticket type for both the TicketType table, grabs the Festivals table and the FestivalsTicketType
        /// </summary>
        /// <returns>Action Result Create View</returns>
        public ActionResult Create()
        {
            var viewModel = new CreateTicketVM
            {
                TicketType = new TicketType(),
                FestivalTicketType = new FestivalTicketType(),
                Festivals = _db.Festivals.ToList()
            };

            return View(viewModel);
        }

        /// <summary>
        /// Grabs the Data the Admin gave and saves it to the database
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Action Result Http Create</returns>
        [HttpPost]
        public ActionResult Create(CreateTicketVM model)
        {
            String result = _ticketTypeRepo.CreateTicketType(model);

            if (result == "Success")
            {
                return RedirectToAction("Index");
            }

            return View(model);
            
            

        }
    }
}
