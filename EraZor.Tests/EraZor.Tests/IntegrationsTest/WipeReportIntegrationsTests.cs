using EraZor.Data;
using EraZor.Model;
using EraZor.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EraZor.WipeReports;


public class WipeReportIntegrationTests
{
    private readonly DbContextOptions<DataContext> _options;

    public WipeReportIntegrationTests()
    {
        // Opsæt SQLite in-memory database
        _options = new DbContextOptionsBuilder<DataContext>()
            .UseSqlite("Filename=:memory:")
            .Options;
    }

    [Fact, Trait("Category", "Integration")]
    public async Task FetchWipeReports_ReturnsViewData()
    {
        using (var context = new DataContext(_options))
        {
            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            // Arrange
            var disk = new Disk { SerialNumber = "12345", Type = "SSD", Capacity = 1024, Manufacturer = "Example" };
            var wipeMethod = new WipeMethod { Name = "Secure Erase", OverwritePass = 3 };
            var user = new IdentityUser { UserName = "Tester" };

            context.Disks.Add(disk);
            context.WipeMethods.Add(wipeMethod);
            context.Users.Add(user);

            context.WipeJobs.Add(new WipeJob
            {
                WipeJobId = 1,
                Status = "Completed",
                Disk = disk,
                WipeMethod = wipeMethod,
                PerformedByUser = user,
                StartTime = DateTime.UtcNow.AddHours(-1),
                EndTime = DateTime.UtcNow
            });
            await context.SaveChangesAsync();

            // Act
            var service = new WipeReportService(context);
            var wipeReports = await service.GetWipeReportsAsync();

            // Assert
            Assert.Single(wipeReports);
            var report = wipeReports.First();
            Assert.Equal("12345", report.SerialNumber);
            Assert.Equal("Secure Erase", report.WipeMethodName);
            Assert.Equal("Completed", report.Status);
        }
    }

}





