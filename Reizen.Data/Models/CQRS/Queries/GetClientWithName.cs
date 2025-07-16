using Microsoft.EntityFrameworkCore;
using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reizen.CommonClasses;

namespace Reizen.Data.Models.CQRS.Queries
{
    public sealed class GetClientWithName
    {
        public record GetClientWithNameQuery(ReizenContext context, string name) : IQuery<Result<IList<ClientDAL>>>;

        public class GetClientWithNameQueryHandler : IQueryHandler<GetClientWithNameQuery, Result<IList<ClientDAL>>>
        {
            public async Task<Result<IList<ClientDAL>>> Handle (GetClientWithNameQuery query)
            {
                try
                {
                    if (string.IsNullOrEmpty (query.name))
                    {
                        return Result<IList<ClientDAL>>.Failure ("Name cannot be empty");
                    }

                    var result = (await query.context.Clients.ToListAsync ())
                        .Where (k => k.FirstName.Contains (query.name, StringComparison.OrdinalIgnoreCase) ||
                                  k.FamilyName.Contains (query.name, StringComparison.OrdinalIgnoreCase))
                        .ToList ();

                    return result.Count == 0
                        ? Result<IList<ClientDAL>>.Failure ($"No customers found with name containing '{query.name}'")
                        : Result<IList<ClientDAL>>.Success (result);
                }
                catch (Exception ex)
                {
                    return Result<IList<ClientDAL>>.Failure ($"Error retrieving customers: {ex.Message}");
                }
            }
        }
    }
}
