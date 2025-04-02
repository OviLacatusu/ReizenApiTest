using Microsoft.EntityFrameworkCore;
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
    public sealed class KlantenService (IMediator mediator, IDbContextFactory<ReizenContext> factory) : IKlantenRepository
    {
        public async Task<ICollection<Klant>?> GetKlantenAsync ()
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteQuery<GetKlantenQuery, IList<Klant>> (new GetKlantenQuery (context));
            }
        }
        public async Task<Klant?> GetKlantMetIdAsync (int id)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteQuery<GetKlantMetIDQuery, Klant> (new GetKlantMetIDQuery (context, id));
            }
        }
        public async Task<ICollection<Klant>?> GetKlantenMetNaamAsync (string naam)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteQuery<GetKlantMetNaamQuery, IList<Klant>> (new GetKlantMetNaamQuery (context, naam));
            }
        }

        public Task<int> AddKlant (Klant klant)
        {
            throw new NotImplementedException ();
        }

        public Task<bool> UpdateKlant (int id, Klant klantDetails)
        {
            throw new NotImplementedException ();
        }
    }
}
