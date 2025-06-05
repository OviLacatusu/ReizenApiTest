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
        public record GetWerelddelenQuery(ReizenContext context): IQuery<Result<IList<WerelddeelDAL>>>;

        public class GetWerelddelenQueryHandler : IQueryHandler<GetWerelddelenQuery, Result<IList<WerelddeelDAL>>>
        {
            public async Task<Result<IList<WerelddeelDAL>>> Handle (GetWerelddelenQuery query)
            {
                try
                {
                    var werelddelen = await query.context.Werelddelen.ToListAsync ();
                    return werelddelen.Count == 0
                        ? Result<IList<WerelddeelDAL>>.Failure ("No world regions found")
                        : Result<IList<WerelddeelDAL>>.Success (werelddelen);
                }
                catch (Exception ex)
                {
                    return Result<IList<WerelddeelDAL>>.Failure ($"Error retrieving world regions: {ex.Message}");
                }
            }
        }
    }
}
