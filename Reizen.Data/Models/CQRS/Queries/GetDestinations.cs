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
    public sealed class GetDestinations
    {
        public record GetDestinationsQuery (ReizenContext context) : IQuery<Result<IList<DestinationDAL>>>;

        public class GetDestinationsOfCountryQueryHandler : IQueryHandler<GetDestinationsQuery, Result<IList<DestinationDAL>>>
        {
            public async Task<Result<IList<DestinationDAL>>> Handle (GetDestinationsQuery query)
            {
                try
                {
                    var result = (await query.context.Destinations.ToListAsync ());
                    return result.Count == 0 ? Result<IList<DestinationDAL>>.Failure("No destinations found")
                                             : Result<IList<DestinationDAL>>.Success(result);
                }
                catch (Exception ex) {
                    return Result<IList<DestinationDAL>>.Failure ($"Error retrieving destinations: {ex.Message}");
                }
            }
        }
    }
}
