using Microsoft.EntityFrameworkCore;
using Reizen.Data.Models;
using Reizen.Data.Models.CQRS;
using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
