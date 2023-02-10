using System;
using System.Collections.Generic;

namespace TeamRedInternalProject.Models;

public partial class Artist
{
    public int ArtistId { get; set; }

    public string ArtistName { get; set; } = null!;

    public string? ArtistBio { get; set; }

    public virtual ICollection<Festival> Festivals { get; } = new List<Festival>();
}
