using MetricsAgent.Controllers;
using MetricsAgent.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace MetricsAgentTests
{
    public class HddControllerUnitTest
    {
        private Mock<ILogger<HddMetricsController>> _mockLogger;
        private HddMetricsController _controller;
        private Mock<IHddMetricsRepository> _mockRepository;

        public HddControllerUnitTest()
        {
            _mockLogger = new Mock<ILogger<HddMetricsController>>();
            _mockRepository = new Mock<IHddMetricsRepository>();

            _controller = new HddMetricsController(_mockLogger.Object, _mockRepository.Object);
        }

        [Fact]
        public void GetMetricsFromAgent_ReturnsOk()
        {
            //Arrange
            var fromTime = TimeSpan.FromSeconds(0);
            var toTime = TimeSpan.FromSeconds(100);

            //Act
            var result = _controller.GetMetricsFromAgent(fromTime, toTime);

            // Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}