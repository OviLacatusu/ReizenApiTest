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
    public sealed class GetCountries
    {
        public record GetCountriesQuery (ReizenContext context) : IQuery<Result<IList<CountryDAL>>>;

        public class GetCountriesQueryHandler : IQueryHandler<GetCountriesQuery, Result<IList<CountryDAL>>>
        {
            public async Task<Result<IList<CountryDAL>>> Handle (GetCountriesQuery query)
            {
                try
                {
                    var result = await query.context.Countries.ToListAsync ();

                    return result.Count == 0
                        ? Result<IList<CountryDAL>>.Failure ("No countries found")
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
