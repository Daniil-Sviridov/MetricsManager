using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MetricsAgent.Controllers
{
    [Route("api/metrics")]
    [ApiController]
    public class NetworkMetricsController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly ILogger<NetworkMetricsController> _logger;

        public NetworkMetricsController(ILogger<NetworkMetricsController> logger, IRepository repository)
        {
            _repository = repository;
            _logger = logger;

            _logger.LogInformation("NLog встроен в NetworkMetricsController");
        }

        [HttpGet("network/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            return Ok();
        }
    }
}
