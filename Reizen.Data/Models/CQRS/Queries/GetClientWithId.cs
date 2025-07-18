using Reizen.Data.Repositories;
using Reizen.CommonClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Reizen.Data.Models.CQRS.Queries
{
    public sealed class GetClientWithId
    {
        public record GetClientWithIdQuery(ReizenContext context, int id) : IQuery<Result<ClientDAL>>;

        public class GetClientWithIdQueryHandler : IQueryHandler<GetClientWithIdQuery, Result<ClientDAL>> 
        {
            public async Task<Result<ClientDAL>> Handle (GetClientWithIdQuery query) 
            {
                try
                {
                    if (query.id <= 0)
                    {
                        return Result<ClientDAL>.Failure ("Invalid customer ID");
                    }

                    var klant = await query.context.Clients.Include (k => k.Residence).FirstOrDefaultAsync (c => c.Id == query.id);

                    return klant == null
                        ? Result<ClientDAL>.Failure ($"Customer with ID {query.id} not found")
                        : Result<ClientDAL>.Success (klant);
                }
                catch (Exception ex)
                {
                    return Result<ClientDAL>.Failure ($"Error retrieving customer: {ex.Message}");
                }
            } 
        }
    }
}
