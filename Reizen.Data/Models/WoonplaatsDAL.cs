using System;
using System.Collections.Generic;

namespace Reizen.Data.Models;

public partial class WoonplaatsDAL
{
    public int Id
    {
        get; set;
    }
    public int Postcode
    {
        get; set;
    }
    public string Naam { get; set; } = null!;

    public virtual ICollection<KlantDAL> Klanten { get; set; } = new List<KlantDAL> ();
}
