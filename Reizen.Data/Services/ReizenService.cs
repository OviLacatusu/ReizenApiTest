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
using static Reizen.Data.Models.CQRS.Commands.AddReisToBestemming;
using static Reizen.Data.Models.CQRS.Commands.DeleteReis;
using static Reizen.Data.Models.CQRS.Queries.GetReisMetId;
using static Reizen.Data.Models.CQRS.Queries.GetReizenNaarBestemming;

namespace Reizen.Data.Services
{
    public class ReizenService (IMediator mediator, IDbContextFactory<ReizenContext> factory) : IReizenRepository
    {
        public async Task<Result<IList<ReisDAL>>> GetReizenMetBestemmingAsync (string bestemmingscode)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteQuery<GetReizenNaarBestemmingQuery, Result<IList<ReisDAL>>> (new GetReizenNaarBestemmingQuery (context, bestemmingscode));
            }
        }

        public async Task<Result<ReisDAL>> AddReisToBestemmingAsync (ReisDAL reis, BestemmingDAL bestemming)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteCommand<AddReisToBestemmingCommand, Result<ReisDAL>> (new AddReisToBestemmingCommand (reis, bestemming, context));
            }
        }

        public async Task<Result<ReisDAL>> GetReisMetIdAsync (int id)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteQuery<GetReisMetIdQuery, Result<ReisDAL>> (new GetReisMetIdQuery (context, id));
            }
        }

        public async Task<Result<ReisDAL>> DeleteReisMetIdAsync (int id)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteCommand<DeleteReisCommand, Result<ReisDAL>> (new DeleteReisCommand (id, context));
            }
        }
    }
}
