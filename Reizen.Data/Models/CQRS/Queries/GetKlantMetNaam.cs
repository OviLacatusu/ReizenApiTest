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
    public sealed class GetKlantMetNaam
    {
        public record GetKlantMetNaamQuery(ReizenContext context, string naam) : IQuery<Result<IList<KlantDAL>>>;

        public class GetKlantMetNaamQueryHandler : IQueryHandler<GetKlantMetNaamQuery, Result<IList<KlantDAL>>>
        {
            public async Task<Result<IList<KlantDAL>>> Handle (GetKlantMetNaamQuery query)
            {
                try
                {
                    if (string.IsNullOrEmpty (query.naam))
                    {
                        return Result<IList<KlantDAL>>.Failure ("Name cannot be empty");
                    }

                    var result = (await query.context.Klanten.ToListAsync ())
                        .Where (k => k.Voornaam.Contains (query.naam, StringComparison.OrdinalIgnoreCase) ||
                                  k.Familienaam.Contains (query.naam, StringComparison.OrdinalIgnoreCase))
                        .ToList ();

                    return result.Count == 0
                        ? Result<IList<KlantDAL>>.Failure ($"No customers found with name containing '{query.naam}'")
                        : Result<IList<KlantDAL>>.Success (result);
                }
                catch (Exception ex)
                {
                    return Result<IList<KlantDAL>>.Failure ($"Error retrieving customers: {ex.Message}");
                }
            }
        }
    }
}
