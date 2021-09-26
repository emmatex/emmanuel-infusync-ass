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
        private readonly IEmailService _emailService;
        private readonly IGenericRepository<Room> _repository;

        public ReservationsController(IGenericRepository<Room> repository, IMapper mapper, IEmailService emailService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<ReservationDto>> GetReservations()
        {
            var rooms = _repository.AsQueryable().Where(x => x.RoomState == RoomState.Reserved);
            if (Role == UserRole.User.ToString())
            {
                var reservations = rooms.Where(x => x.Email == Email).AsEnumerable();
                return Ok(_mapper.Map<IEnumerable<ReservationDto>>(reservations));
            }
            return Ok(_mapper.Map<IEnumerable<ReservationDto>>(rooms));
        }

        [HttpGet("availablerooms")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<ReservationDto>> GetAvailableRooms()
        {
            var rooms = _repository.AsQueryable().Where(x => x.RoomState != RoomState.Reserved && x.RoomState == RoomState.Free);
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
            var reservation = await _repository.FindOneAsync(x => x.RoomNumber == createDto.RoomNumber);
            if (reservation == null) return BadRequest(new ApiResponse(404, $"Room {createDto.RoomNumber} does not exists"));
            if (reservation.ClientState == ClientState.BookedRoom) return BadRequest(new ApiResponse(404, $"Room {createDto.RoomNumber} has been booked."));


            _mapper.Map(createDto, reservation);
            reservation.FullName = FullName;
            reservation.Email = Email;
            reservation.RoomState = RoomState.Reserved;
            reservation.ClientState = ClientState.BookedRoom;

            // var isSent = _emailService.SendEmail(FullName, Email, EmailType.Reservation);
            await _repository.ReplaceOneAsync(reservation);
            return StatusCode(201);
        }

        [HttpPut("cancelreservations/{roomNo}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CancelReservation(int roomNo)
        {
            var cancel = await _repository.FindOneAsync(x => x.RoomNumber == roomNo && x.Email == Email && x.RoomState == RoomState.Reserved);
            if (cancel == null) return BadRequest(new ApiResponse(404));

            cancel.RoomState = RoomState.Free;
            await _repository.ReplaceOneAsync(cancel);
            return StatusCode(204);
        }

    }
}
