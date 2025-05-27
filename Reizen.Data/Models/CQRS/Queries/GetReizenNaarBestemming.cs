using Microsoft.EntityFrameworkCore;
using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Queries
{
    public sealed class GetReizenNaarBestemming
    {
        public record GetReizenNaarBestemmingQuery(ReizenContext context, string bestemmingscode): IQuery<Result<IList<Reis>>>;

        public class GetReizenNaarBestemmingQueryHandler : IQueryHandler<GetReizenNaarBestemmingQuery, Result<IList<Reis>>>
        {
            public async Task<Result<IList<Reis>>> Handle (GetReizenNaarBestemmingQuery query)
            {
                try
                {
                    if (string.IsNullOrEmpty (query.bestemmingscode))
                    {
                        return Result<IList<Reis>>.Failure ("Destination code cannot be empty");
                    }

                    var result = (await query.context.Reizen
                        .Include (r => r.Bestemming)
                        .ToListAsync ())
                        .Where (r => r.Bestemmingscode == query.bestemmingscode)
                        .ToList ();

                    return result.Count == 0
                        ? Result<IList<Reis>>.Failure ($"No trips found for destination code '{query.bestemmingscode}'")
                        : Result<IList<Reis>>.Success (result);
                }
                catch (Exception ex)
                {
                    return Result<IList<Reis>>.Failure ($"Error retrieving trips: {ex.Message}");
                }
            }
        }
    }
}
