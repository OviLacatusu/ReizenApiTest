using System;
using System.Collections.Generic;

namespace Reizen.Data.Models;

public partial class WerelddeelDAL
{
    public int Id
    {
        get; set;
    }

    public string Naam { get; set; } = null!;

    public virtual ICollection<LandDAL> Landen { get; set; } = new List<LandDAL> ();
}
