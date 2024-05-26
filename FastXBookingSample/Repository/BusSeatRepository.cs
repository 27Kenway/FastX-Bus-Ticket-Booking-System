using FastXBookingSample.Exceptions;
using FastXBookingSample.Interface;
using FastXBookingSample.Models;

namespace FastXBookingSample.Repository
{
    public class BusSeatRepository : IBusSeatRepository
    {
        private readonly BookingContext _context;
        private readonly IBusRepository _busRepository;

        public BusSeatRepository(BookingContext context, IBusRepository busRepository)
        {
            _context = context;
            _busRepository = busRepository;
        }

        public void AddSeatByBusId(int busid,int seats, int deptId)
        {
            for (int i = 1; i <= seats; i++) {
                _context.BusSeats.Add(new BusSeat()
                {
                    BusId = busid,
                    SeatNo = i,
                    DepartureId=deptId,
                    IsBooked = false,
                });
            }
            _context.SaveChanges();
        }

        public void DeleteSeatsByBusId(int busid)
        {
            var seats = _context.BusSeats.Where(x => x.BusId == busid).ToList();
            foreach (var seat in seats)
                _context.BusSeats.Remove(seat);
            _context.SaveChanges() ;
        }

        public List<BusSeat> GetSeatsByBusId(int busid,DateTime deptId)
        {
            if (!_busRepository.BusExists(busid))
                throw new BusNotFoundException();
            BusDeparture busDeparture = _context.BusDepartures.FirstOrDefault(z=>z.BusId == busid&& z.DepartureDate==deptId);
            return _context.BusSeats.Where(x=>x.BusId == busid&& x.DepartureId==busDeparture.Id).ToList();
        }
    }
}
