using Microsoft.EntityFrameworkCore;
using Reizen.Data.Repositories;
using Reizen.CommonClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Queries
{
    public sealed class GetBookingWithId
    {
        public record GetBookingWithIdQuery(int id, ReizenContext context): IQuery<Result<BookingDAL?>>;

        public class GetBookingWithIdQueryHandler : IQueryHandler<GetBookingWithIdQuery, Result<BookingDAL?>>
        {
            public async Task<Result<BookingDAL?>> Handle (GetBookingWithIdQuery query)
            {
                try
                {
                    if (query.id <= 0)
                    {
                        return Result<BookingDAL?>.Failure ("Invalid booking ID");
                    }

                    var boeking = await query.context.Bookings.FirstOrDefaultAsync (el => el.Id == query.id);
                    return boeking == null
                        ? Result<BookingDAL?>.Failure ($"Booking with ID {query.id} not found")
                        : Result<BookingDAL?>.Success (boeking);
                }
                catch (Exception ex)
                {
                    return Result<BookingDAL?>.Failure ($"Error retrieving booking: {ex.Message}");
                }
            }
        }
    }
}
