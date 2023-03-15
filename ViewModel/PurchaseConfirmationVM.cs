using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TeamRedInternalProject.Models;

namespace TeamRedInternalProject.ViewModel
{
    
    public class PurchaseConfirmationVM
    {
        public int OrderId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Display(Name = "Order Date:"), DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}", ApplyFormatInEditMode = true)]
        public DateTime OrderDate { get; set; }
        public string OrderEmail { get; set; }
        public string PayerEmail { get; set; }
        public Dictionary<string, int> TicketTypes { get; set; }

        public string FormatOrderDate(DateTime orderDate)
        {
            string formattedDate = this.OrderDate.ToString("MMM dd, yyyy");

            return formattedDate;

        }

    }
}
