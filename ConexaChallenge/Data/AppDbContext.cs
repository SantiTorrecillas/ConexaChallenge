using ConexaChallenge.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConexaChallenge.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Movie> Movies { get; set; }
    }
}
