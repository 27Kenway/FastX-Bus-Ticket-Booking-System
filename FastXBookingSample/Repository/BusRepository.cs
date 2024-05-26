using FastXBookingSample.Models;
using FastXBookingSample.Exceptions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System;
using FastXBookingSample.Interface;

namespace FastXBookingSample.Repository
{
    public class BusRepository:IBusRepository
    {
        private readonly BookingContext _context;
        public BusRepository(BookingContext context)
        {
            _context = context;
        }

        public Bus CreateBus(Bus bus)
        {
            _context.Buses.Add(bus);
            int s=_context.SaveChanges();
            if (s > 0)
                return bus;
            else return new Bus();
        }

        public string DeleteBus(int id)
        {
            if(!BusExists(id))
                throw new BusNotFoundException();
            if (_context.Buses == null)
                throw new BusNotFoundException();
            Bus bus = _context.Buses.FirstOrDefault(x=>x.BusId == id);
            if (bus == null)
                throw new BusNotFoundException();
            _context.Buses.Remove(bus);
            int s = _context.SaveChanges();
            if (s > 0) 
                return "Bus Successfully Deleted";
            return "Bus Not Deleted";
            
        }

        public List<Bus> GetAll()
        {
            List<Bus> list = _context.Buses.ToList();
            return list;
        }

        public List<Bus> GetBusByDetails(string origin, string destination, DateOnly date)
        {
            DateTime startDate = date.ToDateTime(TimeOnly.Parse("12:00 PM"));
            DateTime endDate = startDate.AddDays(1);
            List<int> busIds = _context.BusDepartures
                           .Where(x => x.DepartureDate >= startDate && x.DepartureDate < endDate)
                           .Select(x => x.BusId.Value)
                           .ToList();
            var buses = _context.Buses
                       .Where(x => busIds.Contains(x.BusId) &&
                                   x.Origin == origin &&
                                   x.Destination == destination)
                       .Select(b => new Bus
                       {
                           BusId = b.BusId,
                           BusName = b.BusName,
                           BusType = b.BusType,
                           Origin = b.Origin,
                           Destination = b.Destination,
                           NoOfSeats = b.NoOfSeats,
                           StartTime = b.StartTime,
                           EndTime = b.EndTime,
                           Fare = b.Fare,
                           BusOperator = b.BusOperator,
                           BusAmenities = b.BusAmenities.Select(ba => new BusAmenity
                           {
                               AmenityId = ba.AmenityId,
                               Amenity = ba.Amenity
                           }).ToList(),
                           BusSeats = b.BusSeats.Where(bs => bs.BusId == b.BusId && bs.DepartureId==(_context.BusDepartures.FirstOrDefault(x=>x.BusId==b.BusId && x.DepartureDate >= startDate && x.DepartureDate < endDate)).Id).ToList() // Filter seats by BusId
                       })
                       .ToList();
            return buses;
        }

        public Bus GetBusById(int id)
        {
            if (!BusExists(id))
                throw new BusNotFoundException();
            Bus bus = _context.Buses.FirstOrDefault(x => x.BusId == id);
            return bus;
        }


        public string UpdateBus(int id, Bus bus)
        {
            if (!BusExists(id))
                throw new BusNotFoundException();
            _context.Entry(bus).State = EntityState.Modified;
            int s = _context.SaveChanges();
            if (s > 0)
                return "Bus Details Updated";
            return "Bus Details Not Updated";
       
        }


        public bool BusExists(int id)
        {
            return (_context.Buses?.Any(e => e.BusId == id)).GetValueOrDefault();
            
        }

        public bool RoleExists(int id)
        {
            return (_context.Users.Any(e => e.UserId == id && e.Role == "Bus Operator"));
        }



        public string AddBusAmenity(int busid, int amenityid)
        {
            if (!BusExists(busid))
                throw new BusNotFoundException();
            var bus = _context.Buses.FirstOrDefault(e => e.BusId == busid);
            var amenity = _context.Amenities.FirstOrDefault(x => x.AmenityId == amenityid);
            BusAmenity busAmenity = new BusAmenity()
            {
                Bus = bus,
                AmenityId = amenityid,
            };
            _context.BusAmenities.Add(busAmenity);
            return _context.SaveChanges()>0?"Added Successfully":"Addition Failed";
         
        }

        public string PatchBus(int id, JsonPatchDocument<Bus> patchBus)
        {
            if(!BusExists(id))
                throw new BusNotFoundException();
            var bus = _context.Buses.FirstOrDefault(x => x.BusId == id);
            patchBus.ApplyTo(bus);
            _context.Update(bus);

            return _context.SaveChanges() > 0 ? "Updated Sucessfully" : "Updation Failed ";
            
        }

        public List<Bus> GetBusByBusOperator(int busOperatorId)
        {
            return _context.Buses
                       .Include(b => b.BusAmenities)
                           //.ThenInclude(ba => ba.Amenity)
                           .Include(b=> b.BusDepartures)
                       .Where(b => b.BusOperator == busOperatorId)
                       .ToList();
        }
    }
}
