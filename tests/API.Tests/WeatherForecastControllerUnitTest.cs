using API.Controllers;
using Microsoft.Extensions.Logging;
using Moq;

namespace API.Tests;

public class WeatherForecastControllerUnitTest
{
    [Fact]
    public void Get_Returns1CollectionOfWeatherForecasts()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<WeatherForecastController>>();
        var controller = new WeatherForecastController(mockLogger.Object);
        var expectedCount = 4;


        // Act
        var result = controller.Get();

        // Assert
        var weatherForecasts = Assert.IsAssignableFrom<IEnumerable<WeatherForecast>>(result);
        Assert.Equal(expectedCount, weatherForecasts.Count());
    }
}
