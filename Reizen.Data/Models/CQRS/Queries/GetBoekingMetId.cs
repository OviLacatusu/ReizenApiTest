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
    public sealed class GetBoekingMetId
    {
        public record GetBoekingMetIdQuery(int id, ReizenContext context): IQuery<Result<BoekingDAL?>>;

        public class GetBoekingMetIdQueryHandler : IQueryHandler<GetBoekingMetIdQuery, Result<BoekingDAL?>>
        {
            public async Task<Result<BoekingDAL?>> Handle (GetBoekingMetIdQuery query)
            {
                try
                {
                    if (query.id <= 0)
                    {
                        return Result<BoekingDAL?>.Failure ("Invalid booking ID");
                    }

                    var boeking = await query.context.Boekingen.FirstOrDefaultAsync (el => el.Id == query.id);
                    return boeking == null
                        ? Result<BoekingDAL?>.Failure ($"Booking with ID {query.id} not found")
                        : Result<BoekingDAL?>.Success (boeking);
                }
                catch (Exception ex)
                {
                    return Result<BoekingDAL?>.Failure ($"Error retrieving booking: {ex.Message}");
                }
            }
        }
    }
}
