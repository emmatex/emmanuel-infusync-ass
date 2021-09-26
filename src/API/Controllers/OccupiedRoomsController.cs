using API.Dtos;
using API.Errors;
using AutoMapper;
using Core.Common;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize(Roles = Access.Admin)]
    public class OccupiedRoomsController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Room> _repository;

        public OccupiedRoomsController(IGenericRepository<Room> repository, IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<RoomOccupiedDto>>> GetOccupiedRooms()
        {
            var rooms = _repository.AsQueryable().Where(x => x.RoomState == RoomState.Occupied);
            return Ok(_mapper.Map<IEnumerable<RoomOccupiedDto>>(rooms));
        }

        [HttpGet("{roomId:length(24)}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOccupiedRoom(string roomId)
        {
            var room = await _repository.FindByIdAsync(roomId);
            if (room == null) return NotFound(new ApiResponse(404));
            return Ok(_mapper.Map<RoomOccupiedDto>(room));
        }

        [HttpPut("checkin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CheckIn(UpdateRoomOccupiedDto updateDto)
        {
            if (updateDto == null) return BadRequest(new ApiResponse(400));
            var room = await _repository.FindOneAsync(x => x.RoomNumber == updateDto.RoomNumber);
            if (room == null) return BadRequest(new ApiResponse(404, $"Room no {updateDto.RoomNumber} does not exists"));
            if (room.ClientState == ClientState.CheckIn) return BadRequest(new ApiResponse(404, $"Room no {updateDto.RoomNumber} is occupied"));
            if (room.ClientState == ClientState.BookedRoom) return BadRequest(new ApiResponse(404, $"Room no {updateDto.RoomNumber} has been booked"));

            room.RoomState = RoomState.Occupied;
            room.ClientState = ClientState.CheckIn;
            room.TotalAmount = room.RoomAmount * (updateDto.To - updateDto.From).TotalDays;

            _mapper.Map(updateDto, room);
            await _repository.ReplaceOneAsync(room);
            return NoContent();
        }

        [HttpPut("checkout")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CheckOut(UpdateRoomOccupiedDto updateDto)
        {
            if (updateDto == null) return BadRequest(new ApiResponse(400));
            var room = await _repository.FindOneAsync(x => x.RoomNumber == updateDto.RoomNumber);
            if (room == null) return BadRequest(new ApiResponse(404, $"Room no {updateDto.RoomNumber} does not exists"));

            room.RoomState = RoomState.Free;
            room.ClientState = ClientState.CheckOut;
            room.TotalAmount = 0;

            _mapper.Map(updateDto, room);
            await _repository.ReplaceOneAsync(room);
            return NoContent();
        }

    }
}
