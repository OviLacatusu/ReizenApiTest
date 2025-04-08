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
using static Reizen.Data.Models.CQRS.Commands.AddLandToWerelddeel;
using static Reizen.Data.Models.CQRS.Queries.GetBestemmingen;
using static Reizen.Data.Models.CQRS.Queries.GetBestemmingenVanLand;
using static Reizen.Data.Models.CQRS.Queries.GetKlanten;
using static Reizen.Data.Models.CQRS.Queries.GetKlantMetID;
using static Reizen.Data.Models.CQRS.Queries.GetKlantMetNaam;
using static Reizen.Data.Models.CQRS.Queries.GetLandenVanWerelddeel;
using static Reizen.Data.Models.CQRS.Queries.GetReizenNaarBestemming;
using static Reizen.Data.Models.CQRS.Queries.GetWerelddelen;

namespace Reizen.Domain.Services
{
    public sealed class LandenService (IMediator mediator, IDbContextFactory<ReizenContext> factory) : ILandenWerelddelenRepository
    {
        public async Task<ICollection<Werelddeel>?> GetWerelddelenAsync ()
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteQuery<GetWerelddelenQuery, IList<Werelddeel>> (new GetWerelddelenQuery (context));
            }
        }

        public async Task<ICollection<Land>?> GetLandenVanWerelddeelAsync (string? werelddeelNaam)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteQuery<GetLandenVanWerelddeelQuery, IList<Land>> (new GetLandenVanWerelddeelQuery (werelddeelNaam, context));
            }
        }

        public async Task<ICollection<Bestemming>?> GetBestemmingenVanLandAsync (string? landnaam)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteQuery<GetBestemmingenVanLandQuery, IList<Bestemming>> (new GetBestemmingenVanLandQuery (landnaam, context));
            }
        }

        public async Task<ICollection<Bestemming>?> GetBestemmingenAsync ()
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteQuery<GetBestemmingenQuery, IList<Bestemming>> (new GetBestemmingenQuery (context));
            }
        }

        public async Task<Wrapper<int>> AddLandToWerelddeelAsync (Land land, Werelddeel deel)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteCommand<AddLandToWerelddeelCommand, Wrapper<int>> (new AddLandToWerelddeelCommand(land, deel, context));
            }
        }

        public Task<Bestemming> AddBestemmingAsync (Bestemming bestemming)
        {
            using (var context = factory.CreateDbContext ())
            {

            }
        }
    }
}
