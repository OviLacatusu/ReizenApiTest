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
        public record GetBestemmingenVanLandQuery(string land, ReizenContext context): IQuery<Result<IList<BestemmingDAL>>>;

        public class GetBestemmingenVanLandQueryHandler : IQueryHandler<GetBestemmingenVanLandQuery, Result<IList<BestemmingDAL>>>
        {
            public async Task<Result<IList<BestemmingDAL>>?> Handle (GetBestemmingenVanLandQuery query)
            {
                try
                {
                    if (string.IsNullOrEmpty (query.land))
                    {
                        return Result<IList<BestemmingDAL>>.Failure ($"Name of country cannot be empty or null");
                    }
                    var land = (await query.context.Landen.ToListAsync ()).Where (l => l.Naam.Contains (query.land, StringComparison.OrdinalIgnoreCase)).FirstOrDefault ();
                    if (land is null)
                    {
                        return Result<IList<BestemmingDAL>>.Failure ($"Could not find country");
                    }
                    var result = (await query.context.Bestemmingen.ToListAsync ()).Where (b => b.Landid == land.Id);

                    return result.Count () == 0 ? Result<IList<BestemmingDAL>>.Failure ("No destinations found")
                                               : Result<IList<BestemmingDAL>>.Success (result.ToList ());
                }
                catch (Exception ex) {
                    return Result<IList<BestemmingDAL>>.Failure ($"Error retrieving destinations");
                }
            }
        }
    }
}
