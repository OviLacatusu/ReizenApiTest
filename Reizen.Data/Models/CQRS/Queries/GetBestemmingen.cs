using Microsoft.EntityFrameworkCore;
using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Queries
{
    public class GetBestemmingen
    {
        public record GetBestemmingenQuery (ReizenContext context) : IQuery<IList<Bestemming>>;

        public class GetBestemmingenVanLandQueryHandler : IQueryHandler<GetBestemmingenQuery, IList<Bestemming>>
        {
            public async Task<IList<Bestemming>?> Execute (GetBestemmingenQuery query)
            {
                var result = (await query.context.Bestemmingen.ToListAsync ());
                return result.Count == 0 ? null : result.ToList ();
            }
        }
    }
}
