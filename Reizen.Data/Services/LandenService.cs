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
using static Reizen.Data.Models.CQRS.Commands.AddBestemming;
using static Reizen.Data.Models.CQRS.Commands.AddLand;
using static Reizen.Data.Models.CQRS.Commands.AddLandToWerelddeel;
using static Reizen.Data.Models.CQRS.Commands.DeleteBestemming;
using static Reizen.Data.Models.CQRS.Commands.DeleteLand;
using static Reizen.Data.Models.CQRS.Commands.UpdateLand;
using static Reizen.Data.Models.CQRS.Queries.GetBestemmingen;
using static Reizen.Data.Models.CQRS.Queries.GetBestemmingenVanLand;
using static Reizen.Data.Models.CQRS.Queries.GetKlanten;
using static Reizen.Data.Models.CQRS.Queries.GetKlantMetID;
using static Reizen.Data.Models.CQRS.Queries.GetKlantMetNaam;
using static Reizen.Data.Models.CQRS.Queries.GetLanden;
using static Reizen.Data.Models.CQRS.Queries.GetLandenVanWerelddeel;
using static Reizen.Data.Models.CQRS.Queries.GetLandMetId;
using static Reizen.Data.Models.CQRS.Queries.GetReizenNaarBestemming;
using static Reizen.Data.Models.CQRS.Queries.GetWerelddelen;

namespace Reizen.Data.Services
{
    public sealed class LandenService (IMediator mediator, IDbContextFactory<ReizenContext> factory) : ILandenWerelddelenRepository
    {
        public async Task<Result<IList<Werelddeel>>> GetWerelddelenAsync ()
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteQuery<GetWerelddelenQuery, Result<IList<Werelddeel>>> (new GetWerelddelenQuery (context));
            }
        }

        public async Task<Result<IList<Land>>> GetLandenVanWerelddeelAsync (string? werelddeelNaam)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteQuery<GetLandenVanWerelddeelQuery, Result<IList<Land>>> (new GetLandenVanWerelddeelQuery (werelddeelNaam, context));
            }
        }

        public async Task<Result<IList<Bestemming>>> GetBestemmingenVanLandAsync (string? landnaam)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteQuery<GetBestemmingenVanLandQuery, Result<IList<Bestemming>>> (new GetBestemmingenVanLandQuery (landnaam, context));
            }
        }

        public async Task<Result<IList<Bestemming>>> GetBestemmingenAsync ()
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteQuery<GetBestemmingenQuery, Result<IList<Bestemming>>> (new GetBestemmingenQuery (context));
            }
        }

        public async Task<Result<Land>> AddLandToWerelddeelAsync (Land land, Werelddeel deel)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteCommand<AddLandToWerelddeelCommand, Result<Land>> (new AddLandToWerelddeelCommand(land, deel, context));
            }
        }

        public async Task<Result<Bestemming>> AddBestemmingAsync (Bestemming bestemming)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteCommand<AddBestemmingCommand, Result<Bestemming>> (new AddBestemmingCommand(context, bestemming));
            }
        }

        public async Task<Result<Land>> GetLandMetIdAsync (int id)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteQuery<GetLandMetIdQuery, Result<Land>> (new GetLandMetIdQuery ( id, context));
            }
        }

        public async Task<Result<Land>> AddLandAsync (Land land)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteCommand<AddLandCommand, Result<Land>> (new AddLandCommand (land, context));
            }
        }

        public async Task<Result<Land>> UpdateLandMetIdAsync (int id, Land land)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteCommand<UpdateLandCommand, Result<Land>> (new UpdateLandCommand (id, land, context));
            }
        }
        public async Task<Result<Land>> DeleteLandMetIdAsync (int id)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteCommand <DeleteLandCommand, Result<Land>> (new DeleteLandCommand (id, context));
            }
        }

        public async Task<Result<Bestemming>> DeleteBestemmingMetIdAsync (string code)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteCommand<DeleteBestemmingCommand, Result<Bestemming>> (new DeleteBestemmingCommand (code, context));
            }
        }

        public async Task<Result<IList<Land>>> GetLandenAsync ()
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteQuery<GetLandenQuery, Result<IList<Land>>> (new GetLandenQuery (context));
            }
        }
    }
}
