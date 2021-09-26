using API.Dtos;
using AutoMapper;
using Core.Common;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize(Roles = Access.Admin)]
    public class DasboardController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<Room> _roomRepository;

        public DasboardController(IMapper mapper, IGenericRepository<Room> roomRepository, IGenericRepository<User> userRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _roomRepository = roomRepository ?? throw new ArgumentNullException(nameof(roomRepository));
        }

        [HttpGet("totalrooms")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetTotalRooms()
        {
            return Ok(_roomRepository.AsQueryable().Count());
        }

        [HttpGet("checkins")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetCheckIn()
        {
            return Ok(_roomRepository.FilterBy(x => x.ClientState == ClientState.CheckIn).Count());
        }

        [HttpGet("checkouts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetCheckOut()
        {
            return Ok(_roomRepository.FilterBy(x => x.ClientState == ClientState.CheckOut).Count());
        }

        [HttpGet("freerooms")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<FreeRoomDto>>> GetFreeRoom()
        {
            var rooms = _roomRepository.FilterBy(x => x.RoomState == RoomState.Free);
            var record = _mapper.Map<IEnumerable<FreeRoomDto>>(rooms);
            return Ok(new { rooms = record, total = record.Count() });
        }

        [HttpGet("occupancy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult GetTotalOccupancy()
        {
            return Ok(_roomRepository.FilterBy(x => x.RoomState == RoomState.Occupied).Count());
        }

        [HttpGet("revenue")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetRevenue()
        {
            var amount = _roomRepository.FilterBy(x => x.RoomState == RoomState.Occupied).Sum(s => s.TotalAmount);
            return Ok(amount);
        }

        [HttpGet("customers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetCustomers()
        {
            var users = _userRepository.FilterBy(x => x.Role != UserRole.Admin.ToString());
            var record = _mapper.Map<IEnumerable<UserDto>>(users);
            return Ok(new { customers = record, total = users.Count() });
        }

        [HttpGet("bookedrooms")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetBookedRooms()
        {
            var rooms = _roomRepository.FilterBy(x => x.ClientState == ClientState.BookedRoom);
            var record = rooms.GroupBy(x => x.Email).Select(x => new
            {
                FullName = x.FirstOrDefault().FullName,
                Amount = x.FirstOrDefault().RoomAmount * (x.FirstOrDefault().To - x.FirstOrDefault().From).TotalDays,
                Days = (x.FirstOrDefault().To - x.FirstOrDefault().From).TotalDays
            }).Distinct().ToList();
            return Ok(record);
        }

        //[HttpGet("bookedrooms")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        //public async Task<IActionResult> GetBookedRooms()
        //{
        //    if (Role != UserRole.Admin.ToString()) return StatusCode(403, $"Access denied.");
        //    var users = _userRepository.FilterBy(x => x.ClientState == ClientState.BookedRoom);
        //    var dataToReturn = new ConcurrentBag<CustomerReportDto>();
        //    var tasks = users.Select(async item =>
        //    {
        //        var response = await CustomerBookingReport(item);
        //        dataToReturn.Add(response);
        //    });
        //    await Task.WhenAll(tasks);
        //    return Ok(dataToReturn);
        //}

        //private async Task<CustomerReportDto> CustomerBookingReport(User user)
        //{
        //    var rooms = _roomRepository.AsQueryable().Where(x => x.Email == user.Email);
        //    var data = rooms.GroupBy(x => x.Email).Select(x => new
        //    {
        //        FullName = x.FirstOrDefault().FullName,
        //        Days = (x.FirstOrDefault().From - x.FirstOrDefault().To).TotalDays,
        //        Amount = x.FirstOrDefault().TotalAmount
        //    }).Distinct();

        //    return _mapper.Map<CustomerReportDto>(data);
        //}

    }
}
