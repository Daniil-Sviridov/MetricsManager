using MetricsAgent.Controllers;
using MetricsAgent.DAL;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using Xunit;

namespace MetricsAgentTests
{
    public class DotNetControllerUnitTest
    {
        private DotNetMetricsController _controller;
        private Mock<IDotNetMetricsRepository> _mockRepository;
        private Mock<Microsoft.Extensions.Logging.ILogger<DotNetMetricsController>> _mockLogger;

        public DotNetControllerUnitTest()
        {
            _mockRepository = new Mock<IDotNetMetricsRepository>();

            _mockLogger = new Mock<Microsoft.Extensions.Logging.ILogger<DotNetMetricsController>>();

        //    _controller = new DotNetMetricsController(_mockLogger.Object, _mockRepository.Object);
        }

        [Fact]
        public void GetMetricsFromAgent_ReturnsOk()
        {
            //Arrange
            var fromTime = DateTimeOffset.MinValue;
            var toTime = DateTimeOffset.MinValue;

            //Act
            var result = _controller.GetMetricsFromAgent(fromTime, toTime);

            // Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}