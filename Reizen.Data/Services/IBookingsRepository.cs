using Reizen.Data.Models;
using Reizen.Data.Models.CQRS;
using Reizen.CommonClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Services
{
    public interface IBookingsRepository
    {
        Task<Result<IList<BookingDAL>>> GetBookingsAsync ();
        Task<Result<BookingDAL>> GetBookingWithIdAsync (int id);
        Task <Result<BookingDAL>> AddBookingAsync (BookingDAL? boeking);
        Task <Result<BookingDAL>> UpdateBookingAsync (BookingDAL? boeking, int id);
        Task <Result<BookingDAL>> DeleteBookingAsync (int id);
    }
}
