using System;
using System.Collections.Generic;

namespace Reizen.Data.Models;

public partial class Woonplaats
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

    public virtual ICollection<Klant> Klanten { get; set; } = new List<Klant> ();
}
