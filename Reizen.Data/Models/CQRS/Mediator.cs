using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Diagnostics;
using Reizen.Data.Models.CQRS.Queries;
using static Reizen.Data.Models.CQRS.Queries.GetDestinationsOfCountry;
using static Reizen.Data.Models.CQRS.Queries.GetClients;
using static Reizen.Data.Models.CQRS.Queries.GetClientWithId;
using static Reizen.Data.Models.CQRS.Queries.GetClientsWithName;
using static Reizen.Data.Models.CQRS.Queries.GetCountriesOfContinent;
using static Reizen.Data.Models.CQRS.Queries.GetContinents;
using Reizen.Data.Models.CQRS.Commands;
using static Reizen.Data.Models.CQRS.Commands.AddDestinationToCountry;
using static Reizen.Data.Models.CQRS.Commands.AddClient;
using static Reizen.Data.Models.CQRS.Commands.AddCountryToContinent;
using static Reizen.Data.Models.CQRS.Commands.AddTripToDestination;
using static Reizen.Data.Models.CQRS.Commands.DeleteClient;
using static Reizen.Data.Models.CQRS.Commands.UpdateClient;
using static Reizen.Data.Models.CQRS.Queries.GetTripsToDestination;
using static Reizen.Data.Models.CQRS.Queries.GetTripWithId;
using static Reizen.Data.Models.CQRS.Commands.AddBooking;
using static Reizen.Data.Models.CQRS.Queries.GetCountries;
using Reizen.CommonClasses;

namespace Reizen.Data.Models.CQRS
{
    public class Mediator : IMediator
    {
        private List<Object> _queryHandlers = new List<Object> ();
        private List<Object> _commandHandlers = new List<Object> ();

        //private IMediator _mediator = null;
        public async Task<TResult?> ExecuteCommand<TCommand, TResult> (TCommand command) where TCommand : ICommand<TResult>
                                                                                          where TResult : class 
        {
            foreach (var handler in _commandHandlers)
            {
                if (handler is ICommandHandler<TCommand, TResult>)
                {
                    return await ((ICommandHandler<TCommand, TResult>) handler).Handle (command);
                }
            }
            return null;
        }

        public async Task ExecuteCommand<TCommand> (TCommand command) where TCommand : ICommand
        {
            foreach (var handler in _commandHandlers)
            {
                if (handler is ICommandHandler<TCommand>)
                {
                    (handler as ICommandHandler<TCommand>)?.Handle (command);
                    break;
                }
            }
        }

        public async Task<TResult?> ExecuteQuery<TQuery, TResult> (TQuery query) where TQuery : IQuery<TResult>
                                                                            where TResult : class
        {
            foreach (var handler in _queryHandlers)
            {
                if (handler is IQueryHandler<TQuery, TResult>)
                {
                    return await ((IQueryHandler<TQuery, TResult>)handler).Handle (query);
                }
            }
            return null;
        }

        public void Register<TCommand, TResult> (ICommandHandler<TCommand, TResult> handler) where TCommand : ICommand<TResult>
        {
            _commandHandlers.Add (handler);
        }

        public void Register<TQuery, TResult> (IQueryHandler<TQuery, TResult> handler) where TQuery : IQuery<TResult>
        {
            _queryHandlers.Add (handler);
        }

        // Not all query and command handlers are plugged in
        public static IMediator? MediatorFactory ()
        {
            var mediator = new Mediator ();

            mediator.Register<GetDestinationsOfCountryQuery, Result<IList<DestinationDAL>>> (new GetDestinationsOfCountryQueryHandler());
            mediator.Register<GetClientsQuery, Result<IList<ClientDAL>>> (new GetClientsQueryHandler ());
            mediator.Register<GetClientWithIdQuery, Result<ClientDAL>> (new GetClientWithIdQueryHandler());
            mediator.Register<GetClientsWithNameQuery, Result<IList<ClientDAL>>> (new GetClientsWithNameQueryHandler ());
            mediator.Register<GetCountriesOfContinentQuery, Result<IList<CountryDAL>>> (new GetCountriesOfContinentQueryHandler());
            mediator.Register<GetContinentsQuery, Result<IList<ContinentDAL>>> (new GetContinentsQueryHandler());
            mediator.Register<AddDestinationToCountryCommand, Result<DestinationDAL>> (new AddDestinationToCountryCommandHandler ());
            mediator.Register<AddClientCommand, Result<ClientDAL>> (new AddClientCommandHandler ());
            mediator.Register<AddCountryToContinentCommand, Result<CountryDAL>> (new AddCountryToContinentCommandHandler ());
            mediator.Register<AddTripToDestinationCommand, Result<TripDAL>> (new AddTripToDestinationCommandHandler ());
            mediator.Register<AddBookingCommand, Result<BookingDAL>> (new AddBookingCommandHandler ());
            mediator.Register<DeleteClientCommand, Result<ClientDAL>> (new DeleteClientCommandHandler ());
            mediator.Register<UpdateClientCommand, Result<ClientDAL>> (new UpdateClientCommandHandler ());
            mediator.Register<GetTripWithIdQuery, Result<TripDAL>> (new GetTripWithIdQueryHandler ());
            mediator.Register<GetCountriesQuery, Result<IList<CountryDAL>>> (new GetCountriesQueryHandler ());
            mediator.Register<GetTripsToDestinationQuery, Result<IList<TripDAL>>> (new GetTripsToDestinationQueryHandler());

            return mediator;
        }
    }
}
