using Microsoft.EntityFrameworkCore;
using Reizen.CommonClasses;
using Reizen.Data.Models;
using Reizen.Data.Models.CQRS;
using Reizen.Data.Models.CQRS.Commands;
using Reizen.Data.Models.CQRS.Queries;
using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Reizen.Data.Models.CQRS.Commands.AddDestination;
using static Reizen.Data.Models.CQRS.Commands.AddCountry;
using static Reizen.Data.Models.CQRS.Commands.AddCountryToContinent;
using static Reizen.Data.Models.CQRS.Commands.DeleteDestination;
using static Reizen.Data.Models.CQRS.Commands.DeleteCountry;
using static Reizen.Data.Models.CQRS.Commands.UpdateCountry;
using static Reizen.Data.Models.CQRS.Queries.GetDestinations;
using static Reizen.Data.Models.CQRS.Queries.GetDestinationsOfCountry;
using static Reizen.Data.Models.CQRS.Queries.GetClients;
using static Reizen.Data.Models.CQRS.Queries.GetClientWithId;
using static Reizen.Data.Models.CQRS.Queries.GetClientsWithName;
using static Reizen.Data.Models.CQRS.Queries.GetCountries;
using static Reizen.Data.Models.CQRS.Queries.GetCountriesOfContinent;
using static Reizen.Data.Models.CQRS.Queries.GetCountryWithId;
using static Reizen.Data.Models.CQRS.Queries.GetTripsToDestination;
using static Reizen.Data.Models.CQRS.Queries.GetContinents;

namespace Reizen.Data.Services
{
    public sealed class CountriesService (IMediator mediator, IDbContextFactory<ReizenContext> factory) : ICountriesContinentsRepository
    {
        public async Task<Result<IList<ContinentDAL>>> GetContinentsAsync ()
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteQuery<GetContinentsQuery, Result<IList<ContinentDAL>>> (new GetContinentsQuery (context));
            }
        }

        public async Task<Result<IList<CountryDAL>>> GetCountriesOfContinentAsync (string? ContinentName)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteQuery<GetCountriesOfContinentQuery, Result<IList<CountryDAL>>> (new GetCountriesOfContinentQuery (ContinentName, context));
            }
        }

        public async Task<Result<IList<DestinationDAL>>> GetDestinationsOfCountryAsync (string? Countryname)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteQuery<GetDestinationsOfCountryQuery, Result<IList<DestinationDAL>>> (new GetDestinationsOfCountryQuery (Countryname, context));
            }
        }

        public async Task<Result<IList<DestinationDAL>>> GetDestinationsAsync ()
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteQuery<GetDestinationsQuery, Result<IList<DestinationDAL>>> (new GetDestinationsQuery (context));
            }
        }

        public async Task<Result<CountryDAL>> AddCountryToContinentAsync (CountryDAL Country, ContinentDAL deel)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteCommand<AddCountryToContinentCommand, Result<CountryDAL>> (new AddCountryToContinentCommand(Country, deel, context));
            }
        }

        public async Task<Result<DestinationDAL>> AddDestinationAsync (DestinationDAL Destination)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteCommand<AddDestinationCommand, Result<DestinationDAL>> (new AddDestinationCommand(context, Destination));
            }
        }

        public async Task<Result<CountryDAL>> GetCountryWithIdAsync (int id)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteQuery<GetCountryWithIdQuery, Result<CountryDAL>> (new GetCountryWithIdQuery ( id, context));
            }
        }

        public async Task<Result<CountryDAL>> AddCountryAsync (CountryDAL Country)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteCommand<AddCountryCommand, Result<CountryDAL>> (new AddCountryCommand (Country, context));
            }
        }

        public async Task<Result<CountryDAL>> UpdateCountryWithIdAsync (int id, CountryDAL Country)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteCommand<UpdateCountryCommand, Result<CountryDAL>> (new UpdateCountryCommand (id, Country, context));
            }
        }
        public async Task<Result<CountryDAL>> DeleteCountryWithIdAsync (int id)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteCommand <DeleteCountryCommand, Result<CountryDAL>> (new DeleteCountryCommand (id, context));
            }
        }

        public async Task<Result<DestinationDAL>> DeleteDestinationWithIdAsync (string code)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteCommand<DeleteDestinationCommand, Result<DestinationDAL>> (new DeleteDestinationCommand (code, context));
            }
        }

        public async Task<Result<IList<CountryDAL>>> GetCountriesAsync ()
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteQuery<GetCountriesQuery, Result<IList<CountryDAL>>> (new GetCountriesQuery (context));
            }
        }


    }
}
