using Reizen.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Repositories
{
    public interface ILandenWerelddelenRepository
    {
        Task<ICollection<Werelddeel>?> GetWerelddelenAsync ();
        Task<ICollection<Land>?> GetLandenVanWerelddeelAsync (string? werelddeelNaam);
        Task<ICollection<Bestemming>?> GetBestemmingenVanLandAsync (string? landnaam);
        Task<ICollection<Bestemming>?> GetBestemmingenAsync ();
        Task<Bestemming> AddBestemmingAsync (Bestemming bestemming);
        Task<Land?> GetLandMetIdAsync (int id);
        Task<Land?> AddLandAsync (Land land);
        Task<Land?> UpdateLandMetIdAsync (int id, Land land);
        Task<Land?> DeleteLandMetIdAsync (int id);
        Task<Bestemming?> DeleteBestemmingMetIdAsync (string code);
    }
}
