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
    public sealed class GetLandMetId
    {
        public record GetLandMetIdQuery (int id, ReizenContext context) : IQuery<Result<LandDAL>>;

        public class GetLandMetIdQueryHandler : IQueryHandler<GetLandMetIdQuery, Result<LandDAL>>
        {
            public async Task<Result<LandDAL>> Handle (GetLandMetIdQuery query)
            {
                try
                {
                    if (query.id <= 0)
                    {
                        return Result<LandDAL>.Failure ("Invalid land ID");
                    }

                    var result = await query.context.Landen.FindAsync (query.id);

                    return result == null
                        ? Result<LandDAL>.Failure ($"Land with ID {query.id} not found")
                        : Result<LandDAL>.Success (result);
                }
                catch (Exception ex)
                {
                    return Result<LandDAL>.Failure ($"Error retrieving land: {ex.Message}");
                }
            }
        }

    }
}
