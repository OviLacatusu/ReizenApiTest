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
        public record GetKlantMetIDQuery(ReizenContext context, int id) : IQuery<Result<Klant>>;

        public class GetKlantMetIDQueryHandler : IQueryHandler<GetKlantMetIDQuery, Result<Klant>> 
        {
            public async Task<Result<Klant>> Handle (GetKlantMetIDQuery query) 
            {
                try
                {
                    if (query.id <= 0)
                    {
                        return Result<Klant>.Failure ("Invalid customer ID");
                    }

                    var klant = await query.context.Klanten.FindAsync (query.id);

                    return klant == null
                        ? Result<Klant>.Failure ($"Customer with ID {query.id} not found")
                        : Result<Klant>.Success (klant);
                }
                catch (Exception ex)
                {
                    return Result<Klant>.Failure ($"Error retrieving customer: {ex.Message}");
                }
            } 
        }
    }
}
