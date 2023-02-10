using System;
using System.Collections.Generic;

namespace TeamRedInternalProject.Models;

public partial class User
{
    public string Email { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public bool Admin { get; set; }

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
