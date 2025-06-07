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
using static Reizen.Data.Models.CQRS.Commands.AddBoeking;
using static Reizen.Data.Models.CQRS.Queries.GetLanden;
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

        // Not all queries and commands are plugged in
        public static IMediator? MediatorFactory ()
        {
            var mediator = new Mediator ();

            mediator.Register<GetBestemmingenVanLandQuery, Result<IList<BestemmingDAL>>> (new GetBestemmingenVanLandQueryHandler());
            mediator.Register<GetKlantenQuery, Result<IList<KlantDAL>>> (new GetKlantenQueryHandler ());
            mediator.Register<GetKlantMetIDQuery, Result<KlantDAL>> (new GetKlantMetIDQueryHandler());
            mediator.Register<GetKlantMetNaamQuery, Result<IList<KlantDAL>>> (new GetKlantMetNaamQueryHandler ());
            mediator.Register<GetLandenVanWerelddeelQuery, Result<IList<LandDAL>>> (new GetLandenVanWerelddeelQueryHandler());
            mediator.Register<GetWerelddelenQuery, Result<IList<WerelddeelDAL>>> (new GetWerelddelenQueryHandler());
            mediator.Register<AddBestemmingToLandCommand, Result<BestemmingDAL>> (new AddBestemmingToLandCommandHandler ());
            mediator.Register<AddKlantCommand, Result<KlantDAL>> (new AddKlantCommandHandler ());
            mediator.Register<AddLandToWerelddeelCommand, Result<LandDAL>> (new AddLandToWerelddeelCommandHandler ());
            mediator.Register<AddReisToBestemmingCommand, Result<ReisDAL>> (new AddReisToBestemmingCommandHandler ());
            mediator.Register<AddBoekingCommand, Result<BoekingDAL>> (new AddBoekingCommandHandler ());
            mediator.Register<DeleteKlantCommand, Result<KlantDAL>> (new DeleteKlantCommandHandler ());
            mediator.Register<UpdateKlantCommand, Result<KlantDAL>> (new UpdateKlantCommandHandler ());
            mediator.Register<GetReisMetIdQuery, Result<ReisDAL>> (new GetReisMetIdQueryHandler ());
            mediator.Register<GetLandenQuery, Result<IList<LandDAL>>> (new GetLandenQueryHandler ());
            mediator.Register<GetReizenNaarBestemmingQuery, Result<IList<ReisDAL>>> (new GetReizenNaarBestemmingQueryHandler());

            //Console.WriteLine (mediator.GetHashCode());

            return mediator;
        }
    }
}
