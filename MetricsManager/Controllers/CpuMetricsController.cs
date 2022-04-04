using MetricsManager.DAL.Repositories;
using MetricsManager.Models;
using MetricsManager.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace MetricsManager.Controllers
{
    [Route("api/metrics/cpu")]
    [ApiController]
    public class CpuMetricsController : ControllerBase
    {
        private readonly ILogger<CpuMetricsController> _logger;
        private readonly ICpuMetricsRepository _repository;
        private readonly IAgentsRepository _agentsRepository;
        private readonly IHttpClientFactory _clientFactory;

        /*public CpuMetricsController()
        {
            _logger = null;
        }*/

        public CpuMetricsController(ILogger<CpuMetricsController> logger, ICpuMetricsRepository repository, IAgentsRepository agentsRepository, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _repository = repository;
            _agentsRepository = agentsRepository;
            _clientFactory = clientFactory;

            _logger.LogInformation("NLog встроен в CpuMetricsController");
        }

        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public async Task<IActionResult> GetMetricsFromAgentAsync([FromRoute] int agentId, [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            _logger.LogInformation("Привет! Это наше первое сообщение в лог");
            /*int k = 0;
            _ = 2 / k;*/

            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:5001/api/metrics/cpu/from/2022.03.01/to/2022.04.05");
            request.Headers.Add("Accept", "application/vnd.github.v3+json");
            var client = _clientFactory.CreateClient();
            HttpResponseMessage response = client.Send(request);
            if (response.IsSuccessStatusCode)
            {
                var responseStream = response.Content.ReadAsStringAsync().Result.Trim('\\');
                var metricsResponse = System.Text.Json.JsonSerializer.Deserialize<MetricsApiResponse<CpuMetricDTO>>(responseStream, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }); 

                return Ok(metricsResponse);
            }
            else
            {
                // ошибка при получении ответа
            }

            return Ok();
        }

        [HttpGet("cluster/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAllCluster([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            return Ok();
        }

    }
}
