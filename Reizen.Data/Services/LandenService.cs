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
        public async Task<Result<IList<WerelddeelDAL>>> GetWerelddelenAsync ()
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteQuery<GetWerelddelenQuery, Result<IList<WerelddeelDAL>>> (new GetWerelddelenQuery (context));
            }
        }

        public async Task<Result<IList<LandDAL>>> GetLandenVanWerelddeelAsync (string? werelddeelNaam)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteQuery<GetLandenVanWerelddeelQuery, Result<IList<LandDAL>>> (new GetLandenVanWerelddeelQuery (werelddeelNaam, context));
            }
        }

        public async Task<Result<IList<BestemmingDAL>>> GetBestemmingenVanLandAsync (string? landnaam)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteQuery<GetBestemmingenVanLandQuery, Result<IList<BestemmingDAL>>> (new GetBestemmingenVanLandQuery (landnaam, context));
            }
        }

        public async Task<Result<IList<BestemmingDAL>>> GetBestemmingenAsync ()
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteQuery<GetBestemmingenQuery, Result<IList<BestemmingDAL>>> (new GetBestemmingenQuery (context));
            }
        }

        public async Task<Result<LandDAL>> AddLandToWerelddeelAsync (LandDAL land, WerelddeelDAL deel)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteCommand<AddLandToWerelddeelCommand, Result<LandDAL>> (new AddLandToWerelddeelCommand(land, deel, context));
            }
        }

        public async Task<Result<BestemmingDAL>> AddBestemmingAsync (BestemmingDAL bestemming)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteCommand<AddBestemmingCommand, Result<BestemmingDAL>> (new AddBestemmingCommand(context, bestemming));
            }
        }

        public async Task<Result<LandDAL>> GetLandMetIdAsync (int id)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteQuery<GetLandMetIdQuery, Result<LandDAL>> (new GetLandMetIdQuery ( id, context));
            }
        }

        public async Task<Result<LandDAL>> AddLandAsync (LandDAL land)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteCommand<AddLandCommand, Result<LandDAL>> (new AddLandCommand (land, context));
            }
        }

        public async Task<Result<LandDAL>> UpdateLandMetIdAsync (int id, LandDAL land)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteCommand<UpdateLandCommand, Result<LandDAL>> (new UpdateLandCommand (id, land, context));
            }
        }
        public async Task<Result<LandDAL>> DeleteLandMetIdAsync (int id)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteCommand <DeleteLandCommand, Result<LandDAL>> (new DeleteLandCommand (id, context));
            }
        }

        public async Task<Result<BestemmingDAL>> DeleteBestemmingMetIdAsync (string code)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteCommand<DeleteBestemmingCommand, Result<BestemmingDAL>> (new DeleteBestemmingCommand (code, context));
            }
        }

        public async Task<Result<IList<LandDAL>>> GetLandenAsync ()
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteQuery<GetLandenQuery, Result<IList<LandDAL>>> (new GetLandenQuery (context));
            }
        }
    }
}
