using Reizen.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Services
{
    public interface IKlantenRepository
    {
        Task<Result<IList<Klant>>> GetKlantenAsync ();
        Task<Result<Klant>> GetKlantMetIdAsync (int id);
        Task<Result<IList<Klant>>> GetKlantenMetNaamAsync (string naam);
        Task<Result<Klant>> AddKlantAsync (Klant klant);
        Task<Result<Klant>> UpdateKlantAsync (int id, Klant klantDetails);
        Task<Result<Klant>> DeleteKlantAsync (int id);

    }
}
