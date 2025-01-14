using EraZor.Data;
using EraZor.DTOs;
using EraZor.Interfaces;
using EraZor.Model;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class DiskIntegrationTests
{
    private readonly DbContextOptions<DataContext> _options;

    public DiskIntegrationTests()
    {
        // Opsæt en in-memory database til testen
        _options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
    }

    [Fact, Trait("Category", "Integration")]
    public void CreateDisk_AddsDiskToDatabase()
    {
        using (var context = new DataContext(_options))
        {
            // Arrange
            var service = new DiskService(context);
            var newDisk = new DiskCreateDto
            {
                Type = "SSD",
                Capacity = 256,
                Path = "C:\\",
                SerialNumber = "12345678",
                Manufacturer = "Samsung"
            };

            // Act
            service.AddDiskAsync(new Disk
            {
                Type = newDisk.Type,
                Capacity = newDisk.Capacity,
                Path = newDisk.Path,
                SerialNumber = newDisk.SerialNumber,
                Manufacturer = newDisk.Manufacturer
            }).Wait();

            // Assert
            Assert.Single(context.Disks);
            var disk = context.Disks.First();
            Assert.Equal("Samsung", disk.Manufacturer);
        }
    }
}
