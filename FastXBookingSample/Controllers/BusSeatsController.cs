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
    public class BusSeatsController : ControllerBase
    {
        private readonly IBusSeatRepository _busSeatRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<BusSeatsController> _logger;

        public BusSeatsController(IBusSeatRepository busSeatRepository,IMapper mapper,ILogger<BusSeatsController> logger)
        {
            _busSeatRepository = busSeatRepository;
            _mapper = mapper;
            _logger = logger;
        }

       

        // GET: api/BusSeats/5
        [HttpGet("{busid}")]
        [Authorize(Roles = "Bus Operator,Admin,User")]
        [ProducesResponseType(200, Type = typeof(List<BusSeatDto>))]
        [ProducesResponseType(400)]
        public async Task<ActionResult<List<BusSeatDto>>> GetBusSeat(int busid,DateTime deptId)
        {
            try
            {
                return Ok(_mapper.Map<List<BusSeatDto>>(_busSeatRepository.GetSeatsByBusId(busid,deptId)));
            }
            catch(BusNotFoundException ex)
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
