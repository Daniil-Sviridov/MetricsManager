using MetricsAgent.Controllers;
using Microsoft.AspNetCore.Mvc;
using MetricsAgent.DAL;
using Moq;
using System;
using Xunit;


namespace MetricsAgentTests
{
    public class CpuControllerUnitTest
    {
        private CpuMetricsController _controller;
        private Mock<ICpuMetricsRepository> _mockRepository;
        private Mock<Microsoft.Extensions.Logging.ILogger<CpuMetricsController>> _mockLogger;

        public CpuControllerUnitTest()
        {
            _mockRepository = new Mock<ICpuMetricsRepository>();

            _mockLogger = new Mock<Microsoft.Extensions.Logging.ILogger<CpuMetricsController>>();

            _controller = new CpuMetricsController(_mockLogger.Object, _mockRepository.Object);
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

    internal class rep : IRepository
    { 
    }
}