using EraZor.Data;
using EraZor.Model;
using EraZor.Interfaces;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EraZor.Units;

public class DiskServiceTests
{
    private readonly DiskService _diskService;
    private readonly DataContext _context;

    public DiskServiceTests()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new DataContext(options);
        _diskService = new DiskService(_context);
    }

    [Fact]
    public async Task AddDiskAsync_AddsNewDisk()
    {
        // Arrange
        var newDisk = new Disk
        {
            DiskID = 1,
            SerialNumber = "12345",
            Type = "SSD",
            Capacity = 512,
            Manufacturer = "Example"
        };

        // Act
        await _diskService.AddDiskAsync(newDisk);

        // Assert
        Assert.Single(_context.Disks); // Kontroller, at der kun er én disk i databasen
        var disk = await _context.Disks.FirstOrDefaultAsync();
        Assert.NotNull(disk);
        Assert.Equal("12345", disk.SerialNumber);
    }

    [Fact]
    public async Task GetDiskByIdAsync_ReturnsDisk_WhenDiskExists()
    {
        // Arrange
        var newDisk = new Disk
        {
            DiskID = 1,
            SerialNumber = "12345",
            Type = "SSD",
            Capacity = 512,
            Manufacturer = "Example"
        };

        _context.Disks.Add(newDisk);
        await _context.SaveChangesAsync();

        // Act
        var result = await _diskService.GetDiskByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("12345", result.SerialNumber);
    }

    [Fact]
    public async Task GetDiskByIdAsync_ReturnsNull_WhenDiskDoesNotExist()
    {
        // Act
        var result = await _diskService.GetDiskByIdAsync(1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteDiskAsync_RemovesDisk_WhenDiskExists()
    {
        // Arrange
        var newDisk = new Disk
        {
            DiskID = 1,
            SerialNumber = "12345",
            Type = "SSD",
            Capacity = 512,
            Manufacturer = "Example"
        };

        _context.Disks.Add(newDisk);
        await _context.SaveChangesAsync();

        // Act
        await _diskService.DeleteDiskAsync(1);

        // Assert
        Assert.Empty(_context.Disks);
    }

    [Fact]
    public async Task DiskExistsAsync_ReturnsTrue_WhenDiskExists()
    {
        // Arrange
        var newDisk = new Disk
        {
            DiskID = 1,
            SerialNumber = "12345",
            Type = "SSD",
            Capacity = 512,
            Manufacturer = "Example"
        };

        _context.Disks.Add(newDisk);
        await _context.SaveChangesAsync();

        // Act
        var exists = await _diskService.DiskExistsAsync(1);

        // Assert
        Assert.True(exists);
    }

    [Fact]
    public async Task DiskExistsAsync_ReturnsFalse_WhenDiskDoesNotExist()
    {
        // Act
        var exists = await _diskService.DiskExistsAsync(1);

        // Assert
        Assert.False(exists);
    }
}
