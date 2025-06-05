using Microsoft.EntityFrameworkCore;
using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Queries
{
    public sealed class GetLanden
    {
        public record GetLandenQuery (ReizenContext context) : IQuery<Result<IList<Land>>>;

        public class GetLandenQueryHandler : IQueryHandler<GetLandenQuery, Result<IList<Land>>>
        {
            public async Task<Result<IList<Land>>> Handle (GetLandenQuery query)
            {
                try
                {
                    var result = await query.context.Landen.ToListAsync ();

                    return result.Count == 0
                        ? Result<IList<Land>>.Failure ("No countries found")
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
