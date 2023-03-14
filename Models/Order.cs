using System;
using System.Collections.Generic;

namespace TeamRedInternalProject.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public DateTime OrderDate { get; set; }

    public string PayerEmail { get; set; }

    public string Email { get; set; }

    public virtual User EmailNavigation { get; set; }

    public virtual ICollection<Ticket> Tickets { get; } = new List<Ticket>();
}
