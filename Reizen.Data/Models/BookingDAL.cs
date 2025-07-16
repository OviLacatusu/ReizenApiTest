using System;
using System.Collections.Generic;

namespace Reizen.Data.Models;

public partial class BookingDAL
{
    public int Id
    {
        get; set;
    }

    public int ClientId
    {
        get; set;
    }

    public int TripId
    {
        get; set;
    }

    public DateOnly BookedOnDate
    {
        get; set;
    }

    public int? NumberOfAdults
    {
        get; set;
    }

    public int? NumberOfMinors
    {
        get; set;
    }

    public bool HasCancellationInsurance
    {
        get; set;
    }

    public virtual ClientDAL Client { get; set; } = null!;

    public virtual TripDAL Trip { get; set; } = null!;
}
