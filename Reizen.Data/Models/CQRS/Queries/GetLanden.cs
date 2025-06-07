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
    public sealed class GetLanden
    {
        public record GetLandenQuery (ReizenContext context) : IQuery<Result<IList<LandDAL>>>;

        public class GetLandenQueryHandler : IQueryHandler<GetLandenQuery, Result<IList<LandDAL>>>
        {
            public async Task<Result<IList<LandDAL>>> Handle (GetLandenQuery query)
            {
                try
                {
                    var result = await query.context.Landen.ToListAsync ();

                    return result.Count == 0
                        ? Result<IList<LandDAL>>.Failure ("No countries found")
                        : Result<IList<LandDAL>>.Success (result);
                }
                catch (Exception ex)
                {
                    return Result<IList<LandDAL>>.Failure ($"Error retrieving countries: {ex.Message}");
                }
            }
        }
    }
}
