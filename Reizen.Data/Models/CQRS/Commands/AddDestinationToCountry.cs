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
        public record AddDestinationToCountryCommand (DestinationDAL Destination, CountryDAL Country, ReizenContext context) : ICommand<Result<DestinationDAL>>;

        public class AddDestinationToCountryCommandHandler : ICommandHandler<AddDestinationToCountryCommand, Result<DestinationDAL>>
        {
            public async Task<Result<DestinationDAL>> Handle (AddDestinationToCountryCommand command)
            {
                try
                {
                    using var transaction = await command.context.Database.BeginTransactionAsync ();
                    {
                        try
                        {
                            var Country = await command.context.Countries.FindAsync (command.Country.Id);
                            
                            if (Country == null)
                            {
                                return Result<DestinationDAL>.Failure ($"Country does not exist"); 
                            }
                            if (Country?.Destinations.Where (el => el.PlaceName == command.Destination.PlaceName).Count () > 0)
                            {
                                return Result<DestinationDAL>.Failure ($"Destination already exists for this Country");
                            }

                            Country.Destinations.Add(command.Destination);
                            await command.context.SaveChangesAsync ();
                            await transaction.CommitAsync ();

                            return Result<DestinationDAL>.Success(command.Destination);
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
