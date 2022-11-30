using System;
using System.Collections.Generic;

namespace TeamRedInternalProject.Models
{
    public partial class FestivalTicketType
    {
        public int FestivalId { get; set; }
        public int TicketTypeId { get; set; }
        public int Quantity { get; set; }

        public virtual Festival Festival { get; set; } = null!;
        public virtual TicketType TicketType { get; set; } = null!;
    }
}
