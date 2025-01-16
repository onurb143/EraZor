using Xunit;
using EraZor.Data;
using EraZor.DTO;
using EraZor.Model;
using EraZor.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace EraZor.WipeReports;

public class WipeReportServiceTests
{
    private readonly WipeReportService _service;
    private readonly DataContext _context;

    public WipeReportServiceTests()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new DataContext(options);
        _service = new WipeReportService(_context);

        // Seed data for tests
        SeedData();
    }

    private void SeedData()
    {
        var disk = new Disk { DiskID = 1, SerialNumber = "12345", Manufacturer = "Samsung", Capacity = 512 };
        var wipeMethod = new WipeMethod { WipeMethodID = 1, Name = "Secure Erase", OverwritePass = 3 };
        var user = new IdentityUser { Id = "1", UserName = "Admin" };

        _context.Disks.Add(disk);
        _context.WipeMethods.Add(wipeMethod);
        _context.Users.Add(user);

        _context.WipeJobs.Add(new WipeJob
        {
            WipeJobId = 1,
            StartTime = DateTime.UtcNow.AddHours(-2),
            EndTime = DateTime.UtcNow,
            Status = "Completed",
            Disk = disk,
            WipeMethod = wipeMethod,
            PerformedByUser = user
        });

        _context.SaveChanges();
    }

    [Fact]
    public async Task GetWipeReportsAsync_ReturnsListOfReports_WhenReportsExist()
    {
        // Act
        var result = await _service.GetWipeReportsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("12345", result.First().SerialNumber);
    }

    [Fact]
    public async Task CreateWipeReportAsync_ReturnsTrue_WhenReportIsCreated()
    {
        // Arrange
        var dto = new WipeReportCreateDto
        {
            StartTime = DateTime.UtcNow.AddHours(-1),
            EndTime = DateTime.UtcNow,
            Status = "Completed",
            SerialNumber = "12345",
            WipeMethodName = "Secure Erase",
            OverwritePasses = 3,
            PerformedBy = "Admin"
        };

        // Act
        var result = await _service.CreateWipeReportAsync(dto, "1");

        // Assert
        Assert.True(result);
        Assert.Equal(2, _context.WipeJobs.Count());
    }

    [Fact]
    public async Task DeleteWipeReportAsync_ReturnsTrue_WhenDeletionIsSuccessful()
    {
        // Act
        var result = await _service.DeleteWipeReportAsync(1);

        // Assert
        Assert.True(result);
        Assert.Empty(_context.WipeJobs);
    }

    [Fact]
    public async Task DeleteWipeReportAsync_ReturnsFalse_WhenReportDoesNotExist()
    {
        // Act
        var result = await _service.DeleteWipeReportAsync(99);

        // Assert
        Assert.False(result);
        Assert.Single(_context.WipeJobs);
    }
}
