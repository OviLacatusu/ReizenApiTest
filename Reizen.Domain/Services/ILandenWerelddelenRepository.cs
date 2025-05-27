using Reizen.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Domain.Services
{
    public interface ILandenWerelddelenRepository
    {
        Task<ICollection<Werelddeel>?> GetWerelddelenAsync ();
        Task<ICollection<Land>?> GetLandenVanWerelddeelAsync (string? werelddeelNaam);
        Task<ICollection<Bestemming>?> GetBestemmingenVanLandAsync (string? landnaam);
        Task<ICollection<Bestemming>?> GetBestemmingenAsync ();
        Task<Result<Bestemming>> AddBestemmingAsync (Bestemming bestemming);
        Task<Land?> GetLandMetIdAsync (int id);
        Task<Result<Land>> AddLandAsync (Land land);
        Task<Result<Land>> UpdateLandMetIdAsync (int id, Land land);
        Task<Result<Land>> DeleteLandMetIdAsync (int id);
        Task<Result<Bestemming>> DeleteBestemmingMetIdAsync (string code);
    }
}
