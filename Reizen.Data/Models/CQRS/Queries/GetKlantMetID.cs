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
        public record GetKlantMetIDQuery(ReizenContext context, int id) : IQuery<Result<KlantDAL>>;

        public class GetKlantMetIDQueryHandler : IQueryHandler<GetKlantMetIDQuery, Result<KlantDAL>> 
        {
            public async Task<Result<KlantDAL>> Handle (GetKlantMetIDQuery query) 
            {
                try
                {
                    if (query.id <= 0)
                    {
                        return Result<KlantDAL>.Failure ("Invalid customer ID");
                    }

                    var klant = await query.context.Klanten.FindAsync (query.id);

                    return klant == null
                        ? Result<KlantDAL>.Failure ($"Customer with ID {query.id} not found")
                        : Result<KlantDAL>.Success (klant);
                }
                catch (Exception ex)
                {
                    return Result<KlantDAL>.Failure ($"Error retrieving customer: {ex.Message}");
                }
            } 
        }
    }
}
