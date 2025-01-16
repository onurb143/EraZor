using EraZor.Data;
using EraZor.Model;
using EraZor.Interfaces;
using EraZor.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EraZor.WipeReports;

public class WipeReportServiceIntegrationTests
{
    private readonly DbContextOptions<DataContext> _options;
    private readonly DataContext _context;
    private readonly WipeReportService _service;

    public WipeReportServiceIntegrationTests()
    {
        _options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_WipeReport")
            .Options;

        _context = new DataContext(_options);
        _service = new WipeReportService(_context);
    }

    private async Task InitializeDatabase()
    {
        await _context.Database.EnsureCreatedAsync();
        _context.WipeJobs.RemoveRange(_context.WipeJobs);
        _context.Disks.RemoveRange(_context.Disks);
        _context.WipeMethods.RemoveRange(_context.WipeMethods);
        await _context.SaveChangesAsync();
    }

    [Fact, Trait("Category", "Integration")]
    public async Task CreateWipeReportAsync_ReturnsTrue_WhenValidData()
    {
        // Arrange
        await InitializeDatabase();
        var disk = new Disk
        {
            DiskID = 1,
            SerialNumber = "12345",
            Manufacturer = "Samsung",
            Type = "SSD",
            Capacity = 256
        };
        var wipeMethod = new WipeMethod
        {
            WipeMethodID = 1,
            Name = "Zero Fill",
            OverwritePass = 3,
            Description = "Simple method to overwrite with zeroes."
        };
        _context.Disks.Add(disk);
        _context.WipeMethods.Add(wipeMethod);
        await _context.SaveChangesAsync();

        var dto = new WipeReportCreateDto
        {
            StartTime = DateTime.UtcNow.AddHours(-2),
            EndTime = DateTime.UtcNow,
            Status = "Completed",
            SerialNumber = "12345",
            WipeMethodName = "Zero Fill",
            Manufacturer = "Samsung",
            OverwritePasses = 3,
            PerformedBy = "Tester"
        };

        // Act
        var result = await _service.CreateWipeReportAsync(dto, "userId");

        // Assert
        Assert.True(result);
        var wipeJob = await _context.WipeJobs.Include(wj => wj.Disk).Include(wj => wj.WipeMethod).FirstOrDefaultAsync();
        Assert.NotNull(wipeJob);
        Assert.Equal("Completed", wipeJob.Status);
        Assert.Equal(disk.DiskID, wipeJob.DiskId);
        Assert.Equal(wipeMethod.WipeMethodID, wipeJob.WipeMethodId);
        Assert.Equal(3, wipeMethod.OverwritePass);
        Assert.Equal("userId", wipeJob.PerformedByUserId);
    }

    [Fact, Trait("Category", "Integration")]
    public async Task GetWipeReportsAsync_ReturnsAllWipeReports()
    {
        // Arrange
        await InitializeDatabase();
        _context.WipeJobs.Add(new WipeJob
        {
            WipeJobId = 1,
            StartTime = DateTime.UtcNow.AddHours(-2),
            EndTime = DateTime.UtcNow,
            Status = "Completed",
            Disk = new Disk { Type = "SSD", Capacity = 256, SerialNumber = "12345", Manufacturer = "Samsung" },
            WipeMethod = new WipeMethod { Name = "Zero Fill", OverwritePass = 1 },
            PerformedByUser = new IdentityUser { UserName = "tester" }
        });
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetWipeReportsAsync();

        // Assert
        Assert.Single(result);
        var report = result.First();
        Assert.Equal("12345", report.SerialNumber);
        Assert.Equal("Zero Fill", report.WipeMethodName);
    }

    [Fact, Trait("Category", "Integration")]
    public async Task DeleteWipeReportAsync_RemovesWipeJobFromDatabase()
    {
        // Arrange
        await InitializeDatabase();
        _context.WipeJobs.Add(new WipeJob { WipeJobId = 1 });
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.DeleteWipeReportAsync(1);

        // Assert
        Assert.True(result);
        Assert.Empty(_context.WipeJobs);
    }
}
