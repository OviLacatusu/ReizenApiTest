using Microsoft.EntityFrameworkCore;
using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Queries
{
    public sealed class GetBoekingen
    {
        public record GetBoekingenQuery(ReizenContext context) : IQuery<Result<IList<Boeking>>>;

        public class GetBoekingenQueryHandler : IQueryHandler<GetBoekingenQuery, Result<IList<Boeking>>>
        {
            public async Task<Result<IList<Boeking>>> Handle (GetBoekingenQuery query)
            {
                try
                {
                    var boekingen = await query.context.Boekingen.ToListAsync ();
                    return boekingen.Count == 0 ? Result<IList<Boeking>>.Failure ("No bookings found")
                                                : Result<IList<Boeking>>.Success (boekingen);
                }
                catch (Exception ex) {
                    return Result<IList<Boeking>>.Failure ($"Error retrieving bookings");
                }
            }
        }
    }
}
