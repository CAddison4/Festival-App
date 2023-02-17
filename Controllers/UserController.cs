using Microsoft.AspNetCore.Mvc;
using TeamRedInternalProject.Models;
using TeamRedInternalProject.Repositories;
using TeamRedInternalProject.ViewModel;

namespace TeamRedInternalProject.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserRepo _userRepo;
        private readonly TicketRepo _ticketRepo;
        private readonly TicketTypeRepo _ticketTypeRepo;
        private readonly ConcertContext _db;
        public UserController(ILogger<UserController> logger)
        {
            _db = new();
            _logger = logger;
            _userRepo = new UserRepo();
            _ticketRepo= new TicketRepo();
            _ticketTypeRepo = new TicketTypeRepo();
        }

        //View Ticket Details
        public IActionResult Index()
        {
            return View();
        }

        //Buy Tickets
        public IActionResult PurchaseTickets()
        {
            //string email = User.Identity.Name;
            //User user = _userRepo.GetUsersByEmail(email);

            List<TicketType> ticketTypes = _ticketTypeRepo.GetTicketTypes();

            List<Ticket> tickets = _ticketRepo.GetAllTickets();

            List<TicketOptionVM> ticketOptions = new List<TicketOptionVM>();

            foreach (TicketType ticketType in ticketTypes)
            {
                int qtyTicketsSoldOfType = tickets.Where(t => t.Festival.IsCurrent).Where(t => t.TicketTypeId == ticketType.TicketTypeId).Count();
                int? qtyTicketsAvailableOfType = _db.FestivalTicketTypes.Where(ftt => ftt.TicketTypeId == ticketType.TicketTypeId && ftt.Festival.IsCurrent).Select(ftt => ftt.Quantity).FirstOrDefault();
                int qtyTicketsRemainingOfType = (int)((qtyTicketsAvailableOfType != null) ? qtyTicketsAvailableOfType - qtyTicketsSoldOfType : 0);

                TicketOptionVM ticketOption = new TicketOptionVM()
                {
                    TicketType = ticketType,
                    QtyRemaining = qtyTicketsRemainingOfType
                };
                ticketOptions.Add(ticketOption);
            }

            return View(ticketOptions);

        }

        ////HTTP Post Create
        //[HttpPost]
        //public IActionResult PurchaseTickets(PurchaseDetailsVM purchaseDetails)
        //{
        //    string email = User.Identity.Name;
        //    User user = _userRepo.GetUsersByEmail(email);


        //}

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
