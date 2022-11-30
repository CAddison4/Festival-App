using System;
using System.Collections.Generic;

namespace TeamRedInternalProject.Models
{
    public partial class Festival
    {
        public Festival()
        {
            FestivalTicketTypes = new HashSet<FestivalTicketType>();
            Tickets = new HashSet<Ticket>();
            Artists = new HashSet<Artist>();
        }

        public int FestivalId { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; } = null!;

        public virtual ICollection<FestivalTicketType> FestivalTicketTypes { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }

        public virtual ICollection<Artist> Artists { get; set; }
    }
}
