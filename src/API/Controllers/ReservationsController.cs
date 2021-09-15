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
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class ReservationsController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Reservation> _repository;
        private readonly IGenericRepository<RoomOccupied> _roomOccupiedRepository;

        public ReservationsController(IGenericRepository<Reservation> repository, IMapper mapper,
            IGenericRepository<RoomOccupied> roomOccupiedRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _roomOccupiedRepository = roomOccupiedRepository ?? throw new ArgumentNullException(nameof(roomOccupiedRepository));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<ReservationDto>> GetReservations()
        {
            var rooms = _repository.AsQueryable();
            if(Role == UserRole.Customer.ToString())
            {
                var reservations = rooms.Where(x => x.Email == Email).AsEnumerable();
                return Ok(_mapper.Map<IEnumerable<ReservationDto>>(reservations));
            }
            return Ok(_mapper.Map<IEnumerable<ReservationDto>>(rooms));
        }

        [HttpGet("{reservationId:length(24)}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetReservation(string reservationId)
        {
            var reservation = await _repository.FindByIdAsync(reservationId);
            if (reservation == null) return NotFound(new ApiResponse(404));
            return Ok(_mapper.Map<ReservationDto>(reservation));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateReservation(CreateReservationDto createDto)
        {
            if (createDto == null) return BadRequest(new ApiResponse(400));
            var occupied = await _roomOccupiedRepository.FindOneAsync(x => x.RoomNumber == createDto.RoomNumber);
            if (occupied != null) return BadRequest(new ApiResponse(404, $"Room {createDto.RoomNumber} is currently occupied"));

            var reservation = _mapper.Map<Reservation>(createDto);
            reservation.FullName = FullName;
            reservation.Email = Email;
            await _repository.InsertOneAsync(reservation);
            return StatusCode(201);
        }

        [HttpPut("cancelreservations/{roomNo}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CancelReservation(int roomNo)
        {
            var cancel = await _repository.FindOneAsync(x => x.RoomNumber == roomNo && x.Email == Email);
            if (cancel == null) return BadRequest(new ApiResponse(404));

            var reservation = _mapper.Map<Reservation>(cancel);
            reservation.IsCancelled = true;
            await _repository.ReplaceOneAsync(reservation);
            return StatusCode(204);
        }

    }
}
