using Microsoft.EntityFrameworkCore;
using Reizen.CommonClasses;
using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Commands
{
    public sealed class DeleteCountry
    {
        public record DeleteCountryCommand (int id, ReizenContext context) : ICommand<Result<CountryDAL>>;

        public class DeleteCountryCommandHandler (int id) : ICommandHandler<DeleteCountryCommand, Result<CountryDAL>>
        {
            public async Task<Result<CountryDAL>> Handle (DeleteCountryCommand command)
            {
                try
                {
                    using (var transaction = await command.context.Database.BeginTransactionAsync ())
                    {
                        try
                        {
                            var existingCountry = await command.context.Countries.FindAsync (command.id);
                            if (existingCountry == null)
                            {
                                return Result<CountryDAL>.Failure ($"Country with ID not found");
                            }
                            if (existingCountry.Destinations.Where(b => b.Trips.Where (b => b.DateOfDeparture > DateOnly.FromDateTime (DateTime.Today)).Any()).Any ())
                            {
                                return Result<CountryDAL>.Failure ($"Cannot delete country with active bookings");
                            }
                            CountryDAL? Country = new CountryDAL { Id = command.id };

                            command.context.Attach (Country);
                            command.context.Countries.Remove (Country);
                            await command.context.SaveChangesAsync ();
                            await transaction.CommitAsync ();
                            return Result<CountryDAL>.Success(Country);
                        }
                        catch
                        {
                            await transaction.RollbackAsync ();
                            throw;
                        }
                    }
                }
                catch (Exception ex)
                {
                    return Result<CountryDAL>.Failure ($"Error deleting Country: {ex.Message}");
                }
}
        }
    }
}
