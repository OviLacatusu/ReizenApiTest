using Reizen.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Repositories
{
    public interface IReizenRepository
    {
        Task<ICollection<Reis>?> GetReizenMetBestemmingAsync (string bestemming);

        Task<Wrapper<int>> AddReisToBestemming (Reis reis, Bestemming bestemming);
    }
}
