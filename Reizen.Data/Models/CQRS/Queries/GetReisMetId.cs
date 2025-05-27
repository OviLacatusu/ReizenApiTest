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
        public record GetReisMetIdQuery(ReizenContext context, int id): IQuery<Reis>;

        public class GetReisMetIdQueryHandler : IQueryHandler<GetReisMetIdQuery, Reis>
        {
            public async Task<Reis?> Handle (GetReisMetIdQuery query)
            {
                var reis = (await query.context.Reizen.Include(r => r.Bestemming).ToListAsync()).Where (r => r.Id == query.id).FirstOrDefault();
                return reis;
            }
        }
    }
}
