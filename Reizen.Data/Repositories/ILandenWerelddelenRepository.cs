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

    }
}
