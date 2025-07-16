using Microsoft.EntityFrameworkCore;
using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using Reizen.CommonClasses;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Queries
{
    public sealed class GetTripsToDestination
    {
        public record GetTripsToDestinationQuery(ReizenContext context, string destinationCode): IQuery<Result<IList<TripDAL>>>;

        public class GetTripsToDestinationQueryHandler : IQueryHandler<GetTripsToDestinationQuery, Result<IList<TripDAL>>>
        {
            public async Task<Result<IList<TripDAL>>> Handle (GetTripsToDestinationQuery query)
            {
                try
                {
                    if (string.IsNullOrEmpty (query.destinationCode))
                    {
                        return Result<IList<TripDAL>>.Failure ("Destination code cannot be empty");
                    }

                    var result = (await query.context.Trips
                        .Include (r => r.Destination)
                        .ToListAsync ())
                        .Where (r => r.DestinationCode == query.destinationCode)
                        .ToList ();

                    return result.Count == 0
                        ? Result<IList<TripDAL>>.Failure ($"No trips found for destination code '{query.destinationCode}'")
                        : Result<IList<TripDAL>>.Success (result);
                }
                catch (Exception ex)
                {
                    return Result<IList<TripDAL>>.Failure ($"Error retrieving trips: {ex.Message}");
                }
            }
        }
    }
}
