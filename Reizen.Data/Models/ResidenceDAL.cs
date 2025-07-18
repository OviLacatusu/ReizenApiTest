using System;
using System.Collections.Generic;

namespace Reizen.Data.Models;

public partial class ResidenceDAL
{
    public int Id
    {
        get; set;
    }
    public int PostalCode
    {
        get; set;
    }
    public string? Name { get; set; }

    public virtual ICollection<ClientDAL> Clients { get; set; } = new List<ClientDAL> ();
}
