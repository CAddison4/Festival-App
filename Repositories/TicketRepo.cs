using TeamRedInternalProject.Models;

namespace TeamRedInternalProject.Repositories
{
    public class TicketRepo
    {
        private readonly ConcertContext _db;
        public TicketRepo()
        {
            _db = new();
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

        public List<Ticket> GetAllTickets() {
            return _db.Tickets.ToList();
        }
    }
}
