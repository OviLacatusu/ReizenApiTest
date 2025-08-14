using Microsoft.EntityFrameworkCore;
using Reizen.Data.Models;
using Reizen.Data.Models.CQRS;
using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Reizen.Data.Models.CQRS.Commands.AddBooking;
using static Reizen.Data.Models.CQRS.Commands.DeleteBooking;
using static Reizen.Data.Models.CQRS.Commands.UpdateBooking;
using static Reizen.Data.Models.CQRS.Queries.GetBookings;
using static Reizen.Data.Models.CQRS.Queries.GetBookingWithId;
using Reizen.CommonClasses;

namespace Reizen.Data.Services
{
    public class BookingsService (IMediator mediator, IDbContextFactory<ReizenContext> factory) : IBookingsRepository
    {
        public async Task<Result<BookingDAL>> AddBookingAsync (BookingDAL? boeking)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteCommand<AddBookingCommand, Result<BookingDAL>> (new AddBookingCommand (boeking));
            }
        }

        public async Task<Result<BookingDAL>> DeleteBookingAsync (int id)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteCommand<DeleteBookingCommand, Result<BookingDAL>> (new DeleteBookingCommand (id));
            }
        }

        public async Task<Result<IList<BookingDAL>>> GetBookingsAsync ()
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteQuery<GetBookingsQuery, Result<IList<BookingDAL>>> (new GetBookingsQuery (context));
            }
        }

        public async Task<Result<BookingDAL>> GetBookingWithIdAsync (int id)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteQuery<GetBookingWithIdQuery, Result<BookingDAL>> (new GetBookingWithIdQuery (id, context)); 
            }
        }

        public async Task<Result<BookingDAL>> UpdateBookingAsync (BookingDAL? boeking, int id)
        {
            using (var context = factory.CreateDbContext ())
            {
                return await mediator.ExecuteCommand<UpdateBookingCommand, Result<BookingDAL>> (new UpdateBookingCommand (boeking, id));
            }
        }
    }
}
