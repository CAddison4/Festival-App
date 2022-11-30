using System;
using System.Collections.Generic;

namespace TeamRedInternalProject.Models
{
    public partial class TicketType
    {
        public TicketType()
        {
            FestivalTicketTypes = new HashSet<FestivalTicketType>();
            Tickets = new HashSet<Ticket>();
        }

        public int TicketTypeId { get; set; }
        public string Type { get; set; } = null!;
        public decimal Price { get; set; }

        public virtual ICollection<FestivalTicketType> FestivalTicketTypes { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
