using Microsoft.EntityFrameworkCore;
using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Queries
{
    public sealed class GetWerelddelen
    {
        public record GetWerelddelenQuery(ReizenContext context): IQuery<IList<Werelddeel>>;

        public class GetWerelddelenQueryHandler : IQueryHandler<GetWerelddelenQuery, IList<Werelddeel>>
        {
            public async Task<IList<Werelddeel>?> Handle (GetWerelddelenQuery query)
            {
                return await query.context.Werelddelen.ToListAsync();
            }
        }
    }
}
