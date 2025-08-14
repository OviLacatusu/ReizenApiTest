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
using static Reizen.Data.Models.CQRS.Commands.AddTripToDestination;
using static Reizen.Data.Models.CQRS.Commands.DeleteTrip;
using static Reizen.Data.Models.CQRS.Queries.GetTripWithId;
using static Reizen.Data.Models.CQRS.Queries.GetTripsToDestination;

namespace Reizen.Data.Services
{
    public class TripsService (IMediator mediator, IDbContextFactory<ReizenContext> factory) : ITripsRepository
    {
        public async Task<Result<IList<TripDAL>>> GetTripsToDestinationAsync (string DestinationCode)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteQuery<GetTripsToDestinationQuery, Result<IList<TripDAL>>> (new GetTripsToDestinationQuery (context, DestinationCode));
            }
        }

        public async Task<Result<TripDAL>> AddTripToDestinationAsync (TripDAL trip, DestinationDAL Destination)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteCommand<AddTripToDestinationCommand, Result<TripDAL>> (new AddTripToDestinationCommand (trip, Destination));
            }
        }

        public async Task<Result<TripDAL>> GetTripWithIdAsync (int id)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteQuery<GetTripWithIdQuery, Result<TripDAL>> (new GetTripWithIdQuery (context, id));
            }
        }

        public async Task<Result<TripDAL>> DeleteTripWithIdAsync (int id)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteCommand<DeleteTripCommand, Result<TripDAL>> (new DeleteTripCommand (id));
            }
        }

    }
}
