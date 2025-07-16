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
    public interface ITripsRepository
    {
        Task<Result<IList<TripDAL>>> GetTripsToDestinationAsync (string Destination);

        Task<Result<TripDAL>> AddTripToDestinationAsync (TripDAL trip, DestinationDAL Destination);

        Task<Result<TripDAL>> GetTripWithIdAsync (int id);

        Task<Result<TripDAL>> DeleteTripWithIdAsync (int id);
    }
}
