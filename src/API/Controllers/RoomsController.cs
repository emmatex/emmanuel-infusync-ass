﻿using API.Dtos;
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
    public class RoomsController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Room> _repository;

        public RoomsController(IGenericRepository<Room> repository, IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<RoomDto>>> GetRooms()
        {
            var rooms = _repository.AsQueryable();
            return Ok(_mapper.Map<IEnumerable<RoomDto>>(rooms));
        }

        [HttpGet("{roomId:length(24)}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRoom(string roomId)
        {
            var room = await _repository.FindByIdAsync(roomId);
            if (room == null) return NotFound(new ApiResponse(404));
            return Ok(_mapper.Map<RoomDto>(room));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateRoom(CreateRoomDto createDto)
        {
            if (createDto == null) return BadRequest(new ApiResponse(400));
            if(Role != UserRole.Admin.ToString()) return StatusCode(403, $"Access denied.");

            var room = await _repository.FindOneAsync(x => x.RoomNumber == createDto.RoomNumber);
            if (room != null) return BadRequest(new ApiResponse(404, $"Room no {createDto.RoomNumber} already exists"));

            var roomToAdd = _mapper.Map<Room>(createDto);
            await _repository.InsertOneAsync(roomToAdd);
            return StatusCode(201);
        }

        [HttpPut("{roomId:length(24)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateRoom(string roomId, UpdateRoomDto updateDto)
        {
            if (updateDto == null) return BadRequest(new ApiResponse(400));
            if (Role != UserRole.Admin.ToString()) return StatusCode(403, $"Access denied.");

            var room = await _repository.FindByIdAsync(roomId);
            if (room == null) return NotFound(new ApiResponse(404));

            room.UpdatedDate = DateTime.UtcNow;
            _mapper.Map(updateDto, room);
            await _repository.ReplaceOneAsync(room);
            return NoContent();
        }

        [HttpDelete("{roomId:length(24)}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteRoom(string roomId)
        {
            if (String.IsNullOrWhiteSpace(roomId)) return BadRequest(new ApiResponse(400));
            if (Role != UserRole.Admin.ToString()) return StatusCode(403, $"Access denied.");

            var room = await _repository.FindByIdAsync(roomId);
            if (room == null) return NotFound(new ApiResponse(404));

            await _repository.DeleteByIdAsync(roomId);
            return NoContent();
        }

    }
}
