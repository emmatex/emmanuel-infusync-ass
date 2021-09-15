using API.Dtos;
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class OccupiedRoomsController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Room> _roomRepository;
        private readonly IGenericRepository<RoomOccupied> _repository;

        public OccupiedRoomsController(IGenericRepository<RoomOccupied> repository, IMapper mapper, IGenericRepository<Room> roomRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _roomRepository = roomRepository ?? throw new ArgumentNullException(nameof(roomRepository));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<RoomOccupiedDto>>> GetOccupiedRooms()
        {
            var rooms = _repository.AsQueryable();
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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateOccupiedRoom(CreateRoomOccupiedDto createDto)
        {
            if (createDto == null) return BadRequest(new ApiResponse(400));
            if (Role != UserRole.Admin.ToString()) return StatusCode(403, $"Access denied.");

            var room = await _roomRepository.FindOneAsync(x => x.RoomNumber == createDto.RoomNumber);
            if (room == null) return BadRequest(new ApiResponse(404, $"Room no {createDto.RoomNumber} does not exists"));
            if (room.CheckIns) return BadRequest(new ApiResponse(404, $"Room no {createDto.RoomNumber} is occupied"));

            var itemToAdd = _mapper.Map<RoomOccupied>(createDto);
            itemToAdd.Email = Email;
            itemToAdd.FullName = FullName;
            await _repository.InsertOneAsync(itemToAdd);
            // update room checkins status
            room.CheckIns = true;
            await _roomRepository.ReplaceOneAsync(room);
            return StatusCode(201);
        }

        [HttpPut("{roomId:length(24)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateOccupiedRoom(string roomId, UpdateRoomOccupiedDto updateDto)
        {
            if (updateDto == null) return BadRequest(new ApiResponse(400));
            if (Role != UserRole.Admin.ToString()) return StatusCode(403, $"Access denied.");

            var room = await _repository.FindByIdAsync(roomId);
            if (room == null) return NotFound(new ApiResponse(404));

            _mapper.Map(updateDto, room);
            await _repository.ReplaceOneAsync(room);
            if(!updateDto.IsOccupancy)
            {
                // update room checkins status
                var rm = await _roomRepository.FindOneAsync(x => x.RoomNumber == updateDto.RoomNumber);
                rm.CheckIns = false;
                await _roomRepository.ReplaceOneAsync(rm);
            }
            return NoContent();
        }

        [HttpDelete("{roomId:length(24)}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteOccupiedRoom(string roomId)
        {
            if (string.IsNullOrWhiteSpace(roomId)) return BadRequest(new ApiResponse(400));
            if (Role != UserRole.Admin.ToString()) return StatusCode(403, $"Access denied.");

            var room = await _repository.FindByIdAsync(roomId);
            if (room == null) return NotFound(new ApiResponse(404));

            await _repository.DeleteByIdAsync(roomId);
            return NoContent();
        }

    }
}
