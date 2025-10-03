using ConexaChallenge.Controllers;
using ConexaChallenge.Dtos;
using ConexaChallenge.Entities;
using ConexaChallenge.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests
{
    public class AuthControllerTests
    {
        private readonly Mock<IAuthService> _authServiceMock;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _authServiceMock = new Mock<IAuthService>();
            _controller = new AuthController(_authServiceMock.Object);
        }

        [Fact]
        public async Task Register_ShouldReturnOk_WhenUserIsCreated()
        {
            // Arrange
            UserRequest request = new() { UserName = "newUser", Password = "123" };
            User expectedUser = new() { UserId = 1, UserName = "newUser", Role = "user" };

            _authServiceMock.Setup(s => s.RegisterAsync(request))
                .ReturnsAsync(expectedUser);

            // Act
            ActionResult<User> result = await _controller.Register(request);

            // Assert
            OkObjectResult? okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeEquivalentTo(expectedUser);
        }

        [Fact]
        public async Task Register_ShouldReturnBadRequest_WhenUserAlreadyExists()
        {
            // Arrange
            UserRequest request = new() { UserName = "existingUser", Password = "123" };

            _authServiceMock.Setup(s => s.RegisterAsync(request))
                .ReturnsAsync((User?)null);

            // Act
            ActionResult<User> result = await _controller.Register(request);

            // Assert
            BadRequestObjectResult? badRequest = result.Result as BadRequestObjectResult;
            badRequest.Should().NotBeNull();
            badRequest!.Value.Should().Be("UserName already exists");
        }

        [Fact]
        public async Task Login_ShouldReturnOkWithToken_WhenCredentialsAreValid()
        {
            // Arrange
            UserRequest request = new() { UserName = "user", Password = "123" };

            _authServiceMock.Setup(s => s.LoginAsync(request))
                .ReturnsAsync("fake-jwt-token");

            // Act
            ActionResult<string> result = await _controller.Login(request);

            // Assert
            OkObjectResult? okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.Value.Should().Be("fake-jwt-token");
        }

        [Fact]
        public async Task Login_ShouldReturnBadRequest_WhenCredentialsAreInvalid()
        {
            // Arrange
            UserRequest request = new() { UserName = "user", Password = "wrong" };

            _authServiceMock.Setup(s => s.LoginAsync(request))
                .ReturnsAsync((string?)null);

            // Act
            ActionResult<string> result = await _controller.Login(request);

            // Assert
            BadRequestObjectResult? badRequest = result.Result as BadRequestObjectResult;
            badRequest.Should().NotBeNull();
            badRequest!.Value.Should().Be("Invalid username or password");
        }
    }
}
