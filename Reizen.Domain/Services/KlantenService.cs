using Reizen.Data.Models;
using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Domain.Services
{
    public sealed class KlantenService (IKlantenRepository klantenRepository) : IKlantenRepository
    {
        public async Task<ICollection<Klant>?> GetKlantenAsync ()
        {
            return await klantenRepository.GetKlantenAsync ();
        }
        public async Task<Klant?> GetKlantMetIdAsync (int id)
        {
            return await klantenRepository.GetKlantMetIdAsync (id);
        }
        public async Task<Klant?> GetKlantMetNaamAsync (string naam)
        {
            return await klantenRepository.GetKlantMetNaamAsync(naam);
        }
    }
}
