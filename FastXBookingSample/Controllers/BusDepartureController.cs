using AutoMapper;
using FastXBookingSample.DTO;
using FastXBookingSample.Exceptions;
using FastXBookingSample.Interface;
using FastXBookingSample.Models;
using FastXBookingSample.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FastXBookingSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusDepartureController : Controller
    {
        private readonly IBusDepartureRepository _busDepartureRepository;
        private readonly IMapper _mapper;
        private readonly BookingContext _context;
        private readonly IBusSeatRepository _busSeatRepository;
        public BusDepartureController(IBusDepartureRepository busDepartureRepository,IMapper mapper, IBusSeatRepository seatRepository, BookingContext context)
        {
            _busDepartureRepository = busDepartureRepository;
            _mapper = mapper;
            _busSeatRepository = seatRepository;
            _context = context;
        }

        [HttpPost]
        [Authorize(Roles = "Bus Operator")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Bus>> PostBusDeparture(BusDepartureDTO busDepartureDto)
        {
            Bus bus = _context.Buses.FirstOrDefault(bus => bus.BusId == busDepartureDto.BusId);
            BusDeparture busDeparture = _busDepartureRepository.AddDepartureDate(_mapper.Map<BusDeparture>(busDepartureDto));
            _busSeatRepository.AddSeatByBusId(bus.BusId, bus.NoOfSeats, busDeparture.Id);

            return Ok(busDeparture);
        }
    }
}
