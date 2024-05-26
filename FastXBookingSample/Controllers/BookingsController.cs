using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FastXBookingSample.Models;
using AutoMapper;
using FastXBookingSample.DTO;
using FastXBookingSample.Exceptions;
using Microsoft.AspNetCore.Authorization;
using FastXBookingSample.Interface;


namespace FastXBookingSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<BookingsController> _logger;

        public BookingsController(BookingContext context, IBookingRepository bookingRepository,IMapper mapper, ILogger<BookingsController> logger)
        {
            _bookingRepository = bookingRepository;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/Bookings
        [HttpGet("getBookingsByBusId")]
        [Authorize(Roles = "Bus Operator,User")]
        [ProducesResponseType(200, Type = typeof(List<BookingDto>))]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetBookingsByBusId(int busid)
        {
            try
            {
                return Ok(_mapper.Map<List<BookingDto>>(_bookingRepository.GetAllBookingsByBusId(busid)));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
        }

        [HttpGet("getBookingsByUserId/{userId}")]
        [Authorize(Roles = "Bus Operator,User")]
        [ProducesResponseType(200, Type = typeof(List<BookingDto>))]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetBookingsByUserId(int userId)
        {
            try
            {
                return Ok((_bookingRepository.GetAllBookingsByUserId(userId)));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
        }



        // POST: api/Bookings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "User")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<BookingDto>> PostBooking(BookingDto bookingdto,DateTime deptDate)
        {
            return Ok(_bookingRepository.PostBooking(_mapper.Map<Booking>(bookingdto),deptDate));
        }

        // DELETE: api/Bookings/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            try
            {
                return Ok(_bookingRepository.DeleteBooking(id));
            }
            catch(BookingNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
        }

    }
}
