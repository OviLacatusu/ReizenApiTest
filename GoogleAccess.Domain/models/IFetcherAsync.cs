using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleAccess.Domain.Models
{
    public interface IFetcherAsync<T>
    {
        Task<T> FetchDataAsync();
    }
}
