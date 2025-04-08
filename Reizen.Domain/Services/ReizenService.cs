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
using static Reizen.Data.Models.CQRS.Commands.AddReisToBestemming;
using static Reizen.Data.Models.CQRS.Queries.GetReisMetId;
using static Reizen.Data.Models.CQRS.Queries.GetReizenNaarBestemming;

namespace Reizen.Domain.Services
{
    public class ReizenService (IMediator mediator, IDbContextFactory<ReizenContext> factory) : IReizenRepository
    {
        public async Task<ICollection<Reis>?> GetReizenMetBestemmingAsync (string bestemmingscode)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteQuery<GetReizenNaarBestemmingQuery, IList<Reis>> (new GetReizenNaarBestemmingQuery (context, bestemmingscode));
            }
        }

        public async Task<Wrapper<int>> AddReisToBestemmingAsync (Reis reis, Bestemming bestemming)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteCommand<AddReisToBestemmingCommand, Wrapper<int>> (new AddReisToBestemmingCommand (reis, bestemming, context));
            }
        }

        public async Task<Reis>? GetReisMetIdAsync (int id)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteQuery<GetReisMetIdQuery, Reis> (new GetReisMetIdQuery (context, id));
            }
        }
    }
}
