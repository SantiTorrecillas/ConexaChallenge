using ConexaChallenge.Dtos;
using ConexaChallenge.Entities;
using ConexaChallenge.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConexaChallenge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController(IMovieService service) : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<Movie>>> GetAll()
        {
            List<Movie> movies = await service.GetAllAsync();
            return Ok(movies);
        }

        [Authorize(Policy = "NonAdmin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetById(int id)
        {
            Movie? movie = await service.GetByIdAsync(id);
            if (movie is null)
            {
                return NotFound();
            }

            return Ok(movie);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Movie>> Create(MovieRequest request)
        {
            Movie? createdMovie = await service.CreateAsync(request);
            if (createdMovie is null)
            {
                return BadRequest("Already exists.");
            }
            return Ok(createdMovie);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<Movie>> Update(int id, MovieRequest request)
        {
            Movie? updatedMovie = await service.UpdateAsync(id, request);

            if (updatedMovie is null)
            {
                return NotFound();
            }

            return Ok(updatedMovie);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            bool deleted = await service.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
