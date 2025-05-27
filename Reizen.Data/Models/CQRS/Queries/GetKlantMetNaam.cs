using Microsoft.EntityFrameworkCore;
using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Queries
{
    public sealed class GetKlantMetNaam
    {
        public record GetKlantMetNaamQuery(ReizenContext context, string naam) : IQuery<Result<IList<Klant>>>;

        public class GetKlantMetNaamQueryHandler : IQueryHandler<GetKlantMetNaamQuery, Result<IList<Klant>>>
        {
            public async Task<Result<IList<Klant>>> Handle (GetKlantMetNaamQuery query)
            {
                try
                {
                    if (string.IsNullOrEmpty (query.naam))
                    {
                        return Result<IList<Klant>>.Failure ("Name cannot be empty");
                    }

                    var result = (await query.context.Klanten.ToListAsync ())
                        .Where (k => k.Voornaam.Contains (query.naam, StringComparison.OrdinalIgnoreCase) ||
                                  k.Familienaam.Contains (query.naam, StringComparison.OrdinalIgnoreCase))
                        .ToList ();

                    return result.Count == 0
                        ? Result<IList<Klant>>.Failure ($"No customers found with name containing '{query.naam}'")
                        : Result<IList<Klant>>.Success (result);
                }
                catch (Exception ex)
                {
                    return Result<IList<Klant>>.Failure ($"Error retrieving customers: {ex.Message}");
                }
            }
        }
    }
}
