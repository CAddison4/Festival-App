using Microsoft.AspNetCore.Mvc;
using TeamRedInternalProject.Models;
using TeamRedInternalProject.Repositories;
using TeamRedInternalProject.ViewModel;

namespace TeamRedInternalProject.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly ConcertContext _db;
        private readonly AdminRepo _adminRepo;



        public AdminController(ILogger<AdminController> logger, ConcertContext db)
        {
            _logger = logger;
            _adminRepo = new AdminRepo(db);
            _db = db;
        }

        //Admin Reports 
        public IActionResult Index()
        {
            //Get Tickets - Remaining & Purchased

            //Get Revenue by Ticket

            List<TicketRevenueVM> ticketRevenue = _adminRepo.GetRevenueFromTickets();
            int purchasedTickets = GetTicketsPurchased();
            decimal totalRevenue = GetTicketRevenue();

            ViewData["PurchasedTickets"] = purchasedTickets;
            ViewData["TicketRevenue"] = totalRevenue;


            //Get Total Tickets Sold
            //Get Total Revenue
            //Add Ticket Type

            return View(ticketRevenue);
        }

        public int GetTicketsPurchased()
        {
            int ticketsPurchased = _adminRepo.GetTicketsSoldAtCurrentFestival().Count();
            return ticketsPurchased;
        }

        public decimal GetTicketRevenue()
        {
            List<TicketRevenueVM> ticketRevenueVMs = _adminRepo.GetRevenueFromTickets();
            decimal ticketRevenue = 0;
            foreach (TicketRevenueVM ticketRevenueVM in ticketRevenueVMs)
            {
                ticketRevenue += ticketRevenueVM.Revenue;
            }
            
            return ticketRevenue;
        }




    }
}
