using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/hdd")]
    [ApiController]
    public class HddMetricsController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly ILogger<HddMetricsController> _logger;

        public HddMetricsController(ILogger<HddMetricsController> logger, IRepository repository)
        {
            _repository = repository;
            _logger = logger;

            _logger.LogInformation("NLog встроен в HddMetricsController");
        }

        [HttpGet("left/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            return Ok();
        }
    }
}
