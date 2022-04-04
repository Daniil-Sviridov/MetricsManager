using AutoMapper;
using MetricsAgent.DAL;
using MetricsAgent.DTO;
using MetricsAgent.Models;
using MetricsAgent.Requests;
using MetricsAgent.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/network")]
    [ApiController]
    public class NetworkMetricsController : ControllerBase
    {
        private readonly INetworkMetricsRepository _repository;
        private readonly ILogger<NetworkMetricsController> _logger;
        private readonly IMapper _mapper;

        public NetworkMetricsController(ILogger<NetworkMetricsController> logger, INetworkMetricsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;

            _logger.LogInformation("NLog встроен в NetworkMetricsController");
        }

        [HttpGet("from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent([FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            var metrics = _repository.GetMetricsOutPeriod(fromTime.ToUnixTimeSeconds(), toTime.ToUnixTimeSeconds());
            var response = new AllMetricsResponse<NetworkMetricDto>();

            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<NetworkMetricDto>(metric));
            }

            return Ok(response);
        }
        /*    [HttpPost("create")]
            public IActionResult Create([FromBody] NetworkMetricCreateRequest request)
            {
                _repository.Create(new NetworkMetric
                {
                    Time = request.Time,
                    Value = request.Value
                });

                return Ok();
            }

            [HttpDelete("delete")]
            public IActionResult Delete([FromQuery] int id)
            {
                _repository.Delete(id);
                return Ok();
            }

            [HttpPut("put")]
            public IActionResult Update([FromBody] NetworkMetric item)
            {
                _repository.Update(item);
                return Ok();
            }

            [HttpGet("all")]
            public IActionResult GetAll()
            {
                var metrics = _repository.GetAll();

                var response = new AllMetricsResponse<NetworkMetricDto>()
                {
                    Metrics = new List<NetworkMetricDto>()
                };

                foreach (var metric in metrics)
                {
                    response.Metrics.Add(_mapper.Map<NetworkMetricDto>(metric));
                }

                return Ok(response);
            }

            [HttpGet("get")]
            public IActionResult Get([FromQuery] int id)
            {
                var metric = _repository.GetById(id);

                return Ok(metric);
            }
    */
    }
}
