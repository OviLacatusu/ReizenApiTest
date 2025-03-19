using System;
using System.Collections.Generic;

namespace Reizen.Data.Models;

public partial class Bestemming
{
    public string Code { get; set; } = null!;

    public int Landid
    {
        get; set;
    }

    public string Plaats { get; set; } = null!;

    public virtual Land Land { get; set; } = null!;

    public virtual ICollection<Reis> Reizen { get; set; } = new List<Reis> ();
}
