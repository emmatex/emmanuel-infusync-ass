using API.Dtos;
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
    public class DasboardController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<Room> _roomRepository;
        private readonly IGenericRepository<Reservation> _reservationRepository;
        private readonly IGenericRepository<RoomOccupied> _roomOccupiedRepository;

        public DasboardController(IMapper mapper, IGenericRepository<Room> roomRepository, IGenericRepository<Reservation> reservationRepository,
            IGenericRepository<RoomOccupied> roomOccupiedRepository, IGenericRepository<User> userRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _roomRepository = roomRepository ?? throw new ArgumentNullException(nameof(roomRepository));
            _reservationRepository = reservationRepository ?? throw new ArgumentNullException(nameof(reservationRepository));
            _roomOccupiedRepository = roomOccupiedRepository ?? throw new ArgumentNullException(nameof(roomOccupiedRepository));
        }

        [HttpGet("totalrooms")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetTotalRooms()
        {
            if (Role != UserRole.Admin.ToString()) return StatusCode(403, $"Access denied.");
            var count = _roomRepository.AsQueryable().Count();
            return Ok(count);
        }

        [HttpGet("checkins")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetCheckIns()
        {
            if (Role != UserRole.Admin.ToString()) return StatusCode(403, $"Access denied.");
            var count = _roomRepository.FilterBy(x => x.CheckIns == true).Count();
            return Ok(count);
        }

        [HttpGet("checkouts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetCheckOuts()
        {
            if (Role != UserRole.Admin.ToString()) return StatusCode(403, $"Access denied.");
            var count = _roomRepository.FilterBy(x => x.CheckIns == false).Count();
            return Ok(count);
        }

        [HttpGet("freerooms")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<FreeRoomDto>>> GetFreeRooms()
        {
            if (Role != UserRole.Admin.ToString()) return StatusCode(403, $"Access denied.");
            var rooms = _roomRepository.FilterBy(x => x.CheckIns == false);
            var record = _mapper.Map<IEnumerable<FreeRoomDto>>(rooms);
            return Ok(new { rooms = record, total = record.Count() });
        }

        [HttpGet("occupancy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult GetTotalOccupancy()
        {
            if (Role != UserRole.Admin.ToString()) return StatusCode(403, $"Access denied.");
            var count = _roomOccupiedRepository.AsQueryable().Count();
            return Ok(count);
        }

        [HttpGet("revenue")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetRevenue()
        {
            if (Role != UserRole.Admin.ToString()) return StatusCode(403, $"Access denied.");
            var amount = _roomOccupiedRepository.FilterBy(x => x.IsOccupancy == true).Sum(s => s.Amount);
            return Ok(amount);
        }

        [HttpGet("customers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetCustomers()
        {
            if (Role != UserRole.Admin.ToString()) return StatusCode(403, $"Access denied.");
            var users = _userRepository.FilterBy(x => x.Role != UserRole.Admin);
            var record = _mapper.Map<IEnumerable<UserDto>>(users);
            return Ok(new { customers = record, total = users.Count() });
        }

        [HttpGet("roomobookbycustomer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBooks()
        {
            if (Role != UserRole.Admin.ToString()) return StatusCode(403, $"Access denied.");
            var users = _roomOccupiedRepository.FilterBy(x => x.IsOccupancy == true);
            var record = users.GroupBy(x => x.Email).Select(x => new
            {
                FullName = x.FirstOrDefault().FullName,
                Amount = x.Sum(a => a.Amount),
                Days = (x.FirstOrDefault().From - x.FirstOrDefault().To).TotalDays
            }).Distinct().ToList();
            return Ok(record);
        }

    }
}
