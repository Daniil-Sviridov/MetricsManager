using MetricsManager.Models;
using MetricsManager.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MetricsManager.DAL.Repositories;

namespace MetricsManager.Controllers
{
    [Route("api/metrics/ram")]
    [ApiController]
    public class RamMetricsController : ControllerBase
    {
        private readonly ILogger<RamMetricsController> _logger;
        private readonly IRamMetricsRepository _repository;

        public RamMetricsController(ILogger<RamMetricsController> logger, IRamMetricsRepository repository)
        {
            _logger = logger;
            _repository = repository;

            _logger.LogInformation("NLog встроен в RamMetricsController");
        }

        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent([FromRoute] int agentId, [FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            _logger.LogInformation($"api/metrics/ram/agent/{agentId}/from/{fromTime}/to/{toTime}");

            var metrics = _repository.GetMetricsOutPeriodByAgentId(agentId, fromTime.ToUnixTimeSeconds(), toTime.ToUnixTimeSeconds());
            var response = new MetricsApiResponse<RamMetricDTO>();

            foreach (var metric in metrics)
            {
                response.Metrics.Add(new RamMetricDTO { AgentId = metric.AgentId, Time = DateTimeOffset.FromUnixTimeSeconds(metric.Time), Value = metric.Value });
            }
            return Ok(response);
        }

        [HttpGet("cluster/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAllCluster([FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            _logger.LogInformation($"api/metrics/ram/cluster/from/{fromTime}/to/{toTime}");

            var metrics = _repository.GetMetricsOutPeriod(fromTime.ToUnixTimeSeconds(), toTime.ToUnixTimeSeconds());
            var response = new MetricsApiResponse<RamMetricDTO>();

            foreach (var metric in metrics)
            {
                response.Metrics.Add(new RamMetricDTO { AgentId = metric.AgentId, Time = DateTimeOffset.FromUnixTimeSeconds(metric.Time), Value = metric.Value });
            }
            return Ok(response);
        }
    }
}
