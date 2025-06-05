using System;
using System.Collections.Generic;

namespace Reizen.Data.Models;

public partial class BoekingDAL
{
    public int Id
    {
        get; set;
    }

    public int Klantid
    {
        get; set;
    }

    public int Reisid
    {
        get; set;
    }

    public DateOnly GeboektOp
    {
        get; set;
    }

    public int? AantalVolwassenen
    {
        get; set;
    }

    public int? AantalKinderen
    {
        get; set;
    }

    public bool AnnulatieVerzekering
    {
        get; set;
    }

    public virtual KlantDAL Klant { get; set; } = null!;

    public virtual ReisDAL Reis { get; set; } = null!;
}
