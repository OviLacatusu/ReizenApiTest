using Microsoft.EntityFrameworkCore;
using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Queries
{
    public sealed class GetBestemmingenVanLand
    {
        public record GetBestemmingenVanLandQuery(string land, ReizenContext context): IQuery<Result<IList<Bestemming>>>;

        public class GetBestemmingenVanLandQueryHandler : IQueryHandler<GetBestemmingenVanLandQuery, Result<IList<Bestemming>>>
        {
            public async Task<Result<IList<Bestemming>>?> Handle (GetBestemmingenVanLandQuery query)
            {
                try
                {

                    if (string.IsNullOrEmpty (query.land))
                    {
                        return Result<IList<Bestemming>>.Failure ($"Name of country cannot be empty or null");
                    }
                    var land = (await query.context.Landen.ToListAsync ()).Where (l => l.Naam.Contains (query.land, StringComparison.OrdinalIgnoreCase)).FirstOrDefault ();
                    if (land is null)
                    {
                        return Result<IList<Bestemming>>.Failure ($"Could not find country");
                    }
                    var result = (await query.context.Bestemmingen.ToListAsync ()).Where (b => b.Landid == land.Id);

                    return result.Count () == 0 ? Result<IList<Bestemming>>.Failure ("No destinations found")
                                               : Result<IList<Bestemming>>.Success (result.ToList ());
                }
                catch (Exception ex) {
                    return Result<IList<Bestemming>>.Failure ($"Error retrieving destinations");
                }
            }
        }
    }
}
