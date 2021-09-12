using API.Dtos;
using AutoMapper;
using Core.Entities;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Room, RoomDto>().ReverseMap();
            CreateMap<Reservation, ReservationDto>().ReverseMap();
        }
    }
}
