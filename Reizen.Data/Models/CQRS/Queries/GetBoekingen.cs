using Microsoft.EntityFrameworkCore;
using Reizen.CommonClasses;
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
        public record GetBoekingenQuery(ReizenContext context) : IQuery<Result<IList<BoekingDAL>>>;

        public class GetBoekingenQueryHandler : IQueryHandler<GetBoekingenQuery, Result<IList<BoekingDAL>>>
        {
            public async Task<Result<IList<BoekingDAL>>> Handle (GetBoekingenQuery query)
            {
                try
                {
                    var boekingen = await query.context.Boekingen.ToListAsync ();
                    return boekingen.Count == 0 ? Result<IList<BoekingDAL>>.Failure ("No bookings found")
                                                : Result<IList<BoekingDAL>>.Success (boekingen);
                }
                catch (Exception ex) {
                    return Result<IList<BoekingDAL>>.Failure ($"Error retrieving bookings");
                }
            }
        }
    }
}
