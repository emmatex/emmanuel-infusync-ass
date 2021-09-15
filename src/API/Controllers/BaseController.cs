using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BaseController : ControllerBase
    {
        protected string Role => HttpContext.User.FindFirst(ClaimTypes.Role).Value;
        protected string Email => HttpContext.User.FindFirst(ClaimTypes.Email).Value;
        protected string FullName => HttpContext.User.FindFirst(ClaimTypes.GivenName).Value;
    }
}
