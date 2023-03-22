using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Sockets;
using TeamRedInternalProject.Models;
using TeamRedInternalProject.ViewModel;

namespace TeamRedInternalProject.Repositories
{
    public class TicketRepo
    {
        private readonly ConcertContext _db;
        private readonly UserRepo _userRepo;

        public TicketRepo()
        {
            _db = new();
            _userRepo= new UserRepo();
        }

        //public Ticket CreateTicket(Ticket ticket)
        //{
        //    try
        //    {
        //        _db.Tickets.Add(ticket);
        //        _db.SaveChanges();
        //    }
        //    catch
        //    {
        //        throw (new Exception("Could not add ticket"));
        //    }

        //    return ticket;
        //}

        /// <summary>
        /// 1. Make a ticket for the quantity of each ticket type in the order
        /// 2. Save each ticket to the database
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="currentFestivalId"></param>
        /// <param name="ticketRequests"></param>
        /// <returns></returns>
        public List<Ticket> CreateTickets(int orderId, int currentFestivalId, IEnumerable<TicketRequestVM> ticketRequests)
        {
            List<Ticket> ticketsList = new List<Ticket>();
            foreach (TicketRequestVM ticketRequest in ticketRequests)
            {
                for (int i = 0; i < ticketRequest.quantity; i++)
                {

                    TicketType? ticketType = _db.TicketTypes.Where(tt => tt.Type == ticketRequest.ticketType).FirstOrDefault();
                    if (ticketType == null)
                    {
                        throw (new Exception("Requested ticket type does not exist"));
                        //return "Requested ticket type does not exist";
                    }

                    int ticketTypeId = ticketType.TicketTypeId;

                    Ticket ticket = new Ticket()
                    {
                        OrderId = orderId,
                        FestivalId = currentFestivalId,
                        TicketTypeId = ticketTypeId,
                    };
                    try
                    {
                        ticketsList.Add(ticket);
                        _db.Tickets.Add(ticket);
                        _db.SaveChanges();
                    }
                    catch
                    {
                        throw (new Exception("Could not add ticket"));
                    }

                }

            }
            return ticketsList;
        }

        public List<Ticket> GetTicketsByOrder(int orderId)
        {
            List<Ticket> tickets = _db.Tickets.Where(t => t.OrderId == orderId).ToList();
            return tickets;
        }
        
        //Get all tickets at a festival (current festival by default):
        public List<Ticket> GetAllTickets(int? festivalId=null)
        {

            List<Ticket> allTickets = (festivalId != null) ? 
                _db.Tickets.Where(t => t.FestivalId == festivalId).ToList() :
                _db.Tickets.Where(t => t.Festival.IsCurrent).ToList();

            return allTickets;
        }

        public TicketVM GetUserTicketVM(string email, int ticketId, int? festivalId=null)
        {
            // get user to access first name and last name
            User user = _userRepo.GetUsersByEmail(email);

            // get current festival to access location and date
            Festival festival = _db.Festivals.First(f => (festivalId != null) ? f.FestivalId == festivalId : f.IsCurrent );

            Ticket ticket = _db.Tickets.Where(t => t.TicketId == ticketId).Include(t => t.TicketType).First();

            TicketVM ticketVM = new()
            {
                TicketId = ticket.TicketId,
                TicketType = ticket.TicketType.Type,
                Price = ticket.TicketType.Price,
                Location = festival.Location,
                Date = festival.Date,
                FirstName = user.FirstName,
                LastName = user.LastName,
            };

            return ticketVM;

        }

        public List<TicketVM> GetUserTicketVMs(string email, int? festivalId=null)
        {
            // get user to access first name and last name
            User user = _userRepo.GetUsersByEmail(email);

            // get festival to access location and date
            Festival festival = _db.Festivals.First(f => (festivalId != null) ? f.FestivalId == festivalId : f.IsCurrent );

            // get all the tickets that belong to the logged in user and include the navigation property of ticket type
            List<Ticket> allTicketsForUser = _db.Tickets.Where(t => t.Order.Email == email && t.Festival.IsCurrent).Include(t => t.TicketType).ToList();

            List<TicketVM> allTicketVMsForUser = new List<TicketVM>();
            foreach(Ticket ticket in allTicketsForUser)
            {
                TicketVM ticketVM = new()
                {
                    TicketId = ticket.TicketId,
                    TicketType = ticket.TicketType.Type,
                    Price = ticket.TicketType.Price,
                    Location = festival.Location,
                    Date = festival.Date,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                };
                allTicketVMsForUser.Add(ticketVM);
            }

            return allTicketVMsForUser;
        }
    }
}
