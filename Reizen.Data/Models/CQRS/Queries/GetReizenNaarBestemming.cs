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
    public sealed class GetReizenNaarBestemming
    {
        public record GetReizenNaarBestemmingQuery(ReizenContext context, string bestemmingscode): IQuery<Result<IList<ReisDAL>>>;

        public class GetReizenNaarBestemmingQueryHandler : IQueryHandler<GetReizenNaarBestemmingQuery, Result<IList<ReisDAL>>>
        {
            public async Task<Result<IList<ReisDAL>>> Handle (GetReizenNaarBestemmingQuery query)
            {
                try
                {
                    if (string.IsNullOrEmpty (query.bestemmingscode))
                    {
                        return Result<IList<ReisDAL>>.Failure ("Destination code cannot be empty");
                    }

                    var result = (await query.context.Reizen
                        .Include (r => r.Bestemming)
                        .ToListAsync ())
                        .Where (r => r.Bestemmingscode == query.bestemmingscode)
                        .ToList ();

                    return result.Count == 0
                        ? Result<IList<ReisDAL>>.Failure ($"No trips found for destination code '{query.bestemmingscode}'")
                        : Result<IList<ReisDAL>>.Success (result);
                }
                catch (Exception ex)
                {
                    return Result<IList<ReisDAL>>.Failure ($"Error retrieving trips: {ex.Message}");
                }
            }
        }
    }
}
