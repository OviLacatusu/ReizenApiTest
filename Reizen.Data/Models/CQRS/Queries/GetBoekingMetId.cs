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
        public record GetBoekingMetIdQuery(int id, ReizenContext context): IQuery<Boeking?>;

        public class GetBoekingMetIdQueryHandler : IQueryHandler<GetBoekingMetIdQuery, Boeking?>
        {
            public async Task<Boeking?> Handle (GetBoekingMetIdQuery query)
            {
                var boeking = await query.context.Boekingen.FirstOrDefaultAsync (el => el.Id == query.id);
                return boeking;
            }
        }
    }
}
