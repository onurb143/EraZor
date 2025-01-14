using Xunit;
using Microsoft.AspNetCore.Mvc;
using EraZor.Interfaces;
using EraZor.Controllers;
using EraZor.Model;
using Moq;
using EraZor.DTOs;

public class DiskControllerTests
{
    private readonly Mock<IDiskService> _diskServiceMock;
    private readonly DisksController _controller;

    public DiskControllerTests()
    {
        _diskServiceMock = new Mock<IDiskService>();
        _controller = new DisksController(_diskServiceMock.Object);
    }

    [Fact, Trait("Category", "Unit")]
    public async Task GetDisks_ReturnsOkResult_WithDisks()
    {
        // Arrange
        var disks = new List<Disk>
        {
            new Disk
            {
                DiskID = 1,
                Type = "SSD",
                Capacity = 256,
                Path = "C:\\",
                SerialNumber = "123456789",
                Manufacturer = "Samsung"
            }
        };

        _diskServiceMock.Setup(service => service.GetDisksAsync())
                        .ReturnsAsync(disks);

        // Act
        var result = await _controller.GetDisks();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedDisks = Assert.IsType<List<DiskReadDto>>(okResult.Value);
        Assert.Single(returnedDisks);
        Assert.Equal("Samsung", returnedDisks[0].Manufacturer);
        Assert.Equal("C:\\", returnedDisks[0].Path);
        Assert.Equal("123456789", returnedDisks[0].SerialNumber);
    }

    [Fact, Trait("Category", "Unit")]
    public async Task GetDisks_ReturnsOkResult_WhenNoDisksExist()
    {
        // Arrange
        _diskServiceMock.Setup(service => service.GetDisksAsync())
                        .ReturnsAsync(new List<Disk>());

        // Act
        var result = await _controller.GetDisks();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedDisks = Assert.IsType<List<DiskReadDto>>(okResult.Value);
        Assert.Empty(returnedDisks);
    }
}
