using System;
using System.Collections.Generic;

namespace Reizen.Data.Models;

public partial class BestemmingDAL
{
    public string Code { get; set; } = null!;

    public int Landid
    {
        get; set;
    }

    public string Plaats { get; set; } = null!;

    public virtual LandDAL Land { get; set; } = null!;

    public virtual ICollection<ReisDAL> Reizen { get; set; } = new List<ReisDAL> ();
}
