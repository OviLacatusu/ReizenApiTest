using System;
using System.Collections.Generic;

namespace Reizen.Data.Models;

public partial class KlantDAL
{
    public int Id
    {
        get; set;
    }

    public string Familienaam { get; set; } = null!;

    public string Voornaam { get; set; } = null!;

    public string Adres { get; set; } = null!;

    public int Woonplaatsid
    {
        get; set;
    }

    public virtual ICollection<BoekingDAL> Boekingen { get; set; } = new List<BoekingDAL> ();

    public virtual WoonplaatsDAL Woonplaats { get; set; } = null!;
}
