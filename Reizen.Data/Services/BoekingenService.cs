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
using Reizen.CommonClasses;

namespace Reizen.Data.Services
{
    public class BoekingenService (IMediator mediator, IDbContextFactory<ReizenContext> factory) : IBoekingenRepository
    {
        public async Task<Result<BoekingDAL>> AddBoekingAsync (BoekingDAL? boeking)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteCommand<AddBoekingCommand, Result<BoekingDAL>> (new AddBoekingCommand (boeking, context));
            }
        }

        public async Task<Result<BoekingDAL>> DeleteBoekingAsync (int id)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteCommand<DeleteBoekingCommand, Result<BoekingDAL>> (new DeleteBoekingCommand (id, context));
            }
        }

        public async Task<Result<IList<BoekingDAL>>> GetBoekingenAsync ()
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteQuery<GetBoekingenQuery, Result<IList<BoekingDAL>>> (new GetBoekingenQuery (context));
            }
        }

        public async Task<Result<BoekingDAL>> GetBoekingMetIdAsync (int id)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteQuery<GetBoekingMetIdQuery, Result<BoekingDAL>> (new GetBoekingMetIdQuery (id, context)); 
            }
        }

        public async Task<Result<BoekingDAL>> UpdateBoekingAsync (BoekingDAL? boeking, int id)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteCommand<UpdateBoekingCommand, Result<BoekingDAL>> (new UpdateBoekingCommand (boeking, id, context));
            }
        }
    }
}
