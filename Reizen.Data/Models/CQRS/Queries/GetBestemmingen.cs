using Microsoft.EntityFrameworkCore;
using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Queries
{
    public sealed class GetBestemmingen
    {
        public record GetBestemmingenQuery (ReizenContext context) : IQuery<Result<IList<BestemmingDAL>>>;

        public class GetBestemmingenVanLandQueryHandler : IQueryHandler<GetBestemmingenQuery, Result<IList<BestemmingDAL>>>
        {
            public async Task<Result<IList<BestemmingDAL>>> Handle (GetBestemmingenQuery query)
            {
                try
                {
                    var result = (await query.context.Bestemmingen.ToListAsync ());
                    return result.Count == 0 ? Result<IList<BestemmingDAL>>.Failure("No destinations found")
                                             : Result<IList<BestemmingDAL>>.Success(result);
                }
                catch (Exception ex) {
                    return Result<IList<BestemmingDAL>>.Failure ($"Error retrieving destinations: {ex.Message}");
                }
            }
        }
    }
}
