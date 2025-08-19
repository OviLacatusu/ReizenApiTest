using MassTransit.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Reizen.CommonClasses;
using Reizen.Data.Contracts;
using Reizen.Data.Models.CQRS.Commands;
using Reizen.Data.Repositories;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static Reizen.Data.Models.CQRS.Commands.AddBooking;
using static Reizen.Data.Models.CQRS.Commands.AddClient;
using static Reizen.Data.Models.CQRS.Commands.AddCountryToContinent;
using static Reizen.Data.Models.CQRS.Commands.AddCQRS;
using static Reizen.Data.Models.CQRS.Commands.AddDestinationToCountry;
using static Reizen.Data.Models.CQRS.Commands.AddTripToDestination;
using static Reizen.Data.Models.CQRS.Commands.DeleteClient;
using static Reizen.Data.Models.CQRS.Commands.UpdateClient;
using static Reizen.Data.Models.CQRS.Queries.GetClients;
using static Reizen.Data.Models.CQRS.Queries.GetClientsWithName;
using static Reizen.Data.Models.CQRS.Queries.GetClientWithId;
using static Reizen.Data.Models.CQRS.Queries.GetContinents;
using static Reizen.Data.Models.CQRS.Queries.GetCountries;
using static Reizen.Data.Models.CQRS.Queries.GetCountriesOfContinent;
using static Reizen.Data.Models.CQRS.Queries.GetDestinationsOfCountry;
using static Reizen.Data.Models.CQRS.Queries.GetTripsToDestination;
using static Reizen.Data.Models.CQRS.Queries.GetTripWithId;

namespace Reizen.Data.Models.CQRS
{
    public class Mediator : IMediator
    {    
        private ILogger<IMediator> _logger;
        private IDbContextFactory<ReizenContext> _dbContextFactory;
        private ReizenContext _commandContext;
    
        private List<Object> _queryHandlers = new List<Object> ();
        //private Dictionary<ICommand, ICommandHandler<ICommand>> handlers = new Dictionary<ICommand, CQRS.ICommandHandler<ICommand>> ();
        private List<Object> _commandHandlers = new List<Object> ();
        private AddCQRSCommandHandler _addCQRSCommandHandler;
        //private IMediator _mediator = null;

        public Mediator (ILogger<Mediator> logger, IDbContextFactory<ReizenContext> contextFactory)
        {
            this._logger = logger;
            this._dbContextFactory = contextFactory;

            _commandContext = _dbContextFactory.CreateDbContext ();

            this.Register<GetDestinationsOfCountryQuery, Result<IList<DestinationDAL>>> (new GetDestinationsOfCountryQueryHandler ());
            this.Register<GetClientsQuery, Result<IList<ClientDAL>>> (new GetClientsQueryHandler ());
            this.Register<GetClientWithIdQuery, Result<ClientDAL>> (new GetClientWithIdQueryHandler ());
            this.Register<GetClientsWithNameQuery, Result<IList<ClientDAL>>> (new GetClientsWithNameQueryHandler ());
            this.Register<GetCountriesOfContinentQuery, Result<IList<CountryDAL>>> (new GetCountriesOfContinentQueryHandler ());
            this.Register<GetContinentsQuery, Result<IList<ContinentDAL>>> (new GetContinentsQueryHandler ());
            this.Register<AddDestinationToCountryCommand, Result<DestinationDAL>> (new AddDestinationToCountryCommandHandler (_commandContext));
            this.Register<AddClientCommand, Result<ClientDAL>> (new AddClientCommandHandler (_commandContext));
            this.Register<AddCountryToContinentCommand, Result<CountryDAL>> (new AddCountryToContinentCommandHandler (_commandContext));
            this.Register<AddTripToDestinationCommand, Result<TripDAL>> (new AddTripToDestinationCommandHandler (_commandContext));
            this.Register<AddBookingCommand, Result<BookingDAL>> (new AddBookingCommandHandler (_commandContext));
            this.Register<DeleteClientCommand, Result<ClientDAL>> (new DeleteClientCommandHandler (_commandContext));
            this.Register<UpdateClientCommand, Result<ClientDAL>> (new UpdateClientCommandHandler (_commandContext));
            this.Register<GetTripWithIdQuery, Result<TripDAL>> (new GetTripWithIdQueryHandler ());
            this.Register<GetCountriesQuery, Result<IList<CountryDAL>>> (new GetCountriesQueryHandler ());
            this.Register<GetTripsToDestinationQuery, Result<IList<TripDAL>>> (new GetTripsToDestinationQueryHandler ());
            // The CQRSCommandHAndler's Handle function will be called for every command, best hold a reference to it in class 
            this._addCQRSCommandHandler = new AddCQRSCommandHandler(_commandContext);

            _logger.LogInformation ($"Creating a mediator instance: {this.GetHashCode ()}");
        }

        public async Task<TResult?> ExecuteCommand<TCommand, TResult> (TCommand command) where TCommand : ICommand<TResult>
                                                                                        where TResult : class 
        {
            _logger.LogInformation ($"Executing command: {command.GetType().Name}");
            // Creating CQRSMessage to pass to a new AddCQRSCommand
            var serializedCommand = SerializeCommand<TCommand, TResult> (command);
            if (serializedCommand != null)
            {
                var message = new CQRSMessage
                {
                    CreatedAt = DateTime.Now,
                    Id = new Guid (),
                    Status = CQRSMessage.MessageStatus.Pending,
                    Value = serializedCommand,
                    Type = typeof (TCommand).Name
                };
                var result = _addCQRSCommandHandler.Handle (new AddCQRSCommand (message));
            }
            // Not sure whether a command that throws exception when serialized should be passed to a command handler
            foreach (var handler in _commandHandlers)
            {
                if (handler is ICommandHandler<TCommand, TResult>)
                {
                    return await ((ICommandHandler<TCommand, TResult>) handler).Handle (command);
                }
            }
            return null;
        }

        private string? SerializeCommand<TCommand, TResult> (TCommand command) where TCommand : ICommand<TResult>
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
            _logger.LogInformation ($"Executing command: {command.GetType ().Name}");
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
            _logger.LogInformation ($"Executing command: {query.GetType ().Name}");
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

    }
}
