using Microsoft.EntityFrameworkCore;
using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Queries
{
    public sealed class GetLandenVanWerelddeel
    {
        public record GetLandenVanWerelddeelQuery(string werelddeel, ReizenContext context):IQuery<IList<Land>>;

        public class GetLandenVanWerelddeelQueryHandler : IQueryHandler<GetLandenVanWerelddeelQuery, IList<Land>>
        {
            public async Task<IList<Land>?> Handle (GetLandenVanWerelddeelQuery query)
            {
                var werelddeel = (await query.context.Werelddelen.ToListAsync()).Where (w => w.Naam.Contains (query.werelddeel, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                var result = (await query.context.Landen.ToListAsync ()).Where (l => l.Werelddeelid == werelddeel?.Id ).ToList();
                return (IList<Land>?)result;
            }
        }
    }
}
