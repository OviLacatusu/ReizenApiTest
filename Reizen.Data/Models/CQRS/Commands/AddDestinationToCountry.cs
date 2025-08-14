using Reizen.Data.Repositories;
using Reizen.CommonClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Commands
{
    public sealed class AddDestinationToCountry
    {
        public record AddDestinationToCountryCommand (DestinationDAL destination, CountryDAL country) : ICommand<Result<DestinationDAL>>;

        public class AddDestinationToCountryCommandHandler : ICommandHandler<AddDestinationToCountryCommand, Result<DestinationDAL>>
        {
            private ReizenContext _context;

            public AddDestinationToCountryCommandHandler (ReizenContext context)
            {
                _context = context;
            }
        public async Task<Result<DestinationDAL>> Handle (AddDestinationToCountryCommand command)
            {
                try
                {
                    //if (command.country is null)
                    //{
                    //    return Result<DestinationDAL>.Failure ($"Invalid country data");
                    //}
                    //if (command.destination is null)
                    //{
                    //    return Result<DestinationDAL>.Failure ($"Invalid destination data");
                    //}
                    using var transaction = await _context.Database.BeginTransactionAsync ();
                    {
                        try
                        {
                            var Country = await _context.Countries.FindAsync (command.country.Id);
                            
                            if (Country == null)
                            {
                                return Result<DestinationDAL>.Failure ($"Country not found"); 
                            }
                            if (Country?.Destinations.Where (el => el.PlaceName == command.destination.PlaceName).Count () > 0)
                            {
                                return Result<DestinationDAL>.Failure ($"Destination already exists for this Country");
                            }

                            Country.Destinations.Add(command.destination);
                            await _context.SaveChangesAsync ();
                            await transaction.CommitAsync ();

                            return Result<DestinationDAL>.Success(command.destination);
                        }
                        catch
                        {
                            await transaction.RollbackAsync ();
                            throw;
                        }
                    }
                }
                catch (Exception ex) {
                    return Result<DestinationDAL>.Failure ($"Error adding destination to Country: {ex.Message}");
                }
            }
        }
    }
}
