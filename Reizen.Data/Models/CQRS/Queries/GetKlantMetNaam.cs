using Microsoft.EntityFrameworkCore;
using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Queries
{
    public class GetKlantMetNaam
    {
        public record GetKlantMetNaamQuery(ReizenContext context, string naam) : IQuery<Klant>;

        public class GetKlantMetNaamQueryHandler : IQueryHandler<GetKlantMetNaamQuery, Klant>
        {
            public async Task<Klant?> Execute (GetKlantMetNaamQuery query)
            {
                return await query.context.Klanten.Where (k => k.Voornaam.Contains (query.naam, StringComparison.OrdinalIgnoreCase) || k.Familienaam.Contains(query.naam, StringComparison.OrdinalIgnoreCase)).FirstOrDefaultAsync();
            }
        }
    }
}
