using Microsoft.EntityFrameworkCore;
using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Queries
{
    public sealed class GetKlanten 
    {
        public record GetKlantenQuery(ReizenContext context): IQuery<Result<IList<KlantDAL>>>;

        public class GetKlantenQueryHandler : IQueryHandler<GetKlantenQuery, Result<IList<KlantDAL>>>
        {
            public async Task<Result<IList<KlantDAL>>> Handle (GetKlantenQuery query)
            {
                try
                {
                    var klanten = await query.context.Klanten.ToListAsync ();
                    return Result<IList<KlantDAL>>.Success (klanten);
                }
                catch (Exception ex)
                {
                    return Result<IList<KlantDAL>>.Failure ($"Error retrieving customers: {ex.Message}");
                }
            }
        }
    }
}
