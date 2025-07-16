using Microsoft.EntityFrameworkCore.Diagnostics;
using Reizen.Data.Repositories;
using Reizen.CommonClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Commands
{
    public sealed class AddTripToDestination
    {
        public record AddTripToDestinationCommand(TripDAL trip, DestinationDAL Destination, ReizenContext context):ICommand<Result<TripDAL>>;

        public class AddTripToDestinationCommandHandler : ICommandHandler<AddTripToDestinationCommand, Result<TripDAL>>
        {
            public async Task<Result<TripDAL>> Handle (AddTripToDestinationCommand command)
            {
                try
                {
                    using var transaction = await command.context.Database.BeginTransactionAsync ();
                    {
                        try
                        {
                            var existingDestination = await command.context.Destinations.FindAsync (command.Destination.Code);

                            if (command.trip == null || command.Destination == null)
                                return Result<TripDAL>.Failure ($"Trip or destination cannot be null");
                            if (command.trip.DateOfDeparture < DateOnly.FromDateTime (DateTime.Today))
                                return Result<TripDAL>.Failure ($"Trip departure cannot be in the past");
                            if (existingDestination is null)
                                return Result<TripDAL>.Failure ($"Destination not found");
                            if (command.trip.DestinationCode != command.Destination.Code)
                                return Result<TripDAL>.Failure ($"Destination code does not match trip destination code");

                            existingDestination?.Trips.Add (command.trip);
                            await command.context.SaveChangesAsync ();
                            await transaction.CommitAsync ();

                            return Result<TripDAL>.Success(command.trip);
                        }
                        catch { 
                            await transaction.RollbackAsync ();
                            throw;
                        }
                    }
                }
                catch (Exception ex) {
                    return Result<TripDAL>.Failure ($"Error adding Trip to destination: {ex.Message}");
                }
            }
        }
    }
}
