using System.ComponentModel.DataAnnotations;

namespace TeamRedInternalProject.ViewModel
{
    public class TicketSalesVM
    {
        public string TicketType { get; set; }
        public decimal Price { get; set; }
        public  int TicketsSold { get; set; }
        public int TicketsAvailable { get; set; }
        public decimal Revenue { get; set;}
       
    }
}
