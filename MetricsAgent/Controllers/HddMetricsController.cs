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
    [Route("api/metrics/hdd")]
    [ApiController]
    public class HddMetricsController : ControllerBase
    {
        private readonly IHddMetricsRepository _repository;
        private readonly ILogger<HddMetricsController> _logger;
        private readonly IMapper _mapper;

        public HddMetricsController(ILogger<HddMetricsController> logger, IHddMetricsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;

            _logger.LogInformation("NLog встроен в HddMetricsController");
        }

        [HttpGet("from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent([FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            var metrics = _repository.GetMetricsOutPeriod(fromTime.ToUnixTimeSeconds(), toTime.ToUnixTimeSeconds());
            var response = new AllMetricsResponse<HddMetricDto>();

            foreach (var metric in metrics)
            {
                //response.Metrics.Add(_mapper.Map<HddMetricDto>(metric)); 

                response.Metrics.Add(new HddMetricDto { Time = DateTimeOffset.FromUnixTimeSeconds(metric.Time), Value = metric.Value });

            }

            return Ok(response);
        }
    }
}
