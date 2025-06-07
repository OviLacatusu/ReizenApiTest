using Microsoft.EntityFrameworkCore;
using Reizen.CommonClasses;
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
using KlantDAL = Reizen.Data.Models.KlantDAL;

namespace Reizen.Data.Services
{
    public sealed class KlantenService (IMediator mediator, IDbContextFactory<ReizenContext> factory) : IKlantenRepository
    {
        public async Task<Result<IList<KlantDAL>>> GetKlantenAsync ()
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteQuery<GetKlantenQuery, Result<IList<KlantDAL>>> (new GetKlantenQuery (context));
            }
        }
        public async Task<Result<KlantDAL>> GetKlantMetIdAsync (int id)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteQuery<GetKlantMetIDQuery, Result<KlantDAL>> (new GetKlantMetIDQuery (context, id));
            }
        }
        public async Task<Result<IList<KlantDAL>>> GetKlantenMetNaamAsync (string naam)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteQuery<GetKlantMetNaamQuery, Result<IList<KlantDAL>>> (new GetKlantMetNaamQuery (context, naam));
            }
        }

        public async Task<Result<KlantDAL>> AddKlantAsync (KlantDAL klant)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteCommand<AddKlantCommand, Result<KlantDAL>> (new AddKlantCommand (klant, context));
            }
        }

        public async Task<Result<KlantDAL>> UpdateKlantAsync (int id, KlantDAL klantDetails)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteCommand<UpdateKlantCommand, Result<KlantDAL>> (new UpdateKlantCommand (klantDetails, id, context));
            }
        }

        public async Task<Result<KlantDAL>> DeleteKlantAsync (int id)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteCommand<DeleteKlantCommand, Result<KlantDAL>> (new DeleteKlantCommand (id, context));
            }
        }
    }
}
