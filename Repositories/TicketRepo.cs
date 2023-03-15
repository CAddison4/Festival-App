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

        public TicketRepo()
        {
            _db = new();
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
