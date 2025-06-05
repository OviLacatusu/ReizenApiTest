using Reizen.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Services
{
    public interface ILandenWerelddelenRepository
    {
        Task<Result<IList<Werelddeel>>> GetWerelddelenAsync ();
        Task<Result<IList<Land>>> GetLandenVanWerelddeelAsync (string? werelddeelNaam);
        Task<Result<IList<Land>>> GetLandenAsync();
        Task<Result<IList<Bestemming>>> GetBestemmingenVanLandAsync (string? landnaam);
        Task<Result<IList<Bestemming>>> GetBestemmingenAsync ();
        Task<Result<Bestemming>> AddBestemmingAsync (Bestemming bestemming);
        Task<Result<Land>> GetLandMetIdAsync (int id);
        Task<Result<Land>> AddLandAsync (Land land);
        Task<Result<Land>> UpdateLandMetIdAsync (int id, Land land);
        Task<Result<Land>> DeleteLandMetIdAsync (int id);
        Task<Result<Bestemming>> DeleteBestemmingMetIdAsync (string code);
    }
}
