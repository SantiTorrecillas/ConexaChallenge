using ConexaChallenge.Data;
using ConexaChallenge.Dtos;
using ConexaChallenge.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ConexaChallenge.Services
{
    public class AuthService(AppDbContext dbContext, IConfiguration configuration) : IAuthService
    {

        public async Task<User?> RegisterAsync(UserRequest request)
        {
            if (await dbContext.Users.AnyAsync(x => x.UserName == request.UserName))
            {
                return null;
            }

            User user = new();
            string hashedPassword = new PasswordHasher<User>().HashPassword(user, request.Password);

            user.UserName = request.UserName;
            user.PasswordHash = hashedPassword;

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            return user;
        }

        public async Task<string?> LoginAsync(UserRequest request)
        {
            User? user = await dbContext.Users.FirstOrDefaultAsync(x => x.UserName == request.UserName);
            if (user is null)
            {
                return null;
            }

            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
            {
                return null;
            }

            return CreateToken(user);
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = [
                new(ClaimTypes.Name, user.UserName),
                new(ClaimTypes.NameIdentifier, user.UserName),
                new(ClaimTypes.Role, user.Role)
            ];

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
