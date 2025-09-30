using ConexaChallenge.Data;
using ConexaChallenge.Dtos;
using ConexaChallenge.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConexaChallenge.Services
{
    public class MovieService(AppDbContext dbContext) : IMovieService
    {
        public async Task<List<Movie>> GetAllAsync()
        {
            return await dbContext.Movies.ToListAsync();
        }

        public async Task<Movie?> GetByIdAsync(int id)
        {
            return await dbContext.Movies.FirstOrDefaultAsync(x => x.MovieId == id);
        }

        public async Task<Movie?> CreateAsync(MovieRequest request)
        {
            if (await dbContext.Movies.AnyAsync(x => x.Title == request.Title))
            {
                return null;
            }

            Movie movie = new()
            {
                Title = request.Title,
                Description = request.Description,
                ReleaseDate = request.ReleaseDate
            };

            dbContext.Movies.Add(movie);
            dbContext.SaveChanges();

            return movie;
        }

        public async Task<Movie?> UpdateAsync(int id, MovieRequest movie)
        {
            Movie? existingMovie = await dbContext.Movies.FirstOrDefaultAsync(m => m.MovieId == id);
            if (existingMovie is null)
            {
                return null;
            }

            existingMovie.Title = movie.Title;
            existingMovie.Description = movie.Description;
            existingMovie.ReleaseDate = movie.ReleaseDate;

            await dbContext.SaveChangesAsync();
            return existingMovie;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            Movie? movie = await dbContext.Movies.FirstOrDefaultAsync(x => x.MovieId == id);

            if (movie is null)
            {
                return false;
            }

            dbContext.Movies.Remove(movie);
            await dbContext.SaveChangesAsync();

            return true;
        }
    }
}
