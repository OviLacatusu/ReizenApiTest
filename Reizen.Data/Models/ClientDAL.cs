using System;
using System.Collections.Generic;

namespace Reizen.Data.Models;

public partial class ClientDAL
{
    public int Id
    {
        get; set;
    }

    public string? FamilyName { get; set; }

    public string? FirstName { get; set; }

    public string? Address { get; set; }

    public int ResidenceId
    {
        get; set;
    }

    public virtual ICollection<BookingDAL> Bookings { get; set; } = new List<BookingDAL> ();

    public virtual ResidenceDAL? Residence { get; set; }
}
