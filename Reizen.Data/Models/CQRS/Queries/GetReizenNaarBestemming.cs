using Microsoft.EntityFrameworkCore;
using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Queries
{
    public class GetReizenNaarBestemming
    {
        public record GetReizenNaarBestemmingQuery(ReizenContext context, string bestemmingscode): IQuery<IList<Reis>>;

        public class GetReizenNaarBestemmingQueryHandler : IQueryHandler<GetReizenNaarBestemmingQuery, IList<Reis>>
        {
            public async Task<IList<Reis>?> Execute (GetReizenNaarBestemmingQuery query)
            {
                return (await query.context.Reizen.ToListAsync ()).Where (r => r.Bestemmingscode == query.bestemmingscode).ToList();
            }
        }
    }
}
