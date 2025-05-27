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
        public record GetBestemmingenQuery (ReizenContext context) : IQuery<Result<IList<Bestemming>>>;

        public class GetBestemmingenVanLandQueryHandler : IQueryHandler<GetBestemmingenQuery, Result<IList<Bestemming>>>
        {
            public async Task<Result<IList<Bestemming>>> Handle (GetBestemmingenQuery query)
            {
                try
                {
                    var result = (await query.context.Bestemmingen.ToListAsync ());
                    return result.Count == 0 ? Result<IList<Bestemming>>.Failure("No destinations found")
                                             : Result<IList<Bestemming>>.Success(result);
                }
                catch (Exception ex) {
                    return Result<IList<Bestemming>>.Failure ($"Error retrieving destinations: {ex.Message}");
                }
            }
        }
    }
}
