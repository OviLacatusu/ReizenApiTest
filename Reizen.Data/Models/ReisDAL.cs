using System;
using System.Collections.Generic;

namespace Reizen.Data.Models;

public partial class ReisDAL
{
    public int Id
    {
        get; set;
    }

    public string Bestemmingscode { get; set; } = null!;

    public DateOnly Vertrek
    {
        get; set;
    }

    public int AantalDagen
    {
        get; set;
    }

    public decimal PrijsPerPersoon
    {
        get; set;
    }

    public int AantalVolwassenen
    {
        get; set;
    }

    public int AantalKinderen
    {
        get; set;
    }

    public virtual BestemmingDAL Bestemming { get; set; } = null!;

    public virtual ICollection<BoekingDAL> Boekingen { get; set; } = new List<BoekingDAL> ();
}
