using EraZor.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebKlient.Controllers;
using Erazor.DTOs;
using EraZor.Model;
using Xunit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace EraZor.Units
{
    public class AuthControllerUnitTest
    {
        private readonly Mock<IAuthService> _authServiceMock;
        private readonly AuthController _controller;

        public AuthControllerUnitTest()
        {
            _authServiceMock = new Mock<IAuthService>();
            _controller = new AuthController(_authServiceMock.Object);
        }


        [Fact]
        public async Task Login_ReturnsBadRequest_WhenModelIsNull()
        {
            // Arrange
            LoginModel model = null;

            // Act
            var result = await _controller.Login(model);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var badRequestValue = Assert.IsType<AuthResponse>(badRequestResult.Value); // Bind to AuthResponse
            Assert.Equal("Email and password are required.", badRequestValue.Message); // Access Message
        }


        [Fact]
        public async Task Login_ReturnsBadRequest_WhenEmailOrPasswordIsEmpty()
        {
            // Arrange
            var model = new LoginModel { Email = "", Password = "Password123" };

            // Act
            var result = await _controller.Login(model);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var badRequestValue = Assert.IsType<AuthResponse>(badRequestResult.Value); // Bind to AuthResponse
            Assert.Equal("Email and password are required.", badRequestValue.Message); // Access Message
        }


        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenInvalidCredentials()
        {
            // Arrange
            var model = new LoginModel { Email = "test@example.com", Password = "wrongpassword" };
            _authServiceMock.Setup(service => service.LoginAsync(It.IsAny<string>(), It.IsAny<string>()))
                            .ReturnsAsync((IdentityUser)null); // Simulate invalid login

            // Act
            var result = await _controller.Login(model);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            var unauthorizedValue = Assert.IsType<AuthResponse>(unauthorizedResult.Value); // Bind to AuthResponse
            Assert.Equal("Invalid credentials", unauthorizedValue.Message); // Access Message property
        }



        [Fact]
        public async Task Login_ReturnsOk_WhenLoginIsSuccessful()
        {
            // Arrange
            var model = new LoginModel { Email = "test@example.com", Password = "correctpassword" };
            var mockUser = new IdentityUser { UserName = "test@example.com" };
            var mockToken = "fake.jwt.token";

            _authServiceMock.Setup(service => service.LoginAsync(It.IsAny<string>(), It.IsAny<string>()))
                            .ReturnsAsync(mockUser);
            _authServiceMock.Setup(service => service.GenerateJwtToken(It.IsAny<IdentityUser>()))
                            .ReturnsAsync(mockToken);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            // Act
            var result = await _controller.Login(model);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var okValue = Assert.IsType<AuthResponse>(okResult.Value);

            Assert.NotNull(okValue);
            Assert.Equal("Login successful", okValue.Message);
            Assert.Equal(mockToken, okValue.Token);
        }






    }
}
