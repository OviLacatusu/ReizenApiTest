using Reizen.Data.Models;
using Reizen.Data.Models.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Services
{
    public interface IKlantenRepository
    {
        Task<Result<IList<KlantDAL>>> GetKlantenAsync ();
        Task<Result<KlantDAL>> GetKlantMetIdAsync (int id);
        Task<Result<IList<KlantDAL>>> GetKlantenMetNaamAsync (string naam);
        Task<Result<KlantDAL>> AddKlantAsync (KlantDAL klant);
        Task<Result<KlantDAL>> UpdateKlantAsync (int id, KlantDAL klantDetails);
        Task<Result<KlantDAL>> DeleteKlantAsync (int id);

    }
}
