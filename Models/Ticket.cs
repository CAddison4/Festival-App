using System;
using System.Collections.Generic;

namespace TeamRedInternalProject.Models;

public partial class Ticket
{
    public int TicketId { get; set; }

    public int OrderId { get; set; }

    public int FestivalId { get; set; }

    public int TicketTypeId { get; set; }

    public virtual Festival Festival { get; set; }

    public virtual Order Order { get; set; }

    public virtual TicketType TicketType { get; set; }
}
