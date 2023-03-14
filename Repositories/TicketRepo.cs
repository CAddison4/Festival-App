using Microsoft.EntityFrameworkCore;
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

        public Ticket CreateTicket(Ticket ticket)
        {
            try
            {
                _db.Tickets.Add(ticket);
                _db.SaveChanges();
            }
            catch
            {
                throw (new Exception("Could not add ticket"));
            }

            return ticket;
        }
        
        //Get all tickets at the current festival:
        public List<Ticket> GetAllTickets()
        {

            List<Ticket> allTickets = _db.Tickets.Where(t => t.Festival.IsCurrent).ToList();

            return allTickets;
        }

        public List<TicketVM> GetUserTicketVMs(string email)
        {
            // get user to access first name and last name
            User user = _userRepo.GetUsersByEmail(email);

            // get current festival to access location and date
            Festival currentFestival = _db.Festivals.First(f => f.IsCurrent);

            // get all the tickets that belong to the logged in user and include the navigation property of ticket type
            List<Ticket> allTicketsForUser = _db.Tickets.Where(t => t.Order.Email == email && t.Festival.IsCurrent).Include(t => t.TicketType).ToList();

            if (allTicketsForUser.Count == 0)
            {
                throw (new Exception("Tickets do not exist"));
            }

            List<TicketVM> allTicketVMsForUser = new List<TicketVM>();
            foreach(Ticket ticket in allTicketsForUser)
            {
                TicketVM ticketVM = new()
                {
                    TicketId = ticket.TicketId,
                    TicketType = ticket.TicketType.Type,
                    Price = ticket.TicketType.Price,
                    Location = currentFestival.Location,
                    Date = currentFestival.Date,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                };
                allTicketVMsForUser.Add(ticketVM);
            }

            return allTicketVMsForUser;
        }
    }
}
