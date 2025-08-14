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
        public record DeleteBookingCommand(int id) : ICommand<Result<BookingDAL>>;

        public class DeleteBookingCommandHandler : ICommandHandler<DeleteBookingCommand, Result<BookingDAL>>
        {
            private ReizenContext _context;

            public DeleteBookingCommandHandler(ReizenContext context)
            {
                _context = context;
            }
        public async Task<Result<BookingDAL>> Handle (DeleteBookingCommand command)
            {
                try
                {
                    //if (command.id < 0)
                    //{
                    //    return Result<BookingDAL>.Failure ($"Id of booking cannot be negative");
                    //}
                    using (var transaction = await _context.Database.BeginTransactionAsync ())
                    {
                        try
                        {
                            var existingBooking = _context.Bookings.FirstOrDefault (el => el.Id == command.id);
                            if (existingBooking == null)
                            {
                                return Result<BookingDAL>.Failure ($"Booking does not exist");
                            }
                            
                            var boeking = new BookingDAL { Id = command.id };

                            _context.Attach (boeking);
                            _context.Bookings.Remove (boeking);
                            await _context.SaveChangesAsync ();
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
