using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Diagnostics;

namespace Reizen.Data.Models.CQRS
{
    internal class Mediator : IMediator
    {
        private List<Object> _queryHandlers = new List<Object> ();
        private List<Object> _commandHandlers = new List<Object> ();
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
    }
}
