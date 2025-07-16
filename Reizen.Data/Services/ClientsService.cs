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
using static Reizen.Data.Models.CQRS.Commands.AddClient;
using static Reizen.Data.Models.CQRS.Commands.DeleteClient;
using static Reizen.Data.Models.CQRS.Commands.UpdateClient;
using static Reizen.Data.Models.CQRS.Queries.GetClients;
using static Reizen.Data.Models.CQRS.Queries.GetClientWithId;
using static Reizen.Data.Models.CQRS.Queries.GetClientWithName;
using ClientDAL = Reizen.Data.Models.ClientDAL;

namespace Reizen.Data.Services
{
    public sealed class ClientsService (IMediator mediator, IDbContextFactory<ReizenContext> factory) : IClientsRepository
    {
        public async Task<Result<IList<ClientDAL>>> GetClientsAsync ()
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteQuery<GetClientsQuery, Result<IList<ClientDAL>>> (new GetClientsQuery (context));
            }
        }
        public async Task<Result<ClientDAL>> GetClientWithIdAsync (int id)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteQuery<GetClientWithIdQuery, Result<ClientDAL>> (new GetClientWithIdQuery (context, id));
            }
        }
        public async Task<Result<IList<ClientDAL>>> GetClientsWithNameAsync (string name)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteQuery<GetClientWithNameQuery, Result<IList<ClientDAL>>> (new GetClientWithNameQuery (context, name));
            }
        }

        public async Task<Result<ClientDAL>> AddClientAsync (ClientDAL klant)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteCommand<AddClientCommand, Result<ClientDAL>> (new AddClientCommand (klant, context));
            }
        }

        public async Task<Result<ClientDAL>> UpdateClientAsync (int id, ClientDAL klantDetails)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteCommand<UpdateClientCommand, Result<ClientDAL>> (new UpdateClientCommand (klantDetails, id, context));
            }
        }

        public async Task<Result<ClientDAL>> DeleteClientAsync (int id)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteCommand<DeleteClientCommand, Result<ClientDAL>> (new DeleteClientCommand (id, context));
            }
        }
    }
}
