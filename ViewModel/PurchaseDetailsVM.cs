using System.ComponentModel.DataAnnotations;
using TeamRedInternalProject.Models;

namespace TeamRedInternalProject.ViewModel
{
    public class PurchaseDetailsVM
    {
        [Key]
        public string PayerEmail { get; set; }

        public IEnumerable<TicketRequestVM> TicketRequests { get; set; }
    }
}
