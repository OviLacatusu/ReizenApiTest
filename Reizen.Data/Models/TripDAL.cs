using System;
using System.Collections.Generic;

namespace Reizen.Data.Models;

public partial class TripDAL
{
    public int Id
    {
        get; set;
    }

    public string DestinationCode { get; set; } = null!;

    public DateOnly DateOfDeparture
    {
        get; set;
    }

    public int NumberOfDays
    {
        get; set;
    }

    public decimal PricePerPerson
    {
        get; set;
    }

    public int NumberOfAdults
    {
        get; set;
    }

    public int NumberOfMinors
    {
        get; set;
    }

    public virtual DestinationDAL Destination { get; set; } = null!;

    public virtual ICollection<BookingDAL> Bookings { get; set; } = new List<BookingDAL> ();
}
