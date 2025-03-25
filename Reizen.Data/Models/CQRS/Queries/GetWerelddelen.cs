using Microsoft.EntityFrameworkCore;
using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Queries
{
    public class GetWerelddelen
    {
        public record GetWerelddelenCommand(ReizenContext context): IQuery<IList<Werelddeel>>;

        public class GetWerelddelenCommandHandler : IQueryHandler<GetWerelddelenCommand, IList<Werelddeel>>
        {
            public async Task<IList<Werelddeel>?> Execute (GetWerelddelenCommand query)
            {
                return await query.context.Werelddelen.ToListAsync();
            }
        }
    }
}
