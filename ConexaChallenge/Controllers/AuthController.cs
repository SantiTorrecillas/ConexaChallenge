using ConexaChallenge.Dtos;
using ConexaChallenge.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ConexaChallenge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IConfiguration configuration) : ControllerBase
    {
        public static User user = new();

        [HttpPost("Register")]
        public ActionResult<User> Register(UserDto request)
        {
            string hashedPassword = new PasswordHasher<User>()
                .HashPassword(user, request.Password);

            user.UserName = request.UserName;
            user.PasswordHash = hashedPassword;

            return Ok(user);
        }

        [HttpPost("Login")]
        public ActionResult<string> Login(UserDto request)
        {
            if(user.UserName != request.UserName)
            {
                return BadRequest("User Not Found.");
            }

            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
            {
                return BadRequest("Wrong Password.");
            }

            string token = CreateToken(user);
            
            return Ok(token);
        }

        private string CreateToken (User user)
        {
            List<Claim> claims = [ new(ClaimTypes.Name, user.UserName) ];

            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!));

            SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha512);

            JwtSecurityToken tokenDescriptor = new JwtSecurityToken(
                    issuer: configuration.GetValue<string>("AppSettings:Issuer"),
                    audience: configuration.GetValue<string>("AppSettings:Audience"),
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(1),
                    signingCredentials: creds
                    );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

    }
}
