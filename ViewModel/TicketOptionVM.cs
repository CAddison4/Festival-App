using TeamRedInternalProject.Models;
using System.ComponentModel.DataAnnotations;

namespace TeamRedInternalProject.ViewModel
{
    public class TicketOptionVM
    {
        [Key]
        public int Id { get; set; }
        public TicketType TicketType { get; set; }
        public int QtyRemaining { get; set; }
    }
}
