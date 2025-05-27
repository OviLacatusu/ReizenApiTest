using Microsoft.EntityFrameworkCore;
using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Queries
{
    public sealed class GetLandMetId
    {
        public record GetLandMetIdQuery (int id, ReizenContext context) : IQuery<Result<Land>>;

        public class GetLandMetIdQueryHandler : IQueryHandler<GetLandMetIdQuery, Result<Land>>
        {
            public async Task<Result<Land>> Handle (GetLandMetIdQuery query)
            {
                try
                {
                    if (query.id <= 0)
                    {
                        return Result<Land>.Failure ("Invalid land ID");
                    }

                    var result = await query.context.Landen.FindAsync (query.id);

                    return result == null
                        ? Result<Land>.Failure ($"Land with ID {query.id} not found")
                        : Result<Land>.Success (result);
                }
                catch (Exception ex)
                {
                    return Result<Land>.Failure ($"Error retrieving land: {ex.Message}");
                }
            }
        }

    }
}
