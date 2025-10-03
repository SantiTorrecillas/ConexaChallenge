using ConexaChallenge.Common;
using ConexaChallenge.Data;
using ConexaChallenge.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ConexaChallenge.Seed
{
    public static class AppDbSeeder
    {
        public static async Task SeedRootUserAsync(AppDbContext dbContext, IConfiguration configuration)
        {
            string rootUserName = configuration.GetValue<string>("AppSettings:RootUserName")!;
            string rootPassword = configuration.GetValue<string>("AppSettings:RootPassword")!;

            if (await dbContext.Users.AnyAsync(u => u.UserName == rootUserName))
            {
                return; 
            }

            var user = new User
            {
                UserName = rootUserName,
                Role = nameof(UserRole.Admin)
            };

            user.PasswordHash = new PasswordHasher<User>().HashPassword(user, rootPassword);
            dbContext.Users.Add(user);

            await dbContext.SaveChangesAsync();
        }
    }
}
