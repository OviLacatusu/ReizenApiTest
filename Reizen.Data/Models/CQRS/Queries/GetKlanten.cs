using Microsoft.EntityFrameworkCore;
using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Queries
{
    public class GetKlanten 
    {
        public record GetKlantenQuery(ReizenContext context): IQuery<IList<Klant>>;

        public class GetKlantenQueryHandler : IQueryHandler<GetKlantenQuery, IList<Klant>> 
        {
            public async Task<IList<Klant>?> Execute (GetKlantenQuery query)
            {
                return await query.context.Klanten.ToListAsync();
            }
        }
    }
}
