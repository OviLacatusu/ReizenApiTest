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
    public interface ICountriesContinentsRepository
    {
        Task<Result<IList<ContinentDAL>>> GetContinentsAsync ();
        Task<Result<IList<CountryDAL>>> GetCountriesOfContinentAsync (string? ContinentName);
        Task<Result<IList<CountryDAL>>> GetCountriesAsync(); //
        Task<Result<IList<DestinationDAL>>> GetDestinationsOfCountryAsync (string? Countryname); //
        Task<Result<IList<DestinationDAL>>> GetDestinationsAsync ();
        Task<Result<DestinationDAL>> AddDestinationAsync (DestinationDAL Destination);
        Task<Result<CountryDAL>> GetCountryWithIdAsync (int id);
        Task<Result<CountryDAL>> AddCountryAsync (CountryDAL Country);
        Task<Result<CountryDAL>> UpdateCountryWithIdAsync (int id, CountryDAL Country);
        Task<Result<CountryDAL>> DeleteCountryWithIdAsync (int id);
        Task<Result<DestinationDAL>> DeleteDestinationWithIdAsync (string code);
    }
}
