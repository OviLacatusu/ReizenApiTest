using Microsoft.EntityFrameworkCore;
using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Queries
{
    public class GetBestemmingenVanLand
    {
        public record GetBestemmingenVanLandQuery(string land, ReizenContext context): IQuery<IList<Bestemming>>;

        public class GetBestemmingenVanLandQueryHandler : IQueryHandler<GetBestemmingenVanLandQuery, IList<Bestemming>>
        {
            public async Task<IList<Bestemming>?> Execute (GetBestemmingenVanLandQuery query)
            {
                var land = await query.context.Landen.Where (l => l.Naam.Contains(query.land, StringComparison.OrdinalIgnoreCase)).FirstOrDefaultAsync();
                return land == null ? null : land.Bestemmingen.ToList ();
            }
        }
    }
}
