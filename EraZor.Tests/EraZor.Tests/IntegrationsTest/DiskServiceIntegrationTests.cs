using EraZor.Data;
using EraZor.Model;
using EraZor.Interfaces;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EraZor.Integration
{
    public class DiskServiceIntegrationTests
    {
        private readonly DbContextOptions<DataContext> _options;

        public DiskServiceIntegrationTests()
        {
            // Brug en unik database for hver test for at undgå deling af data
            _options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unik database pr. test
                .Options;
        }

        [Fact, Trait("Category", "Integration")]
        public async Task AddDiskAsync_AddsDiskToDatabase()
        {
            // Arrange
            using var context = new DataContext(_options);
            context.Database.EnsureDeleted(); // Nulstil databasen
            context.Database.EnsureCreated();

            var service = new DiskService(context);
            var newDisk = new Disk
            {
                DiskID = 1,
                SerialNumber = "12345",
                Manufacturer = "Samsung",
                Type = "SSD",
                Capacity = 512
            };

            // Act
            await service.AddDiskAsync(newDisk);

            // Assert
            Assert.Single(context.Disks);
            var disk = await context.Disks.FirstOrDefaultAsync();
            Assert.NotNull(disk);
            Assert.Equal("12345", disk.SerialNumber);
        }

        [Fact, Trait("Category", "Integration")]
        public async Task DeleteDiskAsync_RemovesDiskFromDatabase()
        {
            // Arrange
            using var context = new DataContext(_options);
            context.Database.EnsureDeleted(); // Nulstil databasen
            context.Database.EnsureCreated();

            var service = new DiskService(context);
            var newDisk = new Disk
            {
                DiskID = 1,
                SerialNumber = "12345",
                Manufacturer = "Samsung",
                Type = "SSD",
                Capacity = 512
            };

            context.Disks.Add(newDisk);
            await context.SaveChangesAsync();

            // Act
            await service.DeleteDiskAsync(newDisk.DiskID);

            // Assert
            Assert.Empty(context.Disks);
        }

        [Fact, Trait("Category", "Integration")]
        public async Task DiskExistsAsync_ReturnsTrue_WhenDiskExists()
        {
            // Arrange
            using var context = new DataContext(_options);
            context.Database.EnsureDeleted(); // Nulstil databasen
            context.Database.EnsureCreated();

            var service = new DiskService(context);
            var newDisk = new Disk
            {
                DiskID = 1,
                SerialNumber = "12345",
                Manufacturer = "Samsung",
                Type = "SSD",
                Capacity = 512
            };

            context.Disks.Add(newDisk);
            await context.SaveChangesAsync();

            // Act
            var exists = await service.DiskExistsAsync(newDisk.DiskID);

            // Assert
            Assert.True(exists);
        }

        [Fact, Trait("Category", "Integration")]
        public async Task DiskExistsAsync_ReturnsFalse_WhenDiskDoesNotExist()
        {
            // Arrange
            using var context = new DataContext(_options);
            context.Database.EnsureDeleted(); // Nulstil databasen
            context.Database.EnsureCreated();

            var service = new DiskService(context);

            // Act
            var exists = await service.DiskExistsAsync(999); // Disk med ID 999 findes ikke

            // Assert
            Assert.False(exists);
        }

        [Fact, Trait("Category", "Integration")]
        public async Task GetDisksAsync_ReturnsAllDisks()
        {
            // Arrange
            using var context = new DataContext(_options);
            context.Database.EnsureDeleted(); // Nulstil databasen
            context.Database.EnsureCreated();

            var service = new DiskService(context);
            context.Disks.AddRange(
                new Disk { DiskID = 1, SerialNumber = "12345", Manufacturer = "Samsung", Type = "SSD", Capacity = 512 },
                new Disk { DiskID = 2, SerialNumber = "67890", Manufacturer = "Kingston", Type = "HDD", Capacity = 1024 }
            );
            await context.SaveChangesAsync();

            // Act
            var disks = await service.GetDisksAsync();

            // Assert
            Assert.Equal(2, disks.Count());
        }
    }
}
