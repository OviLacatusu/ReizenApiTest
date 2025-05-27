using Microsoft.EntityFrameworkCore;
using Reizen.Data.Models;
using Reizen.Data.Models.CQRS;
using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Reizen.Data.Models.CQRS.Commands.AddBoeking;
using static Reizen.Data.Models.CQRS.Commands.DeleteBoeking;
using static Reizen.Data.Models.CQRS.Commands.UpdateBoeking;
using static Reizen.Data.Models.CQRS.Queries.GetBoekingen;
using static Reizen.Data.Models.CQRS.Queries.GetBoekingMetId;

namespace Reizen.Domain.Services
{
    public class BoekingenService (IMediator mediator, IDbContextFactory<ReizenContext> factory) : IBoekingenRepository
    {
        public async Task<Result<Boeking>> AddBoekingAsync (Boeking? boeking)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteCommand<AddBoekingCommand, Result<Boeking>> (new AddBoekingCommand (boeking, context));
            }
        }

        public async Task<Result<Boeking>> DeleteBoekingAsync (int id)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteCommand<DeleteBoekingCommand, Result<Boeking>> (new DeleteBoekingCommand (id, context));
            }
        }

        public async Task<IList<Boeking>?> GetBoekingenAsync ()
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteQuery<GetBoekingenQuery, IList<Boeking>?> (new GetBoekingenQuery (context));
            }
        }

        public async Task<Boeking?> GetBoekingMetIdAsync (int id)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteQuery<GetBoekingMetIdQuery, Boeking?> (new GetBoekingMetIdQuery (id, context)); 
            }
        }

        public async Task<Result<Boeking>> UpdateBoekingAsync (Boeking? boeking, int id)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteCommand<UpdateBoekingCommand, Result<Boeking>> (new UpdateBoekingCommand (boeking, id, context));
            }
        }
    }
}
