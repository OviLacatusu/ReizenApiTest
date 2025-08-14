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
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Reizen.Data.Repositories;
using System.Text.Json;
using Reizen.Data.Contracts;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Reizen.Data.Models.CQRS
{
    public class Mediator : IMediator
    {    
        private ILogger<IMediator> _logger;
        private IDbContextFactory<ReizenContext> _dbContextFactory;
    
        private List<Object> _queryHandlers = new List<Object> ();
        private Dictionary<ICommand, ICommandHandler<ICommand>> handlers = new Dictionary<ICommand, CQRS.ICommandHandler<ICommand>> ();
        private List<Object> _commandHandlers = new List<Object> ();

        private static IMediator _mediator = null;

        private Mediator (ILogger<Mediator> logger, IDbContextFactory<ReizenContext> context)
        {
            this._logger = logger;
            this._dbContextFactory = context;
        }
        public async Task<TResult?> ExecuteCommand<TCommand, TResult> (TCommand command) where TCommand : ICommand<TResult>
                                                                                         where TResult : class 
        {
            using var context = _dbContextFactory.CreateDbContext ();
            var message = new CQRSMessage
            {
                CreatedAt = DateTime.Now,
                Id = new Guid (),
                Status = CQRSMessage.MessageStatus.Pending,
                Value = SerializeCommand<TCommand, TResult> (command),
                Type = typeof (TCommand).ToString ()
            };
            context.OutboxCQRSMessages.Add (message);
            var result = await context.SaveChangesAsync ();

            foreach (var handler in _commandHandlers)
            {
                if (handler is ICommandHandler<TCommand, TResult>)
                {
                    return await ((ICommandHandler<TCommand, TResult>) handler).Handle (command);
                }
            }
            return null;
        }
        public string? SerializeCommand<TCommand, TResult> (TCommand command) where TCommand : ICommand<TResult>
                                                                              where TResult : class
        {
            try
            {
                var serializedCommand = JsonConvert.SerializeObject (command, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                return serializedCommand;
            }
            catch (Exception ex) 
            {
                _logger.LogError (ex.Message);
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
        public static IMediator? MediatorFactory (ILogger<Mediator> logger, IDbContextFactory<ReizenContext> contextFactory)
        {
            if (_mediator is null)
            {
                _mediator = new Mediator (logger, contextFactory);

                _mediator.Register<GetDestinationsOfCountryQuery, Result<IList<DestinationDAL>>> (new GetDestinationsOfCountryQueryHandler ());
                _mediator.Register<GetClientsQuery, Result<IList<ClientDAL>>> (new GetClientsQueryHandler ());
                _mediator.Register<GetClientWithIdQuery, Result<ClientDAL>> (new GetClientWithIdQueryHandler ());
                _mediator.Register<GetClientsWithNameQuery, Result<IList<ClientDAL>>> (new GetClientsWithNameQueryHandler ());
                _mediator.Register<GetCountriesOfContinentQuery, Result<IList<CountryDAL>>> (new GetCountriesOfContinentQueryHandler ());
                _mediator.Register<GetContinentsQuery, Result<IList<ContinentDAL>>> (new GetContinentsQueryHandler ());
                _mediator.Register<AddDestinationToCountryCommand, Result<DestinationDAL>> (new AddDestinationToCountryCommandHandler (contextFactory.CreateDbContext ()));
                _mediator.Register<AddClientCommand, Result<ClientDAL>> (new AddClientCommandHandler (contextFactory.CreateDbContext ()));
                _mediator.Register<AddCountryToContinentCommand, Result<CountryDAL>> (new AddCountryToContinentCommandHandler (contextFactory.CreateDbContext ()));
                _mediator.Register<AddTripToDestinationCommand, Result<TripDAL>> (new AddTripToDestinationCommandHandler (contextFactory.CreateDbContext ()));
                _mediator.Register<AddBookingCommand, Result<BookingDAL>> (new AddBookingCommandHandler (contextFactory.CreateDbContext()));
                _mediator.Register<DeleteClientCommand, Result<ClientDAL>> (new DeleteClientCommandHandler (contextFactory.CreateDbContext ()));
                _mediator.Register<UpdateClientCommand, Result<ClientDAL>> (new UpdateClientCommandHandler (contextFactory.CreateDbContext ()));
                _mediator.Register<GetTripWithIdQuery, Result<TripDAL>> (new GetTripWithIdQueryHandler ());
                _mediator.Register<GetCountriesQuery, Result<IList<CountryDAL>>> (new GetCountriesQueryHandler ());
                _mediator.Register<GetTripsToDestinationQuery, Result<IList<TripDAL>>> (new GetTripsToDestinationQueryHandler ());
            }

            return _mediator;
        }
    }
}
