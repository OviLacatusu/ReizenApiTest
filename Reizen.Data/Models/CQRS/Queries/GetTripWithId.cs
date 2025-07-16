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
    public sealed class GetTripWithId
    {
        public record GetTripWithIdQuery(ReizenContext context, int id): IQuery<Result<TripDAL>>;

        public class GetTripWithIdQueryHandler : IQueryHandler<GetTripWithIdQuery, Result<TripDAL>>
        {
            public async Task<Result<TripDAL>> Handle (GetTripWithIdQuery query)
            {
                try
                {
                    if (query.id <= 0)
                    {
                        return Result<TripDAL>.Failure ("Invalid trip ID");
                    }

                    var trip = (await query.context.Trips
                        .Include (r => r.Destination)
                        .ToListAsync ())
                        .FirstOrDefault (r => r.Id == query.id);

                    return trip == null
                        ? Result<TripDAL>.Failure ($"Trip with ID {query.id} not found")
                        : Result<TripDAL>.Success (trip);
                }
                catch (Exception ex)
                {
                    return Result<TripDAL>.Failure ($"Error retrieving trip: {ex.Message}");
                }
            }
        }
    }
}
