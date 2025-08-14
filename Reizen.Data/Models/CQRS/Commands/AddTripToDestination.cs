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
        public record AddTripToDestinationCommand(TripDAL trip, DestinationDAL destination):ICommand<Result<TripDAL>>;

        public class AddTripToDestinationCommandHandler : ICommandHandler<AddTripToDestinationCommand, Result<TripDAL>>
        {
            private ReizenContext _context;

            public AddTripToDestinationCommandHandler (ReizenContext context)
            {
                _context = context;
            }
            public async Task<Result<TripDAL>> Handle (AddTripToDestinationCommand command)
            {
                try
                {
                    //if (command.trip == null || command.destination == null)
                    //    return Result<TripDAL>.Failure ($"Trip or destination cannot be null");
                    //if (command.trip.DateOfDeparture < DateOnly.FromDateTime (DateTime.Today))
                    //    return Result<TripDAL>.Failure ($"Trip departure cannot be in the past");
                    //if (command.trip.DestinationCode != command.destination.Code)
                    //    return Result<TripDAL>.Failure ($"Destination code does not match trip destination code");
                    using var transaction = await _context.Database.BeginTransactionAsync ();
                    {
                        try
                        {
                            var existingDestination = await _context.Destinations.FindAsync (command.destination.Code);
                            if (existingDestination is null)
                                return Result<TripDAL>.Failure ($"Destination not found");

                            existingDestination?.Trips.Add (command.trip);
                            await _context.SaveChangesAsync ();
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
