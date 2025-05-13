using Reizen.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Repositories
{
    public interface IKlantenRepository
    {
        Task<ICollection<Klant>?> GetKlantenAsync ();
        Task<Klant?> GetKlantMetIdAsync (int id);
        Task<ICollection<Klant>?> GetKlantenMetNaamAsync (string naam);
        Task<Klant?> AddKlantAsync (Klant klant);
        Task<Klant?> UpdateKlantAsync (int id, Klant klantDetails);
        Task<Klant?> DeleteKlantAsync (int id);

    }
}
