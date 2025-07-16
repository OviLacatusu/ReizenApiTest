using Microsoft.EntityFrameworkCore;
using Reizen.Data.Models;
using Reizen.Data.Repositories;
using Reizen.Data.Models.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reizen.CommonClasses;

namespace Reizen.Data.Services
{
    public class SQLClientsRepository //: IClientsRepository
    {
        private readonly ReizenContext _klantenContext;

        public SQLClientsRepository (ReizenContext context)
        {
            _klantenContext = context;
        }
        public async Task<ICollection<ClientDAL>?> GetClientsAsync ()
        {
            return await _klantenContext.Clients.ToListAsync();
        }

        public async Task<ClientDAL?> GetClientMetIdAsync (int id)
        {
            return await _klantenContext.Clients.FirstOrDefaultAsync ( k => k.Id == id);
        }

        public async Task<ICollection<ClientDAL>?> GetClientsMetNameAsync (string name)
        {
            return await _klantenContext.Clients.Where (k => k.FirstName.Contains (name, StringComparison.OrdinalIgnoreCase) || 
                                                                           k.FamilyName.Contains (name, StringComparison.OrdinalIgnoreCase)).ToListAsync();
        }

        public Task<ClientDAL?> AddClient (ClientDAL klant)
        {
            throw new NotImplementedException ();
        }

        public Task<ClientDAL?> UpdateClient (int id, ClientDAL klantDetails)
        {
            throw new NotImplementedException ();
        }

        Task<Result<ClientDAL>> AddClientAsync (ClientDAL klant)
        {
            throw new NotImplementedException ();
        }

        Task<Result<ClientDAL>> UpdateClientAsync (int id, ClientDAL klantDetails)
        {
            throw new NotImplementedException ();
        }

        public Task<Result<ClientDAL>> DeleteClientAsync (int id)
        {
            throw new NotImplementedException ();
        }
    }
}
