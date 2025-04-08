using Microsoft.EntityFrameworkCore;
using Reizen.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Repositories
{
    public class SQLKlantenRepository : IKlantenRepository
    {
        private readonly ReizenContext _klantenContext;

        public SQLKlantenRepository (ReizenContext context)
        {
            _klantenContext = context;
        }
        public async Task<ICollection<Klant>?> GetKlantenAsync ()
        {
            return await _klantenContext.Klanten.ToListAsync();
        }

        public async Task<Klant?> GetKlantMetIdAsync (int id)
        {
            return await _klantenContext.Klanten.FirstOrDefaultAsync ( k => k.Id == id);
        }

        public async Task<ICollection<Klant>?> GetKlantenMetNaamAsync (string naam)
        {
            return await _klantenContext.Klanten.Where (k => k.Voornaam.Contains (naam, StringComparison.OrdinalIgnoreCase) || 
                                                                           k.Familienaam.Contains (naam, StringComparison.OrdinalIgnoreCase)).ToListAsync();
        }

        public Task<int> AddKlant (Klant klant)
        {
            throw new NotImplementedException ();
        }

        public Task<bool> UpdateKlant (int id, Klant klantDetails)
        {
            throw new NotImplementedException ();
        }

        Task<Klant?> IKlantenRepository.AddKlantAsync (Klant klant)
        {
            throw new NotImplementedException ();
        }

        Task<Klant?> IKlantenRepository.UpdateKlantAsync (int id, Klant klantDetails)
        {
            throw new NotImplementedException ();
        }

        public Task<Wrapper<int>> DeleteKlantAsync (int id)
        {
            throw new NotImplementedException ();
        }
    }
}
