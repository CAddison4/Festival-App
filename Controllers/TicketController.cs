using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.MSIdentity.Shared;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Protocol;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TeamRedInternalProject.Models;
using TeamRedInternalProject.Repositories;
using TeamRedInternalProject.ViewModel;
using TeamRedInternalProject.Utilities;
using Microsoft.AspNetCore.Authorization;

namespace TeamRedInternalProject.Controllers
{
    public class TicketController : Controller
    {
        private readonly ILogger<TicketController> _logger;
        private readonly UserRepo _userRepo;
        private readonly TicketRepo _ticketRepo;
        private readonly TicketTypeRepo _ticketTypeRepo;
        private readonly OrderRepo _orderRepo;
        private readonly FestivalRepo _festivalRepo;
        private readonly ConcertContext _db;
        private readonly AdminRepo _adminRepo;
        private readonly IWebHostEnvironment _env;
        
        public TicketController(ILogger<TicketController> logger, IWebHostEnvironment env, ConcertContext db)
        {
            _env = env;
            _db = db;
            _logger = logger;
            _userRepo = new UserRepo();
            _ticketRepo= new TicketRepo(db);
            _ticketTypeRepo = new TicketTypeRepo(db);
            _orderRepo = new OrderRepo(db);
            _festivalRepo= new FestivalRepo(db);
            _adminRepo = new AdminRepo(db);
        }

        /// <summary>
        /// Purchase tickets page. It is a list view of all ticket types for the current festival.
        /// Tickets can only be purchased while they are available. Users must be logged in to purchase and 
        /// use PayPal to process purchase, but anyone can view this page without being logged in. 
        /// </summary>
        /// <returns>List view of all ticket types at current festival with purchase funcitonality</returns>
        public IActionResult PurchaseTickets()
        {
            Dictionary<int, int> qtyTicketsAvailableByType = _adminRepo.GetQtyTicketsAvailableByType();

            List<TicketOptionVM> ticketOptions = new();

            foreach( var item in qtyTicketsAvailableByType) {
                TicketOptionVM ticketOption = new()
                {
                    Id = item.Key,
                    TicketType = _db.TicketTypes.Find(item.Key)!,
                    QtyAvailable = item.Value
                };
                ticketOptions.Add(ticketOption);
            }

            return View(ticketOptions);

        }

        /// <summary>
        /// Process successful ticket payment:
        /// 1. Get the email of the logged in user
        /// 2. Create a new order
        /// 3. Get the festival ID to support ticket creation
        /// 4. Create tickets
        /// </summary>
        /// <param name="purchaseDetails">purchase details from the PayPal successful response</param>
        /// <returns>Response to Ajax for redirection to confirmation page</returns>
        [Authorize]
        [HttpPost]
        public string PaySuccess([FromBody] PurchaseDetailsVM purchaseDetails)
        {
            string userEmail = User!.Identity!.Name!;
            Order order = _orderRepo.CreateNewOrder(userEmail, purchaseDetails.PayerEmail);
            int currentFestivalId = _festivalRepo.GetCurrentFestivalId();
            _ticketRepo.CreateTickets(order.OrderId, currentFestivalId, purchaseDetails.TicketRequests);
            
            return JsonConvert.SerializeObject(order.OrderId);
        }

        /// <summary>
        /// Provide the user with confirmation of their ticket purchase for a given order. 
        /// </summary>
        /// <param name="id">Order id</param>
        /// <returns>Confirmation View</returns>
        [Authorize]
        public IActionResult PurchaseConfirmation(int id)
        {
            Order order = _orderRepo.GetOrderById(id);
            List<Ticket> tickets  = _ticketRepo.GetTicketsByOrder(id);
            User user = _userRepo.GetUsersByEmail(User.Identity!.Name!);
            PurchaseConfirmationVM orderConfirmationVM = _orderRepo.CreateOrderConfirmation(order, user, tickets);

            return View(orderConfirmationVM);
        }

        /// <summary>
        /// MyTickets page for a logged in user. Displays all tickets that the user has purchased.
        /// View is a paginated list with a search function on ticket type.
        /// </summary>
        /// <param name="searchString">search string for ticket type filtering</param>
        /// <param name="page">selected page</param>
        /// <returns>List view of tickets for the logged in user</returns>
        [Authorize] // only allow logged in users to see this view
        public IActionResult MyTickets(string searchString, int? page)
        {
            string email = User.Identity!.Name!;
            List<TicketVM> ticketList = _ticketRepo.GetUserTicketVMs(email);
            int pageSize = 6;
            if (String.IsNullOrEmpty(searchString))
            {

                return View(PaginatedList<TicketVM>.Create(ticketList, page ?? 1, pageSize));
            }
            else
            {
                List<TicketVM> searchedTicketList = _ticketRepo
                    .GetUserTicketVMs(email)
                    .Where(t => t.TicketType.ToLower().Contains(searchString.ToLower()))
                    .ToList();

                page = 1;
                return View(PaginatedList<TicketVM>.Create(searchedTicketList, page ?? 1, pageSize));
            }

        }

        /// <summary>
        /// Download a ticket -> currently downloads all ticket info in json format to a text file,
        /// but will eventually be upgraded to a printable PDF
        /// </summary>
        /// <param name="ticketId">The id of the ticket</param>
        /// <returns>Text file, auto downloaded by browser</returns>
        [Authorize]
        public FileResult DownloadTicket(int ticketId)
        {
            string email = User.Identity!.Name!;
            TicketVM ticketVM = _ticketRepo.GetUserTicketVM(email, ticketId);
            string filePath = Path.Combine(Path.GetFullPath(Environment.CurrentDirectory), "Output", $"ticket{ticketId}.txt");
            using (StreamWriter sw = new(filePath))
            {
                sw.Write(ticketVM.ToJson());
            }
            var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None, 4096, FileOptions.DeleteOnClose);
            return File(
                fileStream: fs,
                contentType: System.Net.Mime.MediaTypeNames.Application.Octet,
                fileDownloadName: $"ticket{ticketId}.txt");
        }
    }
}
