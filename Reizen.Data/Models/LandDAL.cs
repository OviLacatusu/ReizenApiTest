using System;
using System.Collections.Generic;

namespace Reizen.Data.Models;

public partial class LandDAL
{
    public int Id
    {
        get; set;
    }

    public string Naam { get; set; } = null!;

    public int Werelddeelid
    {
        get; set;
    }

    public virtual ICollection<BestemmingDAL> Bestemmingen { get; set; } = new List<BestemmingDAL> ();

    public virtual WerelddeelDAL Werelddeel { get; set; } = null!;
}
