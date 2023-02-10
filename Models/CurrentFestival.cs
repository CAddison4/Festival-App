using System;
using System.Collections.Generic;

namespace TeamRedInternalProject.Models;

public partial class CurrentFestival
{
    public int FestivalId { get; set; }

    public virtual Festival Festival { get; set; } = null!;
}
