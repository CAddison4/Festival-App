using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TeamRedInternalProject.ViewModel;
using TeamRedInternalProject.Repositories;
using TeamRedInternalProject.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TeamRedInternalProject.Controllers
{
    public class TicketTypeController : Controller
    {
        private readonly ILogger<TicketTypeController> _logger;
        private readonly TicketTypeRepo _ticketTypeRepo;
        private readonly ConcertContext _db;
        // GET: TicketTypeController
        public TicketTypeController(ILogger<TicketTypeController> logger, ConcertContext db)
        {  
            _logger = logger;
            _ticketTypeRepo = new TicketTypeRepo();
            _db = db;
        }
            public ActionResult Index()
        {
            List<TicketType> result = _ticketTypeRepo.GetTicketTypes();
            return View(result);
        }

        // GET: TicketTypeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

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

        [HttpPost]
        public ActionResult Create(CreateTicketVM model)
        {


            // Create the TicketType object
            var ticketType = new TicketType
            {
                Type = model.TicketType.Type,
                Price = model.TicketType.Price
            };
            _db.TicketTypes.Add(ticketType);
            _db.SaveChanges();

            // Create the FestivalTicketType object using the TicketType object and the selected Festival object
            var festivalTicketType = new FestivalTicketType
            {
                TicketTypeId = ticketType.TicketTypeId,
                FestivalId = model.FestivalTicketType.FestivalId,
                Quantity = model.FestivalTicketType.Quantity
            };
            _db.FestivalTicketTypes.Add(festivalTicketType);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }        // GET: TicketTypeController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TicketTypeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TicketTypeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TicketTypeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
