using Reizen.Data.Repositories;
using Reizen.CommonClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Commands
{
    public sealed class UpdateBooking
    {
        public record UpdateBookingCommand(BookingDAL boeking, int id, ReizenContext context): ICommand<Result<BookingDAL>>;

        public class UpdateBookingCommandHandler : ICommandHandler<UpdateBookingCommand, Result<BookingDAL>>
        {
            public async Task<Result<BookingDAL>> Handle (UpdateBookingCommand command)
            {
                try
                {
                    if (command.id < 0)
                    {
                        return Result<BookingDAL>.Failure ("Invalid id");
                    }
                    if (command.boeking is null)
                    {
                        return Result<BookingDAL>.Failure ("Invalid booking data");
                    }
                    using (var transaction = await command.context.Database.BeginTransactionAsync ())
                    {
                        try
                        {
                            var existingBooking = await command.context.Bookings.FindAsync (command.id);
                            if (existingBooking == null)
                            {
                                return Result<BookingDAL>.Failure ($"Cannot find booking with ID");
                            }

                            existingBooking.NumberOfMinors = command.boeking.NumberOfMinors;
                            existingBooking.NumberOfAdults = command.boeking.NumberOfAdults;
                            existingBooking.HasCancellationInsurance = command.boeking.HasCancellationInsurance;

                            await command.context.SaveChangesAsync ();
                            await transaction.CommitAsync ();
                            return Result<BookingDAL>.Success(command.boeking);
                            
                        }
                        catch 
                        {
                            await transaction.RollbackAsync ();
                            throw;
                        }
                    }
                }
                catch (Exception ex) {
                    return Result<BookingDAL>.Failure ($"Error updating booking: {ex.Message}");
                }
                
            }
        }
    }
}
