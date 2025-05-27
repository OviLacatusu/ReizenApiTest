using Microsoft.EntityFrameworkCore;
using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Queries
{
    public sealed class GetBoekingMetId
    {
        public record GetBoekingMetIdQuery(int id, ReizenContext context): IQuery<Result<Boeking?>>;

        public class GetBoekingMetIdQueryHandler : IQueryHandler<GetBoekingMetIdQuery, Result<Boeking?>>
        {
            public async Task<Result<Boeking?>> Handle (GetBoekingMetIdQuery query)
            {
                try
                {
                    if (query.id <= 0)
                    {
                        return Result<Boeking?>.Failure ("Invalid booking ID");
                    }

                    var boeking = await query.context.Boekingen.FirstOrDefaultAsync (el => el.Id == query.id);
                    return boeking == null
                        ? Result<Boeking?>.Failure ($"Booking with ID {query.id} not found")
                        : Result<Boeking?>.Success (boeking);
                }
                catch (Exception ex)
                {
                    return Result<Boeking?>.Failure ($"Error retrieving booking: {ex.Message}");
                }
            }
        }
    }
}
