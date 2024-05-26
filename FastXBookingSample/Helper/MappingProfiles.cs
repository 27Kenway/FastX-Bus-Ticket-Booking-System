using AutoMapper;
using FastXBookingSample.DTO;
using FastXBookingSample.Models;

namespace FastXBookingSample.Helper
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<Bus, BusDto>();
            CreateMap<BusDto, Bus>();
            CreateMap<BoardingPoint, BoardingPointDto>();
            CreateMap<BoardingPointDto, BoardingPoint>();    
            CreateMap<DroppingPointDto, DroppingPoint>();    
            CreateMap<DroppingPoint, DroppingPointDto>();    
            CreateMap<Amenity, AmenityDto>();    
            CreateMap<AmenityDto, Amenity>();    
            CreateMap<Models.Route, RouteDto>();    
            CreateMap<RouteDto, Models.Route>();    
            CreateMap<UserDto, User>();    
            CreateMap<User, UserDto>();    
            CreateMap<BusSeat, BusSeatDto>();    
            CreateMap<BusSeatDto, BusSeat>();
            CreateMap<Booking, BookingDto>();
            CreateMap<BookingDto, Booking>();    
            CreateMap<Seat, SeatDto>();    
            CreateMap<SeatDto, Seat>();    
            CreateMap<BookingHistory, BookingHistoryDto>();  
            CreateMap<BusDepartureDTO, BusDeparture>();  
            
        }
    }
}
