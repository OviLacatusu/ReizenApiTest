
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS
{
    public interface ICommand : IRequest;
    public interface ICommand<TResult> : IRequest;
}
