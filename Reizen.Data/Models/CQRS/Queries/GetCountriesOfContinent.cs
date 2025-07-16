using Microsoft.EntityFrameworkCore;
using Reizen.Data.Repositories;
using Reizen.CommonClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Queries
{
    public sealed class GetCountriesOfContinent
    {
        public record GetCountriesOfContinentQuery(string Continent, ReizenContext context):IQuery<Result<IList<CountryDAL>>>;

        public class GetCountriesOfContinentQueryHandler : IQueryHandler<GetCountriesOfContinentQuery, Result<IList<CountryDAL>>>
        {
            public async Task<Result<IList<CountryDAL>>> Handle (GetCountriesOfContinentQuery query)
            {
                try
                {
                    if (string.IsNullOrEmpty (query.Continent))
                    {
                        return Result<IList<CountryDAL>>.Failure ("Continent name cannot be empty");
                    }

                    var Continent = (await query.context.Continents.ToListAsync ())
                        .Where (w => w.Name.Contains (query.Continent, StringComparison.OrdinalIgnoreCase))
                        .FirstOrDefault ();

                    if (Continent == null)
                    {
                        return Result<IList<CountryDAL>>.Failure ($"Continent with name '{query.Continent}' not found");
                    }

                    var result = (await query.context.Countries.ToListAsync ())
                        .Where (l => l.Continentid == Continent.Id)
                        .ToList ();

                    return result.Count == 0
                        ? Result<IList<CountryDAL>>.Failure ("No countries found for this Continent")
                        : Result<IList<CountryDAL>>.Success (result);
                }
                catch (Exception ex)
                {
                    return Result<IList<CountryDAL>>.Failure ($"Error retrieving countries: {ex.Message}");
                }
            }
        }
    }
}
