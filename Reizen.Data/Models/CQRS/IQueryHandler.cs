using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS
{
    public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
                                                    //where TResult : class
    {
        Task<TResult?> Execute (TQuery query);
    }
}
