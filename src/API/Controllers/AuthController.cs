using API.Dtos;
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class AuthController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IAuthService _service;
        private readonly IGenericRepository<User> _repository;

        public AuthController(IMapper mapper, IAuthService service, IGenericRepository<User> repository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto login)
        {
            return Ok();
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto register)
        {
            if (register == null) return BadRequest(new ApiResponse(400));
            var users = await _repository.FindOneAsync(x => x.Email == register.Email);
            return BadRequest(new ApiResponse(404, $"Email address is in use"));

            return Ok();
        }

    }
}
