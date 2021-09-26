using API.Dtos;
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;
        private readonly IGenericRepository<User> _repository;

        public AuthController(IMapper mapper, IAuthService authService, IGenericRepository<User> repository, IEmailService emailService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login(LoginDto login)
        {
            if (login == null) return BadRequest(new ApiResponse(400));
            var user = await _repository.FindOneAsync(x => x.Email == login.Email);
            if (user == null) return Unauthorized(new ApiResponse(401, $"Invalid email or password"));

            var userMap = _mapper.Map<User>(login);
            if (!_authService.VerfyPasswordHash(login.Password, user.PasswordHash, user.PasswordSalt))
                return Unauthorized(new ApiResponse(401, $"Invalid email or password"));
            var token = _authService.CreateToken(user);
            return Ok(token);

        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(RegisterDto register)
        {
            byte[] passwordHash, passwordSalt;
            if (register == null) return BadRequest(new ApiResponse(400));
            if (await _repository.FindOneAsync(x => x.Email == register.Email) != null)
                return BadRequest(new ApiResponse(400, $"Email address is in use"));

            if (await _repository.FindOneAsync(x => x.PhoneNumber == register.PhoneNumber) != null)
                return BadRequest(new ApiResponse(400, $"Phone number already exists"));

            var userToCreate = _mapper.Map<User>(register);
            _authService.CreatePasswordHash(register.Password, out passwordHash, out passwordSalt);
            userToCreate.PasswordHash = passwordHash;
            userToCreate.PasswordSalt = passwordSalt;
            userToCreate.ClientState = ClientState.Registered;
            await _repository.InsertOneAsync(userToCreate);
            return StatusCode(201);

            //This code block is meant for email service, 
            //var isSend = _emailService.SendEmail(register.FullName, register.Email, EmailType.Register);
            //if (isSend)
            //{
            //    await _repository.InsertOneAsync(userToCreate);
            //    return StatusCode(201);
            //}
            //return BadRequest(new ApiResponse(400, $"An error occured while trying to create account, Kindly input a valid email address."));
        }
    }
}