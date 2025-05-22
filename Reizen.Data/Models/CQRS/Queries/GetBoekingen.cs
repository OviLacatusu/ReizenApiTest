using Microsoft.EntityFrameworkCore;
using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Queries
{
    public sealed class GetBoekingen
    {
        public record GetBoekingenQuery(ReizenContext context) : IQuery<IList<Boeking>?>;

        public class GetBoekingenQueryHandler : IQueryHandler<GetBoekingenQuery, IList<Boeking>?>
        {
            public async Task<IList<Boeking>?> Execute (GetBoekingenQuery query)
            {
                var boekingen = await query.context.Boekingen?.ToListAsync ();
                return boekingen;
            }
        }
    }
}
