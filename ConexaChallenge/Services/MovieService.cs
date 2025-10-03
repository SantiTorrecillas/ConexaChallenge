using ConexaChallenge.Data;
using ConexaChallenge.Dtos;
using ConexaChallenge.Entities;
using ConexaChallenge.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ConexaChallenge.Services
{
    public class MovieService(AppDbContext dbContext, IHttpClientFactory httpClientFactory) : IMovieService
    {
        private readonly AppDbContext dbContext = dbContext;
        private readonly IHttpClientFactory httpClientFactory = httpClientFactory;

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
            await dbContext.SaveChangesAsync();

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

        public async Task<SwapiSyncResult?> SyncSwapiFilmsAsync()
        {
            HttpClient client = httpClientFactory.CreateClient();

            try
            {
                SwapiFilmResponse? response = await client.GetFromJsonAsync<SwapiFilmResponse>("https://swapi.dev/api/films");
                if (response is null || response.Results.Count == 0)
                {
                    return null;
                }

                SwapiSyncResult result = new();

                foreach (SwapiFilm film in response.Results)
                {
                    MovieRequest movieRequest = new()
                    {
                        Title = film.Title,
                        Description = film.Opening_crawl,
                        ReleaseDate = DateOnly.Parse(film.Release_date)
                    };

                    Movie? existing = await dbContext.Movies.FirstOrDefaultAsync(x => x.Title == film.Title);
                    if (existing is not null)
                    {
                        await UpdateAsync(existing.MovieId, movieRequest);
                        result.Updated++;
                    }
                    else
                    {
                        await CreateAsync(movieRequest);
                        result.Created++;
                    }
                }

                return result;
            }
            catch (Exception)
            {
                //Should log SWAPI's fail
                return null;
            }            
        }
    }
}
