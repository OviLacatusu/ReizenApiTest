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

namespace Reizen.Data.Models.CQRS
{
    public class Mediator : IMediator
    {
        private List<Object> _queryHandlers = new List<Object> ();
        private List<Object> _commandHandlers = new List<Object> ();
        private static Mediator _mediator = null;
        public async Task<TResult?> ExecuteCommand<TCommand, TResult> (TCommand command) where TCommand : ICommand<TResult>
                                                                                          where TResult : class
        {
            foreach (var handler in _commandHandlers)
            {
                if (handler is ICommandHandler<TCommand, TResult>)
                {
                    return await ((ICommandHandler<TCommand, TResult>) handler).Execute (command);
                }
            }
            return null;
        }

        public Task ExecuteCommand<TCommand> (TCommand command) where TCommand : ICommand
        {
            throw new NotImplementedException ();
        }

        public async Task<TResult?> ExecuteQuery<TQuery, TResult> (TQuery query) where TQuery : IQuery<TResult>
                                                                            where TResult : class
        {
            foreach (var handler in _queryHandlers)
            {
                if (handler is IQueryHandler<TQuery, TResult>)
                {
                    return await ((IQueryHandler<TQuery, TResult>)handler).Execute (query);
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

        public static Mediator MediatorFactory ()
        {
            if (_mediator == null)
            {
                var mediator = new Mediator ();

                mediator.Register<GetBestemmingenVanLandQuery, IList<Bestemming>> (new GetBestemmingenVanLandQueryHandler());
                mediator.Register<GetKlantenQuery, IList<Klant>> (new GetKlantenQueryHandler ());
                mediator.Register<GetKlantMetIDQuery, Klant> (new GetKlantMetIDQueryHandler());
                mediator.Register<GetKlantMetNaamQuery, Klant> (new GetKlantMetNaamQueryHandler ());
                mediator.Register<GetLandenVanWerelddeelQuery, IList<Land>> (new GetLandenVanWerelddeelQueryHandler());
                mediator.Register<GetWerelddelenQuery, IList<Werelddeel>> (new GetWerelddelenQueryHandler());
                mediator.Register<AddBestemmingToLandCommand, int> (new AddBestemmingToLandCommandHandler ());
                mediator.Register<AddKlantCommand, int> (new AddKlantCommandHandler ());
                mediator.Register<AddLandToWerelddeelCommand, int> (new AddLandToWerelddeelCommandHandler ());
                mediator.Register<AddReisToBestemmingCommand, int> (new AddReisToBestemmingCommandHandler ());

                _mediator = mediator;
            }
            return _mediator;
        }
    }
}
