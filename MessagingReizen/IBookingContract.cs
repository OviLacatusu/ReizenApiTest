using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingReizen
{
    public interface IBookingContract
    {
            public int Id
            {
                get;
            }
            public int ClientId
            {
                get;
            }

            public int TripId
            {
                get;
            }
            public DateOnly BookedOnDate
            {
                get;
            }
            public int? NumberOfAdults
            {
                get;
            }
            public int? NumberOfMinors
            {
                get;
            }
            public bool HasCancellationInsurance
            {
                get;
            }
        
    }
}
