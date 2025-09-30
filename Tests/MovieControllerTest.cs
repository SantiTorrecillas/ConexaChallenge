using ConexaChallenge.Controllers;
using ConexaChallenge.Dtos;
using ConexaChallenge.Entities;
using ConexaChallenge.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests
{
    public class MovieControllerTests
    {
        private readonly Mock<IMovieService> _movieServiceMock;
        private readonly MovieController _controller;

        public MovieControllerTests()
        {
            _movieServiceMock = new Mock<IMovieService>();
            _controller = new MovieController(_movieServiceMock.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnListOfMovies()
        {
            List<Movie> movies =
            [
                new() { MovieId = 1, Title = "Movie 1", Description = "Desc 1", ReleaseDate = new DateOnly(2020,1,1) },
                new() { MovieId = 2, Title = "Movie 2", Description = "Desc 2", ReleaseDate = new DateOnly(2021,1,1) }
            ];

            _movieServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(movies);

            ActionResult<List<Movie>> result = await _controller.GetAll();

            OkObjectResult? okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeEquivalentTo(movies);
        }

        [Fact]
        public async Task GetById_ShouldReturnMovie_WhenUserIsNonAdminAndMovieExists()
        {
            int id = 1;
            Movie movie = new() { MovieId = id, Title = "Movie 1", Description = "Desc 1", ReleaseDate = new DateOnly(2020, 1, 1) };

            _movieServiceMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(movie);

            ActionResult<Movie> result = await _controller.GetById(id);

            OkObjectResult? okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeEquivalentTo(movie);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenMovieDoesNotExist()
        {
            int id = 99;

            _movieServiceMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((Movie?)null);

            ActionResult<Movie> result = await _controller.GetById(id);

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedMovie_WhenAdminCreatesMovie()
        {
            MovieRequest request = new() { Title = "New Movie", Description = "Desc", ReleaseDate = new DateOnly(2023, 1, 1) };
            Movie createdMovie = new() { MovieId = 1, Title = "New Movie", Description = "Desc", ReleaseDate = new DateOnly(2023, 1, 1) };

            _movieServiceMock.Setup(s => s.CreateAsync(request)).ReturnsAsync(createdMovie);

            ActionResult<Movie> result = await _controller.Create(request);

            OkObjectResult? okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeEquivalentTo(createdMovie);
        }

        [Fact]
        public async Task Create_ShouldReturnBadRequest_WhenMovieAlreadyExists()
        {
            MovieRequest request = new() { Title = "Existing", Description = "Desc", ReleaseDate = new DateOnly(2023, 1, 1) };

            _movieServiceMock.Setup(s => s.CreateAsync(request)).ReturnsAsync((Movie?)null);

            ActionResult<Movie> result = await _controller.Create(request);

            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Update_ShouldReturnUpdatedMovie_WhenMovieExists()
        {
            int id = 1;
            MovieRequest request = new() { Title = "Updated", Description = "Updated Desc", ReleaseDate = new DateOnly(2022, 1, 1) };
            Movie updatedMovie = new() { MovieId = id, Title = "Updated", Description = "Updated Desc", ReleaseDate = new DateOnly(2022, 1, 1) };

            _movieServiceMock.Setup(s => s.UpdateAsync(id, request)).ReturnsAsync(updatedMovie);

            ActionResult<Movie> result = await _controller.Update(id, request);

            OkObjectResult? okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeEquivalentTo(updatedMovie);
        }

        [Fact]
        public async Task Update_ShouldReturnNotFound_WhenMovieDoesNotExist()
        {
            int id = 99;
            MovieRequest request = new() { Title = "Updated", Description = "Updated Desc", ReleaseDate = new DateOnly(2022, 1, 1) };

            _movieServiceMock.Setup(s => s.UpdateAsync(id, request)).ReturnsAsync((Movie?)null);

            ActionResult<Movie> result = await _controller.Update(id, request);

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenMovieDeleted()
        {
            int id = 1;
            _movieServiceMock.Setup(s => s.DeleteAsync(id)).ReturnsAsync(true);

            ActionResult result = await _controller.Delete(id);

            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenMovieDoesNotExist()
        {
            int id = 99;
            _movieServiceMock.Setup(s => s.DeleteAsync(id)).ReturnsAsync(false);

            ActionResult result = await _controller.Delete(id);

            result.Should().BeOfType<NotFoundResult>();
        }
    }
}