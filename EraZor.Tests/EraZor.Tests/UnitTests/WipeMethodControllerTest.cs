using Xunit;
using Microsoft.AspNetCore.Mvc;
using Moq;
using EraZor.Interfaces;
using EraZor.Model;
using Erazor.Controllers;

namespace EraZor.Units;

public class WipeMethodsControllerTests
{
    private readonly Mock<IWipeMethodService> _wipeMethodServiceMock;
    private readonly WipeMethodsController _controller;

    public WipeMethodsControllerTests()
    {
        _wipeMethodServiceMock = new Mock<IWipeMethodService>();
        _controller = new WipeMethodsController(_wipeMethodServiceMock.Object);
    }

    [Fact]
    public async Task GetWipeMethods_ReturnsOk_WhenSuccessful()
    {
        // Arrange
        var wipeMethods = new List<WipeMethod>
        {
            new WipeMethod { WipeMethodID = 1, Name = "Zero Fill" },
            new WipeMethod { WipeMethodID = 2, Name = "Random Fill" }
        };
        _wipeMethodServiceMock.Setup(service => service.GetAllWipeMethodsAsync())
                               .ReturnsAsync(wipeMethods);

        // Act
        var result = await _controller.GetWipeMethods();

        // Assert
        var actionResult = Assert.IsType<ActionResult<IEnumerable<WipeMethod>>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var returnValue = Assert.IsAssignableFrom<IEnumerable<WipeMethod>>(okResult.Value);
        Assert.Equal(2, returnValue.Count());
    }

    [Fact]
    public async Task GetWipeMethods_ReturnsInternalServerError_WhenExceptionThrown()
    {
        // Arrange
        _wipeMethodServiceMock.Setup(service => service.GetAllWipeMethodsAsync())
                               .ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _controller.GetWipeMethods();

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, objectResult.StatusCode);
        Assert.Contains("Internal server error", objectResult.Value.ToString());
    }

    [Fact]
    public async Task DeleteWipeMethod_ReturnsNoContent_WhenDeleteIsSuccessful()
    {
        // Arrange
        var wipeMethod = new WipeMethod { WipeMethodID = 1, Name = "Zero Fill" };

        _wipeMethodServiceMock.Setup(service => service.GetWipeMethodByIdAsync(It.IsAny<int>()))
                              .ReturnsAsync(wipeMethod);

        _wipeMethodServiceMock.Setup(service => service.DeleteWipeMethodAsync(It.IsAny<int>()))
                              .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteWipeMethod(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteWipeMethod_ReturnsNotFound_WhenMethodNotFound()
    {
        // Arrange
        _wipeMethodServiceMock.Setup(service => service.GetWipeMethodByIdAsync(It.IsAny<int>()))
                               .ReturnsAsync((WipeMethod)null); // Return null if method not found

        // Act
        var result = await _controller.DeleteWipeMethod(1);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);

        // Brug System.Text.Json for at læse det anonyme objekt
        var json = System.Text.Json.JsonSerializer.Serialize(notFoundResult.Value);
        var message = System.Text.Json.JsonDocument.Parse(json)
                        .RootElement.GetProperty("Message").GetString();

        // Tjek at meddelelsen er korrekt
        Assert.Equal("Wipe method not found", message);
    }




    [Fact]
    public async Task DeleteWipeMethod_ReturnsInternalServerError_WhenExceptionThrown()
    {
        // Arrange
        _wipeMethodServiceMock.Setup(service => service.GetWipeMethodByIdAsync(It.IsAny<int>()))
                               .ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _controller.DeleteWipeMethod(1);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
        Assert.Contains("Internal server error", objectResult.Value.ToString());
    }
}
