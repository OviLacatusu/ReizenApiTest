using Reizen.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Domain.Services
{
    public interface IKlantenRepository
    {
        Task<ICollection<Klant>?> GetKlantenAsync ();
        Task<Klant?> GetKlantMetIdAsync (int id);
        Task<ICollection<Klant>?> GetKlantenMetNaamAsync (string naam);
        Task<Result<Klant>> AddKlantAsync (Klant klant);
        Task<Result<Klant>> UpdateKlantAsync (int id, Klant klantDetails);
        Task<Result<Klant>> DeleteKlantAsync (int id);

    }
}
