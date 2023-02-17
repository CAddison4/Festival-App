using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TeamRedInternalProject.Models;

public partial class TicketType
{
    public int TicketTypeId { get; set; }

    [Display(Name = "Ticket Type")]
    public string Type { get; set; } = null!;

    public decimal Price { get; set; }

    public virtual ICollection<FestivalTicketType> FestivalTicketTypes { get; } = new List<FestivalTicketType>();

    public virtual ICollection<Ticket> Tickets { get; } = new List<Ticket>();
}
