using Reizen.Data.Models;
using Reizen.Data.Models.CQRS;
using Reizen.Data.Models.CQRS.Queries;
using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Reizen.Data.Models.CQRS.Queries.GetKlanten;
using static Reizen.Data.Models.CQRS.Queries.GetKlantMetID;
using static Reizen.Data.Models.CQRS.Queries.GetKlantMetNaam;

namespace Reizen.Domain.Services
{
    public sealed class KlantenService (IMediator mediator, ReizenContext context) : IKlantenRepository
    {
        public async Task<ICollection<Klant>?> GetKlantenAsync ()
        {
            return await mediator.ExecuteQuery<GetKlantenQuery, IList<Klant>>(new GetKlantenQuery(context));
        }
        public async Task<Klant?> GetKlantMetIdAsync (int id)
        {
            return await mediator.ExecuteQuery<GetKlantMetIDQuery, Klant> (new GetKlantMetIDQuery (context, id));
        }
        public async Task<ICollection<Klant>?> GetKlantenMetNaamAsync (string naam)
        {
            return await mediator.ExecuteQuery<GetKlantMetNaamQuery, IList<Klant>> (new GetKlantMetNaamQuery(context, naam));
        }
    }
}
