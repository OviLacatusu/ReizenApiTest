using Microsoft.EntityFrameworkCore;
using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Queries
{
    public sealed class GetLandMetId
    {
        public record GetLandMetIdQuery (int id, ReizenContext context) : IQuery<Land>;

        public class GetLandMetIdQueryHandler : IQueryHandler<GetLandMetIdQuery, Land>
        {
            public async Task<Land?> Handle (GetLandMetIdQuery query)
            {
                var result = (await query.context.Landen.FindAsync (query.id));
                return result;
            }
        }

    }
}
