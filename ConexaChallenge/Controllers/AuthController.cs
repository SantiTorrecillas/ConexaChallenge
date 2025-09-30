using ConexaChallenge.Dtos;
using ConexaChallenge.Entities;
using ConexaChallenge.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConexaChallenge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService service) : ControllerBase
    {

        [HttpPost("Register")]
        public async Task<ActionResult<User>> Register(UserRequest request)
        {
            User? user = await service.RegisterAsync(request);

            if (user is null)
            {
                return BadRequest("UserName already exists");
            }

            return Ok(user);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login(UserRequest request)
        {
            string? token = await service.LoginAsync(request);

            if (token is null)
            {
                return BadRequest("Invalid username or password");
            }

            return Ok(token);

        }
    }
}
