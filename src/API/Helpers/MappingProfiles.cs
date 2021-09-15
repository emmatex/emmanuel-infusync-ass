using API.Dtos;
using AutoMapper;
using Core.Entities;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<LoginDto, User>().ReverseMap();
            CreateMap<RegisterDto, User>().ReverseMap();
            CreateMap<Room, RoomDto>().ReverseMap();
            CreateMap<Room, FreeRoomDto>().ReverseMap();
            CreateMap<CreateRoomDto, Room>().ReverseMap();
            CreateMap<UpdateRoomDto, Room>().ReverseMap();
            CreateMap<Reservation, ReservationDto>().ReverseMap();
            CreateMap<CreateReservationDto, Reservation>().ReverseMap();
            CreateMap<RoomOccupied, RoomOccupiedDto>().ReverseMap();
            CreateMap<CreateRoomOccupiedDto, RoomOccupied>().ReverseMap();
            CreateMap<UpdateRoomOccupiedDto, RoomOccupied>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<RoomOccupied, UserDto>().ReverseMap();
        }
    }
}
