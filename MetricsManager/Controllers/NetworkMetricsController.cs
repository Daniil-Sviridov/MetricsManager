using MetricsManager.Models;
using MetricsManager.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MetricsManager.DAL.Repositories;

namespace MetricsManager.Controllers
{
    [Route("api/metrics/net")]
    [ApiController]
    public class NetworkMetricsController : ControllerBase
    {
        private readonly ILogger<NetworkMetricsController> _logger;
        private readonly INetworkMetricsRepository _repository;

        public NetworkMetricsController(ILogger<NetworkMetricsController> logger, INetworkMetricsRepository repository)
        {
            _logger = logger;

            _repository = repository;

            _logger.LogInformation("NLog встроен в HddMetricsController");
        }

        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent([FromRoute] int agentId, [FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            _logger.LogInformation($"api/metrics/net/agent/{agentId}/from/{fromTime}/to/{toTime}");

            var metrics = _repository.GetMetricsOutPeriodByAgentId(agentId, fromTime.ToUnixTimeSeconds(), toTime.ToUnixTimeSeconds());
            var response = new MetricsApiResponse<NetworkMetricDTO>();

            foreach (var metric in metrics)
            {
                response.Metrics.Add(new NetworkMetricDTO { AgentId = metric.AgentId, Time = DateTimeOffset.FromUnixTimeSeconds(metric.Time), Value = metric.Value });
            }
            return Ok(response);
        }

        [HttpGet("cluster/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAllCluster([FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            _logger.LogInformation($"api/metrics/net/cluster/from/{fromTime}/to/{toTime}");

            var metrics = _repository.GetMetricsOutPeriod(fromTime.ToUnixTimeSeconds(), toTime.ToUnixTimeSeconds());
            var response = new MetricsApiResponse<NetworkMetricDTO>();

            foreach (var metric in metrics)
            {
                response.Metrics.Add(new NetworkMetricDTO { AgentId = metric.AgentId, Time = DateTimeOffset.FromUnixTimeSeconds(metric.Time), Value = metric.Value });
            }
            return Ok(response);
        }
    }
}
