using Microsoft.EntityFrameworkCore;
using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Queries
{
    public sealed class GetReisMetId
    {
        public record GetReisMetIdQuery(ReizenContext context, int id): IQuery<Result<Reis>>;

        public class GetReisMetIdQueryHandler : IQueryHandler<GetReisMetIdQuery, Result<Reis>>
        {
            public async Task<Result<Reis>> Handle (GetReisMetIdQuery query)
            {
                try
                {
                    if (query.id <= 0)
                    {
                        return Result<Reis>.Failure ("Invalid trip ID");
                    }

                    var reis = (await query.context.Reizen
                        .Include (r => r.Bestemming)
                        .ToListAsync ())
                        .FirstOrDefault (r => r.Id == query.id);

                    return reis == null
                        ? Result<Reis>.Failure ($"Trip with ID {query.id} not found")
                        : Result<Reis>.Success (reis);
                }
                catch (Exception ex)
                {
                    return Result<Reis>.Failure ($"Error retrieving trip: {ex.Message}");
                }
            }
        }
    }
}
