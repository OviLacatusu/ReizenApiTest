using System;
using System.Collections.Generic;

namespace Reizen.Data.Models;

public partial class DestinationDAL
{
    public string Code { get; set; } = null!;

    public int CountryId
    {
        get; set;
    }

    public string PlaceName { get; set; } = null!;

    public virtual CountryDAL Country { get; set; } = null!;

    public virtual ICollection<TripDAL> Trips { get; set; } = new List<TripDAL> ();
}
