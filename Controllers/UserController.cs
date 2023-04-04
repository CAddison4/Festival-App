﻿using Microsoft.AspNetCore;
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

namespace TeamRedInternalProject.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserRepo _userRepo;
        private readonly TicketRepo _ticketRepo;
        private readonly TicketTypeRepo _ticketTypeRepo;
        private readonly OrderRepo _orderRepo;
        private readonly FestivalRepo _festivalRepo;
        private readonly ConcertContext _db;
        private readonly AdminRepo _adminRepo;
        private readonly IWebHostEnvironment _env;
        
        public UserController(ILogger<UserController> logger, IWebHostEnvironment env)
        {
            _env = env;
            _db = new();
            _logger = logger;
            _userRepo = new UserRepo();
            _ticketRepo= new TicketRepo();
            _ticketTypeRepo = new TicketTypeRepo();
            _orderRepo = new OrderRepo();
            _festivalRepo= new FestivalRepo();
            _adminRepo = new AdminRepo(_db);
        }

        // Display all tickets according to user
        public IActionResult Index(string searchString, int? page)
        {
            string email = User.Identity.Name;
            List<TicketVM> ticketList = _ticketRepo.GetUserTicketVMs(email);
            int pageSize = 6;
            if (String.IsNullOrEmpty(searchString))
            {
                page = 1;
                return View(PaginatedList<TicketVM>.Create(ticketList, page ?? 1, pageSize));
            }
            else
            {
                List<TicketVM> searchedTicketList = _ticketRepo.GetUserTicketVMs(email).Where(t => t.TicketType.Contains(searchString)).ToList();

                page = 1;
                return View(PaginatedList<TicketVM>.Create(searchedTicketList, page ?? 1, pageSize));
            }

        }

        private void CreateTicketFile(TicketVM ticketVM)
        {

        }

        // Download Tickets
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

        //Buy Tickets
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

        //HTTP Post Create
        /// <summary>
        /// 1. Get the email of the user
        /// 2. Create a new order
        /// 3. Get the festival ID to support ticket creation
        /// 4. Create tickets
        /// </summary>
        /// <param name="purchaseDetails"></param>
        /// <returns>Response to Ajax for redirection to confirmation page</returns>
        [HttpPost]
        public string PaySuccess([FromBody] PurchaseDetailsVM purchaseDetails)
        {
            string userEmail = User!.Identity!.Name!;
            Order order = _orderRepo.CreateNewOrder(userEmail, purchaseDetails.PayerEmail);
            int currentFestivalId = _festivalRepo.GetCurrentFestivalId();
            List<Ticket> ticketList = _ticketRepo.CreateTickets(order.OrderId, currentFestivalId, purchaseDetails.TicketRequests);
            
            return JsonConvert.SerializeObject(order.OrderId);
        }

        /// <summary>
        /// Provide the user with confirmation of their ticket purchase. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Confirmation View</returns>
        public IActionResult PurchaseConfirmation(int id)
        {
            Order order = _orderRepo.GetOrderById(id);
            List<Ticket> tickets  = _ticketRepo.GetTicketsByOrder(id);
            User user = _userRepo.GetUsersByEmail(User.Identity.Name);
            PurchaseConfirmationVM orderConfirmationVM = _orderRepo.CreateOrderConfirmation(order, user, tickets);

            return View(orderConfirmationVM);
        }

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
