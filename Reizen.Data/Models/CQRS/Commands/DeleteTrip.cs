using Reizen.Data.Repositories;
using Reizen.CommonClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Commands
{
    public sealed class DeleteTrip
    {
        public record DeleteTripCommand(int id, ReizenContext context):ICommand<Result<TripDAL>>;

        public class DeleteTripCommandHandler : ICommandHandler<DeleteTripCommand, Result<TripDAL>>
        {
            public async Task<Result<TripDAL>> Handle (DeleteTripCommand command)
            {
                try
                {
                    using (var transaction = await command.context.Database.BeginTransactionAsync ())
                    {
                        try
                        {
                            var existingTrip = await command.context.Trips.FindAsync (command.id);
                            if (existingTrip == null)
                            {
                                return Result<TripDAL>.Failure ($"Trip with ID not found");
                            }
                            if (existingTrip.Bookings.Where (b => b.Trip.DateOfDeparture > DateOnly.FromDateTime (DateTime.Today)).Any ())
                            {
                                return Result<TripDAL>.Failure ($"Cannot delete trip with active bookings");
                            }
                            TripDAL? trip = new TripDAL { Id = command.id };

                            command.context.Attach (trip);
                            command.context.Trips.Remove (trip);
                            await command.context.SaveChangesAsync ();
                            await transaction.CommitAsync ();
                            return Result<TripDAL>.Success (trip);
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
                    return Result<TripDAL>.Failure ($"Error deleting trip: {ex.Message}");
                }
            }
        }
    }
}
