using System.ComponentModel.DataAnnotations;
using TeamRedInternalProject.Models;

namespace TeamRedInternalProject.ViewModel
{
    public class PurchaseDetailsVM
    {
        [Key]
        public string PayerEmail { get; set; }

        public TicketRequestVM TicketRequests { get; set; }
        //public string TicketRequests { get; set; }

    }
}
