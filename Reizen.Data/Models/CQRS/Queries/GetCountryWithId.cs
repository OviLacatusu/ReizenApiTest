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
    public sealed class GetCountryWithId
    {
        public record GetCountryWithIdQuery (int id, ReizenContext context) : IQuery<Result<CountryDAL>>;

        public class GetCountryWithIdQueryHandler : IQueryHandler<GetCountryWithIdQuery, Result<CountryDAL>>
        {
            public async Task<Result<CountryDAL>> Handle (GetCountryWithIdQuery query)
            {
                try
                {
                    if (query.id <= 0)
                    {
                        return Result<CountryDAL>.Failure ("Invalid Country ID");
                    }

                    var result = await query.context.Countries.FindAsync (query.id);

                    return result == null
                        ? Result<CountryDAL>.Failure ($"Country with ID {query.id} not found")
                        : Result<CountryDAL>.Success (result);
                }
                catch (Exception ex)
                {
                    return Result<CountryDAL>.Failure ($"Error retrieving Country: {ex.Message}");
                }
            }
        }

    }
}
