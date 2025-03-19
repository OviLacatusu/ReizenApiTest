using System;
using System.Collections.Generic;

namespace Reizen.Data.Models;

public partial class Werelddeel
{
    public int Id
    {
        get; set;
    }

    public string Naam { get; set; } = null!;

    public virtual ICollection<Land> Landens { get; set; } = new List<Land> ();
}
