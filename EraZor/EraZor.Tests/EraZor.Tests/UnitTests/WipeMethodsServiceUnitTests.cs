using EraZor.Data;
using EraZor.Interfaces;
using EraZor.Model;
using EraZor.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EraZor.Units
{
    public class WipeMethodServiceTests
    {
        private readonly DbContextOptions<DataContext> _options;

        public WipeMethodServiceTests()
        {
            // Brug en unik database for hver test
            _options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unik database
                .Options;
        }

        [Fact]
        public async Task GetAllWipeMethodsAsync_ReturnsAllMethods()
        {
            using var context = new DataContext(_options);
            var service = new WipeMethodService(context);

            // Arrange
            context.WipeMethods.AddRange(new List<WipeMethod>
            {
                new WipeMethod { Name = "Zero Fill", OverwritePass = 1, Description = "Fills with zeroes." },
                new WipeMethod { Name = "Random Fill", OverwritePass = 3, Description = "Fills with random data." }
            });
            await context.SaveChangesAsync();

            // Act
            var result = await service.GetAllWipeMethodsAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, wm => wm.Name == "Zero Fill");
            Assert.Contains(result, wm => wm.Name == "Random Fill");
        }

        [Fact]
        public async Task GetWipeMethodByIdAsync_ReturnsWipeMethod_WhenFound()
        {
            using var context = new DataContext(_options);
            var service = new WipeMethodService(context);

            // Arrange
            var wipeMethod = new WipeMethod { Name = "Zero Fill", OverwritePass = 1, Description = "Fills with zeroes." };
            context.WipeMethods.Add(wipeMethod);
            await context.SaveChangesAsync();

            // Act
            var result = await service.GetWipeMethodByIdAsync(wipeMethod.WipeMethodID);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Zero Fill", result.Name);
        }

        [Fact]
        public async Task DeleteWipeMethodAsync_RemovesMethod_WhenFound()
        {
            using var context = new DataContext(_options);
            var service = new WipeMethodService(context);

            // Arrange
            var wipeMethod = new WipeMethod { Name = "Zero Fill", OverwritePass = 1, Description = "Fills with zeroes." };
            context.WipeMethods.Add(wipeMethod);
            await context.SaveChangesAsync();

            // Act
            await service.DeleteWipeMethodAsync(wipeMethod.WipeMethodID);

            // Assert
            var result = await context.WipeMethods.FindAsync(wipeMethod.WipeMethodID);
            Assert.Null(result); // Bekræft, at metoden blev slettet
        }

        [Fact]
        public async Task DeleteWipeMethodAsync_DoesNothing_WhenNotFound()
        {
            using var context = new DataContext(_options);
            var service = new WipeMethodService(context);

            // Arrange
            // Ingen data tilføjet

            // Act
            await service.DeleteWipeMethodAsync(1);

            // Assert
            Assert.Empty(context.WipeMethods); // Bekræft, at databasen stadig er tom
        }
    }
}
