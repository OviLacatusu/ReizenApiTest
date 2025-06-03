using Microsoft.EntityFrameworkCore;
using Reizen.Data.Models;
using Reizen.Data.Models.CQRS;
using Reizen.Data.Models.CQRS.Commands;
using Reizen.Data.Models.CQRS.Queries;
using Reizen.Data.Repositories; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Reizen.Data.Models.CQRS.Commands.AddKlant;
using static Reizen.Data.Models.CQRS.Commands.DeleteKlant;
using static Reizen.Data.Models.CQRS.Commands.UpdateKlant;
using static Reizen.Data.Models.CQRS.Queries.GetKlanten;
using static Reizen.Data.Models.CQRS.Queries.GetKlantMetID;
using static Reizen.Data.Models.CQRS.Queries.GetKlantMetNaam;
using Klant = Reizen.Data.Models.Klant;

namespace Reizen.Data.Services
{
    public sealed class KlantenService (IMediator mediator, IDbContextFactory<ReizenContext> factory) : IKlantenRepository
    {
        public async Task<Result<IList<Klant>>> GetKlantenAsync ()
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteQuery<GetKlantenQuery, Result<IList<Klant>>> (new GetKlantenQuery (context));
            }
        }
        public async Task<Result<Klant>> GetKlantMetIdAsync (int id)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteQuery<GetKlantMetIDQuery, Result<Klant>> (new GetKlantMetIDQuery (context, id));
            }
        }
        public async Task<Result<IList<Klant>>> GetKlantenMetNaamAsync (string naam)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteQuery<GetKlantMetNaamQuery, Result<IList<Klant>>> (new GetKlantMetNaamQuery (context, naam));
            }
        }

        public async Task<Result<Klant>> AddKlantAsync (Klant klant)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteCommand<AddKlantCommand, Result<Klant>> (new AddKlantCommand (klant, context));
            }
        }

        public async Task<Result<Klant>> UpdateKlantAsync (int id, Klant klantDetails)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteCommand<UpdateKlantCommand, Result<Klant>> (new UpdateKlantCommand (klantDetails, id, context));
            }
        }

        public async Task<Result<Klant>> DeleteKlantAsync (int id)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteCommand<DeleteKlantCommand, Result<Klant>> (new DeleteKlantCommand (id, context));
            }
        }
    }
}
