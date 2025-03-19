using System;
using System.Collections.Generic;

namespace Reizen.Data.Models;

public partial class Klant
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

    public virtual ICollection<Boeking> Boekingens { get; set; } = new List<Boeking> ();

    public virtual Woonplaats Woonplaats { get; set; } = null!;
}
