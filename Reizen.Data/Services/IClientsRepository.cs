using Reizen.Data.Models;
using Reizen.CommonClasses;
using Reizen.Data.Models.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Services
{
    public interface IClientsRepository
    {
        Task<Result<IList<ClientDAL>>> GetClientsAsync ();
        Task<Result<ClientDAL>> GetClientWithIdAsync (int id);
        Task<Result<IList<ClientDAL>>> GetClientsWithNameAsync (string name);
        Task<Result<ClientDAL>> AddClientAsync (ClientDAL klant);
        Task<Result<ClientDAL>> UpdateClientAsync (int id, ClientDAL klantDetails);
        Task<Result<ClientDAL>> DeleteClientAsync (int id);

    }
}
