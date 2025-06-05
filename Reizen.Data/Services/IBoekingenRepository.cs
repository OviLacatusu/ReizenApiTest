using Reizen.Data.Models;
using Reizen.Data.Models.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Services
{
    public interface IBoekingenRepository
    {
        Task<Result<IList<BoekingDAL>>> GetBoekingenAsync ();
        Task<Result<BoekingDAL>> GetBoekingMetIdAsync (int id);
        Task <Result<BoekingDAL>> AddBoekingAsync (BoekingDAL? boeking);
        Task <Result<BoekingDAL>> UpdateBoekingAsync (BoekingDAL? boeking, int id);
        Task <Result<BoekingDAL>> DeleteBoekingAsync (int id);
    }
}
