using Microsoft.EntityFrameworkCore;
using Reizen.Data.Repositories;
using Reizen.CommonClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Queries
{
    public sealed class GetClients 
    {
        public record GetClientsQuery(ReizenContext context): IQuery<Result<IList<ClientDAL>>>;

        public class GetClientsQueryHandler : IQueryHandler<GetClientsQuery, Result<IList<ClientDAL>>>
        {
            public async Task<Result<IList<ClientDAL>>> Handle (GetClientsQuery query)
            {
                try
                {
                    var klanten = await query.context.Clients.ToListAsync ();
                    return Result<IList<ClientDAL>>.Success (klanten);
                }
                catch (Exception ex)
                {
                    return Result<IList<ClientDAL>>.Failure ($"Error retrieving customers: {ex.Message}");
                }
            }
        }
    }
}
