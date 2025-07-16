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
    public sealed class GetContinents
    {
        public record GetContinentsQuery(ReizenContext context): IQuery<Result<IList<ContinentDAL>>>;

        public class GetContinentsQueryHandler : IQueryHandler<GetContinentsQuery, Result<IList<ContinentDAL>>>
        {
            public async Task<Result<IList<ContinentDAL>>> Handle (GetContinentsQuery query)
            {
                try
                {
                    var continents = await query.context.Continents.ToListAsync ();
                    return continents.Count == 0
                        ? Result<IList<ContinentDAL>>.Failure ("No world regions found")
                        : Result<IList<ContinentDAL>>.Success (continents);
                }
                catch (Exception ex)
                {
                    return Result<IList<ContinentDAL>>.Failure ($"Error retrieving world regions: {ex.Message}");
                }
            }
        }
    }
}
