using System;
using System.Collections.Generic;

namespace Reizen.Data.Models;

public partial class ContinentDAL
{
    public int Id
    {
        get; set;
    }

    public string Name { get; set; } = null!;

    public virtual ICollection<CountryDAL> Countries { get; set; } = new List<CountryDAL> ();
}
