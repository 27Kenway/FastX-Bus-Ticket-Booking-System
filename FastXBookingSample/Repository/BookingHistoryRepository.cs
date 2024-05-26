using FastXBookingSample.Interface;
using FastXBookingSample.Models;
using Microsoft.EntityFrameworkCore;

namespace FastXBookingSample.Repository
{
    public class BookingHistoryRepository : IBookingHistoryRepository
    {
        private readonly BookingContext _context;
        public BookingHistoryRepository(BookingContext context)
        {
            _context = context;
        }

        public List<BookingHistory> GetAll()
        {
            return _context.BookingHistories.ToList();
        }

        public List<BookingHistory> GetAllBookingsByBus(int busId)
        {
            Bus bus=_context.Buses.FirstOrDefault(x=>x.BusId==busId);
            var bookingHistories = _context.BookingHistories
                .Include(x => x.Booking)
                    .ThenInclude(b => b.Bus)  // Then include the Bus related to each Booking
                .Include(x => x.Booking)  // Include the Booking related to each BookingHistory again
                       .ThenInclude(b => b.Boarding)  // Then include the Boarding related to each Booking
                .Include(x => x.Booking)  // Include the Booking related to each BookingHistory again
                       .ThenInclude(b => b.Dropping)
                          .Include(x => x.Booking)  // Include the Booking related to each BookingHistory again
                       .ThenInclude(b => b.Dept)
                 .Where(x=>x.BusNumber==bus.BusNumber && x.BusName==bus.BusName).ToList();
            return bookingHistories;
        }

        public List<BookingHistory> GetAllBookingsByUserId(int userId)
        {
            User user = _context.Users.FirstOrDefault(x => x.UserId == userId);
            var bookingHistories = _context.BookingHistories
    .Include(x => x.Booking)  
        .ThenInclude(b => b.Bus)  // Then include the Bus related to each Booking
    .Include(x => x.Booking)  // Include the Booking related to each BookingHistory again
        .ThenInclude(b => b.Boarding)  // Then include the Boarding related to each Booking
    .Include(x => x.Booking)  // Include the Booking related to each BookingHistory again
        .ThenInclude(b => b.Dropping)
        .Include(x => x.Booking)  // Include the Booking related to each BookingHistory again
        .ThenInclude(b => b.Dept)// Then include the Dropping related to each Booking
    .Where(x => x.UserName == user.Email)  // Filter by UserName
    .ToList();

            return bookingHistories;
        }

        public List<BookingHistory> GetCancelledBookingsByUserId(int userId)
        {
            User user = _context.Users.FirstOrDefault(x => x.UserId == userId);
            var bookingHistories = _context.BookingHistories
    .Include(x => x.Booking)
        .ThenInclude(b => b.Bus)  // Then include the Bus related to each Booking
    .Include(x => x.Booking)  // Include the Booking related to each BookingHistory again
        .ThenInclude(b => b.Boarding)  // Then include the Boarding related to each Booking
    .Include(x => x.Booking)  // Include the Booking related to each BookingHistory again
        .ThenInclude(b => b.Dropping)
        .Include(x => x.Booking)  // Include the Booking related to each BookingHistory again
        .ThenInclude(b => b.Dept)
                .Where(x => x.UserName == user.Email&& x.IsCancelled==true).ToList();
            return bookingHistories;
        }

        public void PostBookingHistory(BookingHistory bookingHistory)
        {
            _context.BookingHistories.Add(bookingHistory);
        }
    }
}
