using Reizen.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Domain.Services
{
    public interface IBoekingenRepository
    {
        Task<IList<Boeking>?> GetBoekingenAsync ();
        Task<Boeking?> GetBoekingMetIdAsync (int id);
        Task <Result<Boeking>> AddBoekingAsync (Boeking? boeking);
        Task <Result<Boeking>> UpdateBoekingAsync (Boeking? boeking, int id);
        Task <Result<Boeking>> DeleteBoekingAsync (int id);
    }
}
