using Reizen.Data.Models;
using Reizen.CommonClasses;
using Reizen.Data.Models.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Services
{
    public interface IReizenRepository
    {
        Task<Result<IList<ReisDAL>>> GetReizenMetBestemmingAsync (string bestemming);

        Task<Result<ReisDAL>> AddReisToBestemmingAsync (ReisDAL reis, BestemmingDAL bestemming);

        Task<Result<ReisDAL>> GetReisMetIdAsync (int id);

        Task<Result<ReisDAL>> DeleteReisMetIdAsync (int id);
    }
}
