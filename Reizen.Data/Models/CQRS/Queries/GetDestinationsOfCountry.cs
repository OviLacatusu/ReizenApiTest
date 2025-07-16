using Microsoft.EntityFrameworkCore;
using Reizen.CommonClasses;
using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Queries
{
    public sealed class GetDestinationsOfCountry
    {
        public record GetDestinationsOfCountryQuery(string Country, ReizenContext context): IQuery<Result<IList<DestinationDAL>>>;

        public class GetDestinationsOfCountryQueryHandler : IQueryHandler<GetDestinationsOfCountryQuery, Result<IList<DestinationDAL>>>
        {
            public async Task<Result<IList<DestinationDAL>>?> Handle (GetDestinationsOfCountryQuery query)
            {
                try
                {
                    if (string.IsNullOrEmpty (query.Country))
                    {
                        return Result<IList<DestinationDAL>>.Failure ($"Name of country cannot be empty or null");
                    }
                    var Country = (await query.context.Countries.ToListAsync ()).Where (l => l.Name.Contains (query.Country, StringComparison.OrdinalIgnoreCase)).FirstOrDefault ();
                    if (Country is null)
                    {
                        return Result<IList<DestinationDAL>>.Failure ($"Could not find country");
                    }
                    var result = (await query.context.Destinations.ToListAsync ()).Where (b => b.CountryId == Country.Id);

                    return result.Count () == 0 ? Result<IList<DestinationDAL>>.Failure ("No destinations found")
                                               : Result<IList<DestinationDAL>>.Success (result.ToList ());
                }
                catch (Exception ex) {
                    return Result<IList<DestinationDAL>>.Failure ($"Error retrieving destinations");
                }
            }
        }
    }
}
