using EraZor.Data;
using EraZor.Model;
using EraZor.Interfaces;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EraZor.Integration;

public class WipeMethodServiceIntegrationTests
{
    private readonly DbContextOptions<DataContext> _options;
    private readonly DataContext _context;
    private readonly WipeMethodService _service;

    public WipeMethodServiceIntegrationTests()
    {
        _options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_WipeMethod")
            .Options;

        _context = new DataContext(_options);
        _service = new WipeMethodService(_context);
    }

    [Fact, Trait("Category", "Integration")]
    public async Task GetAllWipeMethodsAsync_ReturnsAllWipeMethods()
    {
        // Arrange
        _context.WipeMethods.Add(new WipeMethod { WipeMethodID = 1, Name = "Method1" });
        _context.WipeMethods.Add(new WipeMethod { WipeMethodID = 2, Name = "Method2" });
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetAllWipeMethodsAsync();

        // Assert
        Assert.Equal(2, result.Count());
    }



    [Fact, Trait("Category", "Integration")]
    public async Task DeleteWipeMethodAsync_RemovesWipeMethodFromDatabase()
    {
        // Arrange
        var wipeMethod = new WipeMethod { WipeMethodID = 1, Name = "MethodToDelete" };
        _context.WipeMethods.Add(wipeMethod);
        await _context.SaveChangesAsync();

        // Act
        await _service.DeleteWipeMethodAsync(1);

        // Assert
        Assert.Empty(_context.WipeMethods);
    }
}

