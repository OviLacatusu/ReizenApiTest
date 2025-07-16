using Microsoft.EntityFrameworkCore;
using Reizen.CommonClasses;
using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Queries
{
    public sealed class GetBookings
    {
        public record GetBookingsQuery(ReizenContext context) : IQuery<Result<IList<BookingDAL>>>;

        public class GetBookingsQueryHandler : IQueryHandler<GetBookingsQuery, Result<IList<BookingDAL>>>
        {
            public async Task<Result<IList<BookingDAL>>> Handle (GetBookingsQuery query)
            {
                try
                {
                    var boekingen = await query.context.Bookings.ToListAsync ();
                    return boekingen.Count == 0 ? Result<IList<BookingDAL>>.Failure ("No bookings found")
                                                : Result<IList<BookingDAL>>.Success (boekingen);
                }
                catch (Exception ex) {
                    return Result<IList<BookingDAL>>.Failure ($"Error retrieving bookings");
                }
            }
        }
    }
}
