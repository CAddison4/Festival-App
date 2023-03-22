using EllipticCurve.Utils;
using Microsoft.EntityFrameworkCore.Storage;
using TeamRedInternalProject.Models;
using TeamRedInternalProject.ViewModel;

namespace TeamRedInternalProject.Repositories
{
    public class AdminRepo
    {
        private readonly ConcertContext _db;

        public AdminRepo(ConcertContext db)
        {
            _db = db;
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
            List<Ticket> ticketsSold = GetTicketsSoldAtCurrentFestival();
            Dictionary<string, int> ticketTypeDict = new Dictionary<string, int>();
            List<TicketRevenueVM> ticketRevenueVMs= new List<TicketRevenueVM>();
            List<string> ticketTypes = new List<string>();

            string ticketTypeName = "";


            foreach (Ticket ticket in ticketsSold)
            {
                TicketType ticketType = _db.TicketTypes.Where(tt => tt.TicketTypeId == ticket.TicketTypeId).FirstOrDefault();
                ticketTypeName = ticketType.Type;
                ticketTypes.Add(ticketTypeName);
            }

            foreach (string type in ticketTypes)
            {
                try
                {
                    ticketTypeDict.Add(ticketTypeName, 1);
                }
                catch
                {
                    if (ticketTypeName == type)
                    {
                        int i = 0;
                        foreach (string ttn in ticketTypes)
                        {
                            i++;
                        }

                        ticketTypeDict[ticketTypeName] = i;
                    }
                }
            }

            foreach (KeyValuePair<string, int> entry in ticketTypeDict)
            { 
                TicketType ticketType = _db.TicketTypes.Where(tt => tt.Type == entry.Key).FirstOrDefault();
                decimal ticketPrice = ticketType.Price;
                decimal ticketRevenue = ticketPrice * entry.Value;

                TicketRevenueVM ticketRevenueVM = new TicketRevenueVM()
                {
                    TicketName = entry.Key,
                    TicketsSold = entry.Value,
                    Revenue = ticketRevenue,
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
                totalRevenue = totalRevenue + ticketRevenueVM.Revenue;
            }

            return totalRevenue;
           
        }

        //Add Ticket Type
    }
}
