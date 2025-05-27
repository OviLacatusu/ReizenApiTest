using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Diagnostics;
using Reizen.Data.Models.CQRS.Queries;
using static Reizen.Data.Models.CQRS.Queries.GetBestemmingenVanLand;
using static Reizen.Data.Models.CQRS.Queries.GetKlanten;
using static Reizen.Data.Models.CQRS.Queries.GetKlantMetID;
using static Reizen.Data.Models.CQRS.Queries.GetKlantMetNaam;
using static Reizen.Data.Models.CQRS.Queries.GetLandenVanWerelddeel;
using static Reizen.Data.Models.CQRS.Queries.GetWerelddelen;
using Reizen.Data.Models.CQRS.Commands;
using static Reizen.Data.Models.CQRS.Commands.AddBestemmingToLand;
using static Reizen.Data.Models.CQRS.Commands.AddKlant;
using static Reizen.Data.Models.CQRS.Commands.AddLandToWerelddeel;
using static Reizen.Data.Models.CQRS.Commands.AddReisToBestemming;
using static Reizen.Data.Models.CQRS.Commands.DeleteKlant;
using static Reizen.Data.Models.CQRS.Commands.UpdateKlant;
using static Reizen.Data.Models.CQRS.Queries.GetReizenNaarBestemming;
using static Reizen.Data.Models.CQRS.Queries.GetReisMetId;

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

        // Not all queries and commands are plugged in
        public static IMediator? MediatorFactory ()
        {
            var mediator = new Mediator ();

            mediator.Register<GetBestemmingenVanLandQuery, IList<Bestemming>> (new GetBestemmingenVanLandQueryHandler());
            mediator.Register<GetKlantenQuery, IList<Klant>> (new GetKlantenQueryHandler ());
            mediator.Register<GetKlantMetIDQuery, Klant> (new GetKlantMetIDQueryHandler());
            mediator.Register<GetKlantMetNaamQuery, IList<Klant>> (new GetKlantMetNaamQueryHandler ());
            mediator.Register<GetLandenVanWerelddeelQuery, IList<Land>> (new GetLandenVanWerelddeelQueryHandler());
            mediator.Register<GetWerelddelenQuery, IList<Werelddeel>> (new GetWerelddelenQueryHandler());
            mediator.Register<AddBestemmingToLandCommand, Result<Bestemming>> (new AddBestemmingToLandCommandHandler ());
            mediator.Register<AddKlantCommand, Result<Klant>> (new AddKlantCommandHandler ());
            mediator.Register<AddLandToWerelddeelCommand, Result<Land>> (new AddLandToWerelddeelCommandHandler ());
            mediator.Register<AddReisToBestemmingCommand, Result<Reis>> (new AddReisToBestemmingCommandHandler ());
            mediator.Register<DeleteKlantCommand, Result<Klant>> (new DeleteKlantCommandHandler ());
            mediator.Register<UpdateKlantCommand, Result<Klant>> (new UpdateKlantCommandHandler ());
            mediator.Register<GetReisMetIdQuery, Reis> (new GetReisMetIdQueryHandler ());
            mediator.Register<GetReizenNaarBestemmingQuery, IList<Reis>> (new GetReizenNaarBestemmingQueryHandler());

            //Console.WriteLine (mediator.GetHashCode());

            return mediator;
        }
    }
}
