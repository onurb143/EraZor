using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using EraZor.DTO;
using EraZor.Interfaces;
using Microsoft.AspNetCore.Identity;
using EraZor.Model;

namespace EraZor.WipeReports;

public class WipeReportsControllerTests
{
    private readonly Mock<IWipeReportService> _wipeReportServiceMock;
    private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
    private readonly WipeReportsController _controller;

    public WipeReportsControllerTests()
    {
        _wipeReportServiceMock = new Mock<IWipeReportService>();
        _userManagerMock = new Mock<UserManager<IdentityUser>>(
            Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null
        );

        _controller = new WipeReportsController(_wipeReportServiceMock.Object, _userManagerMock.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, "1"),
                        new Claim(ClaimTypes.Name, "Admin")
                    }, "mock"))
                }
            }
        };
    }

    [Fact]
    public async Task CreateWipeReport_ReturnsUnauthorized_WhenUserIsNotAuthenticated()
    {
        // Arrange
        _userManagerMock.Setup(manager => manager.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                        .ReturnsAsync((IdentityUser)null);

        var dto = new WipeReportCreateDto
        {
            StartTime = DateTime.UtcNow.AddHours(-1),
            EndTime = DateTime.UtcNow,
            Status = "Completed",
            SerialNumber = "123456789",
            Manufacturer = "Samsung",
            WipeMethodName = "Secure Erase",
            OverwritePasses = 3,
            PerformedBy = "Admin"
        };

        // Act
        var result = await _controller.CreateWipeReport(dto);

        // Assert
        var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
        var value = unauthorizedResult.Value as ApiResponse;
        Assert.NotNull(value);
        Assert.Equal("User not found.", value.Message);
    }

    [Fact]
    public async Task CreateWipeReport_ReturnsBadRequest_WhenCreationFails()
    {
        // Arrange
        var user = new IdentityUser { Id = "1", UserName = "Admin" };
        _userManagerMock.Setup(manager => manager.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                        .ReturnsAsync(user);

        var dto = new WipeReportCreateDto
        {
            StartTime = DateTime.UtcNow.AddHours(-1),
            EndTime = DateTime.UtcNow,
            Status = "Completed",
            SerialNumber = "123456789",
            Manufacturer = "Samsung",
            WipeMethodName = "Secure Erase",
            OverwritePasses = 3,
            PerformedBy = "Admin"
        };

        _wipeReportServiceMock.Setup(service => service.CreateWipeReportAsync(dto, user.Id))
                              .ReturnsAsync(false);

        // Act
        var result = await _controller.CreateWipeReport(dto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var value = badRequestResult.Value as ApiResponse;
        Assert.NotNull(value);
        Assert.Equal("Failed to create wipe report.", value.Message);
    }



    [Fact]
    public async Task CreateWipeReport_ReturnsOk_WhenReportIsCreatedSuccessfully()
    {
        // Arrange
        var user = new IdentityUser { Id = "1", UserName = "Admin" };
        _userManagerMock.Setup(manager => manager.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                        .ReturnsAsync(user);

        var dto = new WipeReportCreateDto
        {
            StartTime = DateTime.UtcNow.AddHours(-1),
            EndTime = DateTime.UtcNow,
            Status = "Completed",
            SerialNumber = "123456789",
            Manufacturer = "Samsung",
            WipeMethodName = "Secure Erase",
            OverwritePasses = 3,
            PerformedBy = "Admin"
        };

        _wipeReportServiceMock.Setup(service => service.CreateWipeReportAsync(dto, user.Id))
                              .ReturnsAsync(true);

        // Act
        var result = await _controller.CreateWipeReport(dto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var value = okResult.Value as ApiResponse;
        Assert.NotNull(value);
        Assert.Equal("Wipe report created successfully.", value.Message);
    }


}
