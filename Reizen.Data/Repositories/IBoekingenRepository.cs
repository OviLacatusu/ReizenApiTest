using Reizen.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Repositories
{
    public interface IBoekingenRepository
    {
        Task<IList<Boeking>?> GetBoekingenAsync ();
        Task<Boeking?> GetBoekingMetIdAsync (int id);
        Task<Boeking?> AddBoekingAsync (Boeking? boeking);
        Task<Boeking?> UpdateBoekingAsync (Boeking? boeking, int id);
        Task<Boeking?> DeleteBoekingAsync (int id);
    }
}
