using EllipticCurve.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Net.Sockets;
using TeamRedInternalProject.Models;
using TeamRedInternalProject.ViewModel;

namespace TeamRedInternalProject.Repositories
{
    public class AdminRepo
    {
        private readonly ConcertContext _db;
        private readonly TicketRepo _ticketRepo;
        private readonly TicketTypeRepo _ticketTypeRepo;

        public AdminRepo(ConcertContext db)
        {
            _db = db;
            _ticketRepo = new(db);
            _ticketTypeRepo = new(db);
        }

        /// <summary>
        /// Gets the total tickets sold at a given festival.
        /// If no festival is specified, default is the current festival.
        /// </summary>
        /// <returns>The total number of tickets sold at the given festival</returns>
        public int GetTotalTicketsSold(int? festivalId=null)
        {
            List<Ticket> tickets = _ticketRepo.GetAllTickets(festivalId);
            return tickets.Count;
        }

        /// <summary>
        /// Get the number of tickets sold and corresponding revenue
        /// for each ticket type at a given festival.
        /// If no festival is specified, default is the current festival.
        /// </summary>
        /// <returns>A list of objects with the properties  
        ///     {TicketType, TicketsSold, Revenue}
        /// </returns>
        public List<TicketSalesVM> GetTicketSalesDataByTicketType(int? festivalId=null)
        {

            Dictionary<int, int> qtyTicketsAvailableByType = this.GetQtyTicketsAvailableByType(festivalId);
            Dictionary<int, int> qtyTicketsSoldByType = this.GetQtyTicketsSoldByType(festivalId);
            List<TicketSalesVM> ticketSalesVMs = new();

            foreach (int ticketTypeId in qtyTicketsSoldByType.Keys)
            {
                TicketType ticketType = _db.TicketTypes.Find(ticketTypeId)!;
                TicketSalesVM ticketSalesVM = new()
                {
                    TicketType = ticketType.Type,
                    TicketsSold = qtyTicketsSoldByType[ticketTypeId],
                    TicketsAvailable = qtyTicketsAvailableByType[ticketTypeId],
                    Revenue = qtyTicketsSoldByType[ticketTypeId] * ticketType.Price,
                };

                ticketSalesVMs.Add(ticketSalesVM);
            }

            return ticketSalesVMs;
        }

        /// <summary>
        /// Calculates total revenue of all tickets in the ticketRevenueVMs list
        /// </summary>
        /// <param name="ticketRevenueVMs"></param>
        /// <returns>Total Revenue</returns>
        public decimal GetTotalRevenue(List<TicketSalesVM> ticketRevenueVMs)
        {
            decimal totalRevenue = 0;
            foreach (TicketSalesVM ticketRevenueVM in ticketRevenueVMs)
            {
                totalRevenue += ticketRevenueVM.Revenue;
            }

            return totalRevenue;
        }


        /// <summary>
        /// Creates a dictionary mapping the ticketTypeId of each of the ticket types at a given festival
        /// to the quantity of tickets sold of that type at that festival.
        /// If no festival is specified, default is the current festival.
        /// </summary>
        /// <returns>The dictionary described above</returns>
        public Dictionary<int, int> GetQtyTicketsSoldByType(int? festivalId=null)
        {
            List<Ticket> tickets = _ticketRepo.GetAllTickets(festivalId, true); // all tickets at current festival

            Dictionary<int, int> qtyTicketsSoldByType = new();

            foreach (Ticket ticket in tickets)
            {
                if (qtyTicketsSoldByType.ContainsKey(ticket.TicketType.TicketTypeId))
                {
                    qtyTicketsSoldByType[ticket.TicketType.TicketTypeId]++;
                }
                else
                {
                    qtyTicketsSoldByType[ticket.TicketType.TicketTypeId] = 1;
                }
            }

            return qtyTicketsSoldByType;
        }


        /// <summary>
        /// Get the quantity of tickets available for purchase by type at the given festival
        /// If no festival is specified, the default is the current festival
        /// </summary>
        /// <returns>A dictionary mapping the ticketTypeId of each ticket type existing at the given festival 
        /// to the number of tickets available of that type</returns>
        public Dictionary<int, int> GetQtyTicketsAvailableByType(int? festivalId = null)
        {
            List<Ticket> tickets = _ticketRepo.GetAllTickets(festivalId); // all tickets at given festival
            List<TicketType> ticketTypes = _db.TicketTypes.ToList(); // list of all ticket types in the db
            Dictionary<int, int> qtyTicketsAvailableByType = new();

            foreach (TicketType ticketType in ticketTypes)
            {
                int? qtyAtCurrentFestival = this.GetQtyTicketsOfTypeById(ticketType.TicketTypeId, festivalId);

                if (qtyAtCurrentFestival == null)
                {
                    continue; // skip over the ticket type if it does not exist at the current festival
                }

                int qtySold = tickets.Where(t => t.TicketTypeId == ticketType.TicketTypeId).Count();

                int qtyAvailable = (int)qtyAtCurrentFestival - qtySold;

                qtyTicketsAvailableByType.Add(ticketType.TicketTypeId, qtyAvailable);
            }
            return qtyTicketsAvailableByType;
        }

        /// <summary>
        /// Gets the quantity of tickets of the given ticket type at a given festival
        /// If no festival is specified, the default is the current festival
        /// </summary>
        /// <param name="ticketTypeId"></param>
        /// <returns>The quantity or null (if the given ticket type does not exist at the given festival)</returns>
        private int? GetQtyTicketsOfTypeById(int ticketTypeId, int? festivalId=null)
        {
            int? qtyAtSelectedFestival = _db.FestivalTicketTypes
                    .Where(ftt => ftt.Festival.FestivalId == festivalId && ftt.TicketTypeId == ticketTypeId)
                    .Select(ftt => ftt.Quantity)
                    .FirstOrDefault();

            int? qtyAtCurrentFestival = _db.FestivalTicketTypes
                    .Where(ftt => ftt.Festival.IsCurrent && ftt.TicketTypeId == ticketTypeId)
                    .Select(ftt => ftt.Quantity)
                    .FirstOrDefault();

            return (festivalId == null) ? qtyAtCurrentFestival : qtyAtSelectedFestival;
        }

    }
}