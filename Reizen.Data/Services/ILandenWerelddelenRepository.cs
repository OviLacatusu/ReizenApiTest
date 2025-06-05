using Reizen.Data.Models;
using Reizen.Data.Models.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Services
{
    public interface ILandenWerelddelenRepository
    {
        Task<Result<IList<WerelddeelDAL>>> GetWerelddelenAsync ();
        Task<Result<IList<LandDAL>>> GetLandenVanWerelddeelAsync (string? werelddeelNaam);
        Task<Result<IList<LandDAL>>> GetLandenAsync();
        Task<Result<IList<BestemmingDAL>>> GetBestemmingenVanLandAsync (string? landnaam);
        Task<Result<IList<BestemmingDAL>>> GetBestemmingenAsync ();
        Task<Result<BestemmingDAL>> AddBestemmingAsync (BestemmingDAL bestemming);
        Task<Result<LandDAL>> GetLandMetIdAsync (int id);
        Task<Result<LandDAL>> AddLandAsync (LandDAL land);
        Task<Result<LandDAL>> UpdateLandMetIdAsync (int id, LandDAL land);
        Task<Result<LandDAL>> DeleteLandMetIdAsync (int id);
        Task<Result<BestemmingDAL>> DeleteBestemmingMetIdAsync (string code);
    }
}
