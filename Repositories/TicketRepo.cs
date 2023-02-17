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
        
        //Get all tickets at the current festival:
        public List<Ticket> GetAllTickets()
        {

            List<Ticket> allTickets = _db.Tickets.Where(t => t.Festival.IsCurrent).ToList();

            return allTickets;
        }

        public List<Ticket> GetUserTickets(string email)
        {
            UserRepo userRepo= new UserRepo();
            List<Ticket> allTicketsByUser = GetAllTickets().Where(to => to.Order.Email== email).ToList();


            if (allTicketsByUser == null)
            {
                throw (new Exception("Tickets do not exist"));
            }

            return allTicketsByUser;
        }
    }
}
