using System;
using System.Collections.Generic;

namespace TeamRedInternalProject.Models;

public partial class Festival
{
    public int FestivalId { get; set; }

    public DateTime Date { get; set; }

    public string Location { get; set; } = null!;

    public virtual CurrentFestival? CurrentFestival { get; set; }

    public virtual ICollection<FestivalTicketType> FestivalTicketTypes { get; } = new List<FestivalTicketType>();

    public virtual ICollection<Ticket> Tickets { get; } = new List<Ticket>();

    public virtual ICollection<Artist> Artists { get; } = new List<Artist>();
}
