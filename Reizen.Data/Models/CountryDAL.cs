using System;
using System.Collections.Generic;

namespace Reizen.Data.Models;

public partial class CountryDAL
{
    public int Id
    {
        get; set;
    }

    public string Name { get; set; } = null!;

    public int Continentid
    {
        get; set;
    }

    public virtual ICollection<DestinationDAL> Destinations { get; set; } = new List<DestinationDAL> ();

    public virtual ContinentDAL Continent { get; set; } = null!;
}
