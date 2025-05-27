using Reizen.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Domain.Services
{
    public interface IReizenRepository
    {
        Task<ICollection<Reis>?> GetReizenMetBestemmingAsync (string bestemming);

        Task<Result<Reis>> AddReisToBestemmingAsync (Reis reis, Bestemming bestemming);

        Task<Reis?> GetReisMetIdAsync (int id);

        Task<Result<Reis>> DeleteReisMetIdAsync (int id);
    }
}
