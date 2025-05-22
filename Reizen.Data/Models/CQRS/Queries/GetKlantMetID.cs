using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Queries
{
    public sealed class GetKlantMetID
    {
        public record GetKlantMetIDQuery(ReizenContext context, int id) : IQuery<Klant>;

        public class GetKlantMetIDQueryHandler : IQueryHandler<GetKlantMetIDQuery, Klant> 
        {
            public async Task<Klant?> Execute (GetKlantMetIDQuery query) 
            {
                return await query.context.Klanten.FindAsync (query.id);
            } 
        }
    }
}
