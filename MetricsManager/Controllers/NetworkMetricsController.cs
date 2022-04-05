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

        /// <summary>
        /// Получает метрики Network на заданном агете в диапазоне  времени
        /// </summary>
        /// /// <remarks>
        /// Пример запроса:
        ///
        /// GET api/metrics/cpu/agent/1/from/2022-04-04T21:33:40+00:00/to/2022-04-05T23:16:50+00:00
        ///
        /// </remarks>
        /// <param name="agentId">ID агента менеджера сбора метри</param>
        /// <param name="fromTime">начало периода получения данных 1999-12-31T23:59:59+00:00</param>
        /// <param name="toTime">конец периода получения данных 2022-12-31T23:59:59+00:00</param>
        /// <returns>Список метрик, сохранённых в заданном диапазоне времени</returns>
        /// <response code="200">Если всё хорошо</response>
        /// <response code="400">Если передали неправильные параметры</response>
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

        /// <summary>
        /// Получает метрики Network по всем агетам в диапазоне времени
        /// </summary>
        /// /// <remarks>
        /// Пример запроса:
        ///
        /// GET api/metrics/cpu/from/2022-04-04T21:33:40+00:00/to/2022-04-05T23:16:50+00:00
        ///
        /// </remarks>
        /// <param name="fromTime">начало периода получения данных 1999-12-31T23:59:59+00:00</param>
        /// <param name="toTime">конец периода получения данных 2022-12-31T23:59:59+00:00</param>
        /// <returns>Список метрик, сохранённых в заданном диапазоне времени</returns>
        /// <response code="200">Если всё хорошо</response>
        /// <response code="400">Если передали неправильные параметры</response>
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
