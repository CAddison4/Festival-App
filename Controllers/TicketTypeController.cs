using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TeamRedInternalProject.Models;
using TeamRedInternalProject.Repositories;

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

        // GET: TicketTypeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TicketTypeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string type, int price)
        {
            try
            {
                _ticketTypeRepo.CreateTicketType(type, price);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: TicketTypeController/Edit/5
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
