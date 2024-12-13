using Xunit;
using Microsoft.EntityFrameworkCore;
using EraZor.Data;
using EraZor.Model;

public class DatabaseTests
{
    [Fact]
    public void CanConnectToDockerDatabase()
    {
        // Brug din Docker PostgreSQL-database
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseNpgsql("Host=localhost;Port=5432;Database=DatamatikerDB;Username=postgres;Password=Test1234!")
            .Options;

        using (var context = new DataContext(options))
        {
            // Tilføj data
            context.WipeMethods.Add(new WipeMethod { WipeMethodID = 1, Name = "Docker Test Method" });
            context.SaveChanges();

            // Hent data
            var method = context.WipeMethods.FirstOrDefault();
            Assert.NotNull(method);
            Assert.Equal("Docker Test Method", method.Name);
        }
    }

}
