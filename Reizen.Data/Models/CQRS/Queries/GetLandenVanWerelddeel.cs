using Microsoft.EntityFrameworkCore;
using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Queries
{
    public class GetLandenVanWerelddeel
    {
        public record GetLandenVanWerelddeelCommand(string werelddeel, ReizenContext context):IQuery<IList<Land>>;

        public class GetLandenVanWerelddeelCommandHandler : IQueryHandler<GetLandenVanWerelddeelCommand, IList<Land>>
        {
            public async Task<IList<Land>?> Execute (GetLandenVanWerelddeelCommand query)
            {
                var werelddeel = await query.context.Werelddelen.Where (w => w.Naam.Contains (query.werelddeel, StringComparison.OrdinalIgnoreCase)).FirstOrDefaultAsync();
                return werelddeel == null ? null : werelddeel.Landen.ToList();
            }
        }
    }
}
