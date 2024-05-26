using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FastXBookingSample.Models;
using FastXBookingSample.DTO;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using FastXBookingSample.Exceptions;
using Microsoft.AspNetCore.Authorization;
using FastXBookingSample.Interface;

namespace FastXBookingSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusesController : ControllerBase
    {
        private readonly IBusRepository _busrepository;
        private readonly IMapper _mapper;
        private readonly IBusSeatRepository _busSeatRepository;
        private readonly BookingContext _context;
        private readonly ILogger<BusesController> _logger;
        private readonly IBusDepartureRepository _busDepartureRepository;
        public BusesController(IBusRepository busrepository,IMapper mapper, IBusSeatRepository busSeatRepository,BookingContext context,ILogger<BusesController> logger,IBusDepartureRepository busDepartureRepository)
        {
            _busrepository = busrepository;
            _mapper = mapper;
            _busSeatRepository = busSeatRepository;
            _context = context;
            _logger = logger;
            _busDepartureRepository = busDepartureRepository;
        }

        // GET: api/Buses
        [HttpGet]
        [Authorize(Roles = "Bus Operator,Admin")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Bus>))]
        public async Task<ActionResult<IEnumerable<Bus>>> Getbuses()
        {
            try
            {
                var buses = _mapper.Map<List<BusDto>>(_busrepository.GetAll());
                if (buses == null)
                {
                    return NotFound();
                }
                return Ok(buses);
            }catch (NoBusAvailableException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
        }


        // GET: api/Buses/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Bus Operator,Admin")]
        [ProducesResponseType(200,Type = typeof(Bus))]
        public async Task<ActionResult<Bus>> GetBus(int id)
        {
            try
            {
                var bus = _mapper.Map<BusDto>(_busrepository.GetBusById(id));
                if (bus == null)
                { return NotFound(); }
                return Ok(bus);
            }
            catch(NoBusAvailableException ex) 
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

        // PUT: api/Buses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Bus Operator,Admin")]
        [ProducesResponseType(200,Type=typeof(String))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PutBus(int id, BusDto busdto)
        {
            try
            {
                if (id != busdto.BusId)
                {
                    return BadRequest();
                }
                if (!_busrepository.BusExists(id))
                    return NotFound();
                Bus existingBus = _busrepository.GetBusById(id);
                if (existingBus.NoOfSeats != busdto.NoOfSeats)
                {
                    _busSeatRepository.DeleteSeatsByBusId(busdto.BusId);
                    //_busSeatRepository.AddSeatByBusId(busdto.BusId, busdto.NoOfSeats);
                }
                _context.Entry(existingBus).State = EntityState.Detached;

                var bus = _mapper.Map<Bus>(busdto);



                return Ok(_busrepository.UpdateBus(id, bus));
            }
            catch (NoBusAvailableException ex)
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

        // POST: api/Buses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Bus Operator")]
        [ProducesResponseType(200 ,Type=typeof(string))]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Bus>> PostBus(BusDto busdto,DateTime departureDate)
        {
            try
            {
                if (busdto == null)
                    return BadRequest(ModelState);
                if (!_busrepository.RoleExists(busdto.BusOperator))
                    return BadRequest(ModelState);
                var bus = _mapper.Map<Bus>(busdto);
                Bus message = _busrepository.CreateBus(bus);
                
                return Ok(message);
            }
            catch (NoBusAvailableException ex)
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

        // DELETE: api/Buses/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Bus Operator,Admin")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteBus(int id)
        {
            try
            {
                if (!_busrepository.BusExists(id))
                    return NotFound();
                _busSeatRepository.DeleteSeatsByBusId(id);
                string message = _busrepository.DeleteBus(id);

                return Ok(message);
            }
            catch (NoBusAvailableException ex)
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

        [HttpGet("GetBusByDetails")]
        [Authorize(Roles = "Bus Operator,User,Admin")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Bus>))]
        public async Task<ActionResult<IEnumerable<Bus>>> GetBusByDetails([FromQuery] string origin, [FromQuery] string destination, [FromQuery] string date)
        {
            try
            {
                DateOnly dateOnly = DateOnly.Parse(date);
                var buses = (_busrepository.GetBusByDetails(origin, destination, dateOnly));
                if (buses == null)
                    return BadRequest(ModelState);
                return Ok(buses);
            }
            catch (NoBusAvailableException ex)
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


        [HttpPost("PostBusAmenities")]
        [Authorize(Roles = "Bus Operator")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400)]
        public async Task<ActionResult<BusAmenity>> PostBusAmenity(int busid,int amenityid)
        {
            try
            {
                return Ok(_busrepository.AddBusAmenity(busid, amenityid));

            }catch (NoBusAvailableException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        //Patch
        [HttpPatch("{id:int}")]
        [Authorize(Roles = "Bus Operator")]
        public IActionResult Patch(int id, [FromBody] JsonPatchDocument<Bus> patchBus)
        {
            try
            {
                return Ok(_busrepository.PatchBus(id, patchBus));

            }
            catch (NoBusAvailableException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("getBusByOperatorId")]
        [Authorize(Roles ="Bus Operator,Admin")]
        public IActionResult GetBusByBusOperatorId([FromQuery]int busOperatorId) {
            try
            {
                return Ok(_busrepository.GetBusByBusOperator(busOperatorId));

            }
            catch (NoBusAvailableException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
