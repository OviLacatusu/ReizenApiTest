using Microsoft.EntityFrameworkCore;
using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Queries
{
    public sealed class GetWerelddelen
    {
        public record GetWerelddelenQuery(ReizenContext context): IQuery<Result<IList<Werelddeel>>>;

        public class GetWerelddelenQueryHandler : IQueryHandler<GetWerelddelenQuery, Result<IList<Werelddeel>>>
        {
            public async Task<Result<IList<Werelddeel>>> Handle (GetWerelddelenQuery query)
            {
                try
                {
                    var werelddelen = await query.context.Werelddelen.ToListAsync ();
                    return werelddelen.Count == 0
                        ? Result<IList<Werelddeel>>.Failure ("No world regions found")
                        : Result<IList<Werelddeel>>.Success (werelddelen);
                }
                catch (Exception ex)
                {
                    return Result<IList<Werelddeel>>.Failure ($"Error retrieving world regions: {ex.Message}");
                }
            }
        }
    }
}
