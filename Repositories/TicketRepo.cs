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

        public TicketRepo(ConcertContext db)
        {
            _db = db;
            _userRepo= new UserRepo();
        }

        /// <summary>
        /// 1. Make a ticket for the quantity of each ticket type in the order
        /// 2. Save each ticket to the database
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="currentFestivalId"></param>
        /// <param name="ticketRequests"></param>
        /// <returns>List of tickets created</returns>
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

        /// <summary>
        /// Get all the tickets purchased in a given order (with the specified orderId)
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns>List of tickets in the order</returns>
        public List<Ticket> GetTicketsByOrder(int orderId)
        {
            List<Ticket> tickets = _db.Tickets.Where(t => t.OrderId == orderId).ToList();
            return tickets;
        }
        
        /// <summary>
        /// Get all tickets at a specified festival. Default is the current festival.
        /// Can optionally include the ticket type data as well.
        /// </summary>
        /// <param name="festivalId"></param>
        /// <param name="includeTicketTypes"></param>
        /// <returns>List of tickets</returns>
        public List<Ticket> GetAllTickets(int? festivalId=null, bool includeTicketTypes=false)
        {

            IQueryable<Ticket> allTickets = (festivalId != null) ? 
                _db.Tickets.Where(t => t.FestivalId == festivalId) :
                _db.Tickets.Where(t => t.Festival.IsCurrent);

            return includeTicketTypes ? allTickets.Include(t => t.TicketType).ToList() : allTickets.ToList();
        }

        /// <summary>
        /// Get all the details of a user's ticket for the purpose of donwloading/printing the ticket.
        /// </summary>
        /// <param name="email">User's email</param>
        /// <param name="ticketId"></param>
        /// <param name="festivalId"></param>
        /// <returns>Ticket details as a UserTicketVM object</returns>
        public TicketVM GetUserTicketVM(string email, int ticketId)
        {
            // get user to access first name and last name
            User user = _userRepo.GetUsersByEmail(email);

            // get the ticket object from the db, including the associated ticket type and festival data
            Ticket ticket = _db.Tickets
                .Where(t => t.TicketId == ticketId)
                .Include(t => t.TicketType)
                .Include(t => t.Festival)
                .First();

            TicketVM ticketVM = new()
            {
                TicketId = ticket.TicketId,
                TicketType = ticket.TicketType.Type,
                Price = ticket.TicketType.Price,
                Location = ticket.Festival.Location,
                Date = ticket.Festival.Date,
                FirstName = user.FirstName,
                LastName = user.LastName,
            };

            return ticketVM;

        }

        /// <summary>
        /// Get a list of tickets belonging to a user (identified by email) for a given festival.
        /// Default is the current festival.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="festivalId"></param>
        /// <returns>List of TicketVM objects</returns>
        public List<TicketVM> GetUserTicketVMs(string email, int? festivalId=null)
        {
            // get user to access first name and last name
            User user = _userRepo.GetUsersByEmail(email);

            // get all the tickets that belong to the logged in user for the given festival
            // including the associated ticket type and festival data
            List<Ticket> allTicketsForUser = _db.Tickets
                .Where(t => t.Order.Email == email && 
                           (t.Festival.FestivalId == festivalId || t.Festival.IsCurrent)
                )
                .Include(t => t.TicketType)
                .Include(t => t.Festival)
                .ToList();

            List<TicketVM> allTicketVMsForUser = new List<TicketVM>();
            foreach(Ticket ticket in allTicketsForUser)
            {
                TicketVM ticketVM = new()
                {
                    TicketId = ticket.TicketId,
                    TicketType = ticket.TicketType.Type,
                    Price = ticket.TicketType.Price,
                    Location = ticket.Festival.Location,
                    Date = ticket.Festival.Date,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                };
                allTicketVMsForUser.Add(ticketVM);
            }

            return allTicketVMsForUser;
        }
    }
}
