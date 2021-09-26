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
            CreateMap<User, UserDto>().ReverseMap();

            CreateMap<Room, RoomDto>().ReverseMap();
            CreateMap<Room, FreeRoomDto>().ReverseMap();
            CreateMap<CreateRoomDto, Room>().ReverseMap();
            CreateMap<UpdateRoomDto, Room>().ReverseMap();

            CreateMap<Room, ReservationDto>().ReverseMap();
            CreateMap<CreateReservationDto, Room>().ReverseMap();

            CreateMap<Room, RoomOccupiedDto>().ReverseMap();
            CreateMap<CreateRoomOccupiedDto, Room>().ReverseMap();
            CreateMap<UpdateRoomOccupiedDto, Room>().ReverseMap();
            CreateMap<Room, UserDto>().ReverseMap();
        }
    }
}
