using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS
{
    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        void Handle (TCommand command);
    }
    public interface ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
    {
        Task<TResult> Handle (TCommand command);
    }
}
