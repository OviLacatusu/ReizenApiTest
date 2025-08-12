using Reizen.Data.Repositories;
using Reizen.CommonClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Commands
{
    public sealed class DeleteBooking
    {
        public record DeleteBookingCommand(int id, ReizenContext context) : ICommand<Result<BookingDAL>>;

        public class DeleteBookingCommandHandler : ICommandHandler<DeleteBookingCommand, Result<BookingDAL>>
        {
            public async Task<Result<BookingDAL>> Handle (DeleteBookingCommand command)
            {
                try
                {
                    if (command.id < 0)
                    {
                        return Result<BookingDAL>.Failure ($"Id of booking cannot be negative");
                    }
                    using (var transaction = await command.context.Database.BeginTransactionAsync ())
                    {
                        try
                        {
                            var existingBooking = command.context.Bookings.FirstOrDefault (el => el.Id == command.id);
                            if (existingBooking == null)
                            {
                                return Result<BookingDAL>.Failure ($"Booking does not exist");
                            }
                            
                            var boeking = new BookingDAL { Id = command.id };

                            command.context.Attach (boeking);
                            command.context.Bookings.Remove (boeking);
                            await command.context.SaveChangesAsync ();
                            await transaction.CommitAsync ();

                            return Result<BookingDAL>.Success(boeking);
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
                    return Result<BookingDAL>.Failure ($"Error deleting booking: {ex.Message}");
                }
            }
        }
    }
}
