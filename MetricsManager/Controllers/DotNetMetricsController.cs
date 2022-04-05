using MetricsManager.DAL.Repositories;
using MetricsManager.Models;
using MetricsManager.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MetricsManager.Controllers
{
    [Route("api/metrics/dotnet")]
    [ApiController]
    public class DotNetMetricsController : ControllerBase
    {
        private readonly ILogger<DotNetMetricsController> _logger;
        private readonly IDotNetMetricsRepository _repository;

        public DotNetMetricsController(ILogger<DotNetMetricsController> logger, IDotNetMetricsRepository repository)
        {
            _logger = logger;
            _repository = repository;

            _logger.LogInformation("NLog встроен в DotNetMetricsController");
        }

        /// <summary>
        /// Получает метрики DotNet на заданном агете в диапазоне  времени
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
            _logger.LogInformation($"api/metrics/dotnet/agent/{agentId}/from/{fromTime}/to/{toTime}");

            var metrics = _repository.GetMetricsOutPeriodByAgentId(agentId, fromTime.ToUnixTimeSeconds(), toTime.ToUnixTimeSeconds());
            var response = new MetricsApiResponse<DotNetMetricDTO>();

            foreach (var metric in metrics)
            {
                response.Metrics.Add(new DotNetMetricDTO { AgentId = metric.AgentId, Time = DateTimeOffset.FromUnixTimeSeconds(metric.Time), Value = metric.Value });
            }
            return Ok(response);
        }

        /// <summary>
        /// Получает метрики DotNet по всем агетам в диапазоне времени
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
            _logger.LogInformation($"api/metrics/dotnet/cluster/from/{fromTime}/to/{toTime}");

            var metrics = _repository.GetMetricsOutPeriod(fromTime.ToUnixTimeSeconds(), toTime.ToUnixTimeSeconds());
            var response = new MetricsApiResponse<DotNetMetricDTO>();

            foreach (var metric in metrics)
            {
                response.Metrics.Add(new DotNetMetricDTO { AgentId = metric.AgentId, Time = DateTimeOffset.FromUnixTimeSeconds(metric.Time), Value = metric.Value });
            }
            return Ok(response);
        }
    }
}
