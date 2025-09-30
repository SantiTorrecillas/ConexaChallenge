using ConexaChallenge.Dtos;
using ConexaChallenge.Entities;

namespace ConexaChallenge.Services
{
    public interface IMovieService
    {
        Task<List<Movie>> GetAllAsync();
        Task<Movie?> GetByIdAsync(int id);
        Task<Movie?> CreateAsync(MovieRequest movie);
        Task<Movie?> UpdateAsync(int id, MovieRequest movie);
        Task<bool> DeleteAsync(int id);
    }
}
