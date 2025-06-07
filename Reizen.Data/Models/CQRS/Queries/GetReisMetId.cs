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
    public sealed class GetReisMetId
    {
        public record GetReisMetIdQuery(ReizenContext context, int id): IQuery<Result<ReisDAL>>;

        public class GetReisMetIdQueryHandler : IQueryHandler<GetReisMetIdQuery, Result<ReisDAL>>
        {
            public async Task<Result<ReisDAL>> Handle (GetReisMetIdQuery query)
            {
                try
                {
                    if (query.id <= 0)
                    {
                        return Result<ReisDAL>.Failure ("Invalid trip ID");
                    }

                    var reis = (await query.context.Reizen
                        .Include (r => r.Bestemming)
                        .ToListAsync ())
                        .FirstOrDefault (r => r.Id == query.id);

                    return reis == null
                        ? Result<ReisDAL>.Failure ($"Trip with ID {query.id} not found")
                        : Result<ReisDAL>.Success (reis);
                }
                catch (Exception ex)
                {
                    return Result<ReisDAL>.Failure ($"Error retrieving trip: {ex.Message}");
                }
            }
        }
    }
}
