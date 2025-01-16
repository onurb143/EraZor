using Microsoft.EntityFrameworkCore;
using Xunit;
using EraZor.Data;
using EraZor.Model;

namespace EraZor.Integration.Migrations;

public class DataContextTests
{
    private readonly DbContextOptions<DataContext> _options;

    public DataContextTests()
    {
        _options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
    }

    [Fact]
    public void CanCreateWipeJobsTable()
    {
        using (var context = new DataContext(_options))
        {
            context.Database.EnsureCreated();
            var wipeJobExists = context.Model.GetEntityTypes()
                .Any(t => t.ClrType == typeof(WipeJob));
            Assert.True(wipeJobExists, "WipeJobs table should be created.");
        }
    }

    [Fact]
    public void CanCreateDisksTable()
    {
        using (var context = new DataContext(_options))
        {
            context.Database.EnsureCreated();
            var disksExists = context.Model.GetEntityTypes()
                .Any(t => t.ClrType == typeof(Disk));
            Assert.True(disksExists, "Disks table should be created.");
        }
    }

    [Fact]
    public void CanCreateWipeMethodsTable()
    {
        using (var context = new DataContext(_options))
        {
            context.Database.EnsureCreated();
            var wipeMethodExists = context.Model.GetEntityTypes()
                .Any(t => t.ClrType == typeof(WipeMethod));
            Assert.True(wipeMethodExists, "WipeMethods table should be created.");
        }
    }

    [Fact]
    public void CanCreateWipeReportsView()
    {
        using (var context = new DataContext(_options))
        {
            context.Database.EnsureCreated();
            var wipeReportExists = context.Model.GetEntityTypes()
                .Any(t => t.ClrType == typeof(WipeReport));
            Assert.True(wipeReportExists, "WipeReports view should be created.");
        }
    }
}
