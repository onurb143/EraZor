using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using EraZor.Controllers;
using EraZor.DTOs;
using EraZor.Interfaces;
using EraZor.Model;

namespace EraZor.Units;

public class DisksControllerTests
{
    private readonly Mock<IDiskService> _diskServiceMock;
    private readonly DisksController _controller;

    public DisksControllerTests()
    {
        _diskServiceMock = new Mock<IDiskService>();
        _controller = new DisksController(_diskServiceMock.Object);
    }

    [Fact]
    public async Task GetDisks_ReturnsOkResult_WithDisks()
    {
        // Arrange
        var disks = new List<Disk>
        {
            new Disk { DiskID = 1, Type = "SSD", Capacity = 256, Path = "C:\\", SerialNumber = "123", Manufacturer = "Samsung" }
        };

        _diskServiceMock.Setup(service => service.GetDisksAsync()).ReturnsAsync(disks);

        // Act
        var result = await _controller.GetDisks();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedDisks = Assert.IsType<List<DiskReadDto>>(okResult.Value);
        Assert.Single(returnedDisks);
        Assert.Equal("Samsung", returnedDisks[0].Manufacturer);
    }

    [Fact]
    public async Task GetDisks_ReturnsOkResult_WhenNoDisksExist()
    {
        // Arrange
        _diskServiceMock.Setup(service => service.GetDisksAsync()).ReturnsAsync(new List<Disk>());

        // Act
        var result = await _controller.GetDisks();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedDisks = Assert.IsType<List<DiskReadDto>>(okResult.Value);
        Assert.Empty(returnedDisks);
    }

    [Fact]
    public async Task GetDisk_ReturnsOkResult_WhenDiskExists()
    {
        // Arrange
        var disk = new Disk { DiskID = 1, Type = "SSD", Capacity = 256, Path = "C:\\", SerialNumber = "123", Manufacturer = "Samsung" };

        _diskServiceMock.Setup(service => service.GetDiskByIdAsync(1)).ReturnsAsync(disk);

        // Act
        var result = await _controller.GetDisk(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedDisk = Assert.IsType<DiskReadDto>(okResult.Value);
        Assert.Equal("Samsung", returnedDisk.Manufacturer);
    }

    [Fact]
    public async Task GetDisk_ReturnsNotFound_WhenDiskDoesNotExist()
    {
        // Arrange
        _diskServiceMock.Setup(service => service.GetDiskByIdAsync(1)).ReturnsAsync((Disk)null);

        // Act
        var result = await _controller.GetDisk(1);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task CreateDisk_ReturnsCreatedAtActionResult_WithNewDisk()
    {
        // Arrange
        var dto = new DiskCreateDto
        {
            Type = "SSD",
            Capacity = 512,
            Path = "D:\\",
            SerialNumber = "ABC123",
            Manufacturer = "Intel"
        };

        var createdDisk = new Disk
        {
            DiskID = 1,
            Type = "SSD",
            Capacity = 512,
            Path = "D:\\",
            SerialNumber = "ABC123",
            Manufacturer = "Intel"
        };

        _diskServiceMock.Setup(service => service.AddDiskAsync(It.IsAny<Disk>()))
                        .Callback<Disk>(d => d.DiskID = 1);

        // Act
        var result = await _controller.CreateDisk(dto);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var returnedDisk = Assert.IsType<DiskReadDto>(createdResult.Value);
        Assert.Equal("Intel", returnedDisk.Manufacturer);
    }

    [Fact]
    public async Task CreateDisk_ReturnsBadRequest_WhenDtoIsInvalid()
    {
        // Arrange
        DiskCreateDto invalidDto = null;

        // Act
        var result = await _controller.CreateDisk(invalidDto);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task DeleteDisk_ReturnsNoContent_WhenDiskExists()
    {
        // Arrange
        _diskServiceMock.Setup(service => service.DiskExistsAsync(1)).ReturnsAsync(true);
        _diskServiceMock.Setup(service => service.DeleteDiskAsync(1)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteDisk(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteDisk_ReturnsNotFound_WhenDiskDoesNotExist()
    {
        // Arrange
        _diskServiceMock.Setup(service => service.DiskExistsAsync(1)).ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteDisk(1);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }
}
