using Microsoft.EntityFrameworkCore;
using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Queries
{
    public sealed class GetLandenVanWerelddeel
    {
        public record GetLandenVanWerelddeelQuery(string werelddeel, ReizenContext context):IQuery<Result<IList<Land>>>;

        public class GetLandenVanWerelddeelQueryHandler : IQueryHandler<GetLandenVanWerelddeelQuery, Result<IList<Land>>>
        {
            public async Task<Result<IList<Land>>> Handle (GetLandenVanWerelddeelQuery query)
            {
                try
                {
                    if (string.IsNullOrEmpty (query.werelddeel))
                    {
                        return Result<IList<Land>>.Failure ("Werelddeel name cannot be empty");
                    }

                    var werelddeel = (await query.context.Werelddelen.ToListAsync ())
                        .Where (w => w.Naam.Contains (query.werelddeel, StringComparison.OrdinalIgnoreCase))
                        .FirstOrDefault ();

                    if (werelddeel == null)
                    {
                        return Result<IList<Land>>.Failure ($"Werelddeel with name '{query.werelddeel}' not found");
                    }

                    var result = (await query.context.Landen.ToListAsync ())
                        .Where (l => l.Werelddeelid == werelddeel.Id)
                        .ToList ();

                    return result.Count == 0
                        ? Result<IList<Land>>.Failure ("No countries found for this werelddeel")
                        : Result<IList<Land>>.Success (result);
                }
                catch (Exception ex)
                {
                    return Result<IList<Land>>.Failure ($"Error retrieving countries: {ex.Message}");
                }
            }
        }
    }
}
