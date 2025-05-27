using Reizen.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Services
{
    public interface IReizenRepository
    {
        Task<Result<IList<Reis>>> GetReizenMetBestemmingAsync (string bestemming);

        Task<Result<Reis>> AddReisToBestemmingAsync (Reis reis, Bestemming bestemming);

        Task<Result<Reis>> GetReisMetIdAsync (int id);

        Task<Result<Reis>> DeleteReisMetIdAsync (int id);
    }
}
