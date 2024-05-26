using FastXBookingSample.Models;
using FastXBookingSample.Exceptions;
using System;
using FastXBookingSample.Interface;
using Microsoft.EntityFrameworkCore;

namespace FastXBookingSample.Repository
{
    public class BookingRepository : IBookingRepository
    {
        private readonly BookingContext _context;
        private readonly IBusRepository _busRepository;
        private readonly IUserRepository _userRepository;
        public BookingRepository(BookingContext context, IBusRepository busRepository, IUserRepository userRepository)
        {
            _context = context;
            _busRepository = busRepository;
            _userRepository = userRepository;
        }
        public string DeleteBooking(int id)
        {
            if (!IsBookingExists(id))
                throw new BookingNotFoundException();
            var booking = _context.Bookings.FirstOrDefault(x=>x.BookingId == id);
            _context.Bookings.Remove(booking);
            return _context.SaveChanges()>0?"Deleted Successfully":"Deletion Failed";
        }

        public List<Booking> GetAllBookingsByBusId(int busId)
        {
            if(!_busRepository.BusExists(busId))
                throw new BusNotFoundException();
            return _context.Bookings.Where(x=>busId== x.BusId).ToList();
            
        }

        public List<Booking> GetAllBookingsByUserId(int userId)
        {
            if (!_userRepository.IsUserExists(userId))
                throw new UserNotFoundException();
            return _context.Bookings
        .Include(b => b.Seats)  // Include related seats
        .Include(b => b.Bus)
        .Include(b => b.Boarding)
        .Include(b => b.Dropping)
        .Include(b => b.Dept)
        .Where(b => b.UserId == userId)
        .ToList();

        }

        public bool IsBookingExists(int id)
        {
            return _context.Bookings.Any(x=>x.BookingId == id);
        }

        public Booking PostBooking(Booking booking,DateTime deptDate)
        {
            if (!IsUser(Convert.ToInt32(booking.UserId)))
                throw new UserNotFoundException();
            Booking book = _context.Bookings.FirstOrDefault(x=>x.UserId == booking.UserId &&x.BusId == booking.BusId && x.BoardingId==booking.BoardingId && x.DroppingId==booking.DroppingId);
            if(book != null)
               return book;
            booking.DeptId = (_context.BusDepartures.FirstOrDefault(x => x.BusId == booking.BusId && x.DepartureDate == deptDate)).Id;
            _context.Bookings.Add(booking);
            _context.SaveChanges();
            return booking;
        }

        public bool IsUser(int userId)
        {
            return _context.Users.FirstOrDefault(x=>x.UserId == userId).Role=="User"?true:false;
        }
    }
}
