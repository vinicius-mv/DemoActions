using API;
using API.Controllers;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Moq;

namespace Tests;

public class WeatherForecastControllerUnitTest
{
    [Fact]
    public void Get_ReturnsCollectionOfWeatherForecasts()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<WeatherForecastController>>();
        var controller = new WeatherForecastController(mockLogger.Object);
        var expectedCount = 5;


        // Act
        var result = controller.Get();

        // Assert
        var weatherForecasts = Assert.IsAssignableFrom<IEnumerable<WeatherForecast>>(result);
        Assert.Equal(expectedCount, weatherForecasts.Count());
    }
}