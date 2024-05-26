using FastXBookingSample.Interface;
using FastXBookingSample.Models;

namespace FastXBookingSample.Repository
{
    public class BusDepartureRepository:IBusDepartureRepository
    {
        private readonly BookingContext _context;
        public BusDepartureRepository(BookingContext context)
        {
            _context = context;
        }

        public BusDeparture AddDepartureDate(BusDeparture busDeparture)
        {
            _context.Add(busDeparture);
            return _context.SaveChanges() > 0 ? busDeparture : new BusDeparture();
        }
    }
}
