using EllipticCurve.Utils;
using Microsoft.EntityFrameworkCore.Storage;
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
            _ticketRepo = new();
            _ticketTypeRepo = new();
        }

        //Get Tickets - Remaining & Purchased

        public List<Ticket> GetTicketsSoldAtCurrentFestival()
        {
            List<Ticket> ticketList = new List<Ticket>();
            
            ticketList = _db.Tickets.Where(t => t.Festival.IsCurrent).ToList();
            return ticketList;
        }

        public List<TicketRevenueVM> GetRevenueFromTickets()
        {
            Dictionary<TicketType, QtyTicketsByTypeVM> qtyTicketsByType = GetQtyTicketsByType();
            List<TicketRevenueVM> ticketRevenueVMs= new List<TicketRevenueVM>();

            foreach (var item in qtyTicketsByType)
            {
                TicketRevenueVM ticketRevenueVM = new()
                {
                    TicketName = item.Key.Type,
                    TicketsSold = item.Value.QtySold,
                    Revenue = item.Value.QtySold * item.Key.Price
                };
                ticketRevenueVMs.Add(ticketRevenueVM);
            }

            return ticketRevenueVMs;
        }

        public decimal GetTotalRevenue(List<TicketRevenueVM> ticketRevenueVMs)
        {
            decimal totalRevenue = 0;
            foreach (TicketRevenueVM ticketRevenueVM in ticketRevenueVMs)
            {
                totalRevenue += ticketRevenueVM.Revenue;
            }

            return totalRevenue;
        }

        //Add Ticket Type


        public Dictionary<TicketType, QtyTicketsByTypeVM> GetQtyTicketsByType()
        {
            List<TicketType> ticketTypes = _ticketTypeRepo.GetTicketTypes(); // all ticket types in the db

            List<Ticket> tickets = _ticketRepo.GetAllTickets(); // all tickets at current festival

            Dictionary<TicketType, QtyTicketsByTypeVM> qtyTicketsByType = new();

            foreach (TicketType ticketType in ticketTypes)
            {
                int? qtyAvailable = _db.FestivalTicketTypes.Where(ftt => ftt.TicketTypeId == ticketType.TicketTypeId && ftt.Festival.IsCurrent).Select(ftt => ftt.Quantity).First();

                if(qtyAvailable == null)
                {
                    continue; // skip over the ticket Type if it is not in the current festival
                }

                int qtySold = tickets.Where(t => t.TicketTypeId == ticketType.TicketTypeId).Count();
                int qtyRemaining = (int)qtyAvailable - qtySold;

                qtyTicketsByType.Add(ticketType, new QtyTicketsByTypeVM
                {
                    QtySold = qtySold,
                    QtyAvailable = (int)qtyAvailable,
                    QtyRemaining = qtyRemaining,
                });
            }
            return qtyTicketsByType;
        }
    }
}
