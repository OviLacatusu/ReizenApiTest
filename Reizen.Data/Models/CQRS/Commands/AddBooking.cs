using Microsoft.EntityFrameworkCore;
using Reizen.Data.Repositories;
using Reizen.CommonClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Commands
{
    public sealed class AddBooking
    {
        public record AddBookingCommand(BookingDAL boeking) : ICommand<Result<BookingDAL>>;

        public class AddBookingCommandHandler : ICommandHandler<AddBookingCommand, Result<BookingDAL>>
        {
            private ReizenContext _context;

            public AddBookingCommandHandler(ReizenContext context)
            {   
                _context = context;
            }

            public async Task<Result<BookingDAL>> Handle (AddBookingCommand command)
            {
                try
                {
                    using (var transaction = await _context.Database.BeginTransactionAsync ())
                    {
                        try
                        {
                            var existingBooking = await _context.Bookings.Where (b => b.ClientId == command.boeking.ClientId && b.TripId == command.boeking.TripId).ToListAsync ();

                            if (existingBooking.Any ())
                            {
                                await transaction.RollbackAsync ();
                                return Result<BookingDAL>.Failure ("Booking already exists for this client");
                            }
                            var result = await _context.Bookings.AddAsync (command.boeking);
                            await _context.SaveChangesAsync ();
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
                catch (Exception ex) 
                {
                    return Result<BookingDAL>.Failure ($"Error adding booking: {ex.Message}");
                }
                
            }
        }
    }
}
