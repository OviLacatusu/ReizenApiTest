using Microsoft.EntityFrameworkCore;
using Reizen.Data.Models;
using Reizen.Data.Repositories;
using Reizen.Data.Models.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Services
{
    public class SQLKlantenRepository //: IKlantenRepository
    {
        private readonly ReizenContext _klantenContext;

        public SQLKlantenRepository (ReizenContext context)
        {
            _klantenContext = context;
        }
        public async Task<ICollection<KlantDAL>?> GetKlantenAsync ()
        {
            return await _klantenContext.Klanten.ToListAsync();
        }

        public async Task<KlantDAL?> GetKlantMetIdAsync (int id)
        {
            return await _klantenContext.Klanten.FirstOrDefaultAsync ( k => k.Id == id);
        }

        public async Task<ICollection<KlantDAL>?> GetKlantenMetNaamAsync (string naam)
        {
            return await _klantenContext.Klanten.Where (k => k.Voornaam.Contains (naam, StringComparison.OrdinalIgnoreCase) || 
                                                                           k.Familienaam.Contains (naam, StringComparison.OrdinalIgnoreCase)).ToListAsync();
        }

        public Task<KlantDAL?> AddKlant (KlantDAL klant)
        {
            throw new NotImplementedException ();
        }

        public Task<KlantDAL?> UpdateKlant (int id, KlantDAL klantDetails)
        {
            throw new NotImplementedException ();
        }

        Task<Result<KlantDAL>> AddKlantAsync (KlantDAL klant)
        {
            throw new NotImplementedException ();
        }

        Task<Result<KlantDAL>> UpdateKlantAsync (int id, KlantDAL klantDetails)
        {
            throw new NotImplementedException ();
        }

        public Task<Result<KlantDAL>> DeleteKlantAsync (int id)
        {
            throw new NotImplementedException ();
        }
    }
}
