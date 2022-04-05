using MetricsAgent.DAL;
using MetricsAgent.DTO;
using Core;
using MetricsAgent.Models;
using MetricsAgent.Requests;
using MetricsAgent.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SQLite;
using AutoMapper;

namespace MetricsAgent.Controllers
{


    [Route("api/metrics/cpu")]
    [ApiController]
    public class CpuMetricsController : ControllerBase
    {
        private readonly ICpuMetricsRepository _repository;
        private readonly ILogger<CpuMetricsController> _logger;
        private readonly IMapper _mapper;

        //private readonly INotifierMediatorService _notifierMediatorService;


        public CpuMetricsController(ILogger<CpuMetricsController> logger, ICpuMetricsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
           // _notifierMediatorService = notifierMediatorService;

            _logger.LogInformation("NLog встроен в CpuMetricsController");
        }

        [HttpGet("from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent([FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            var metrics = _repository.GetMetricsOutPeriod(fromTime.ToUnixTimeSeconds(), toTime.ToUnixTimeSeconds());
            var response = new AllMetricsResponse<CpuMetricDto>();

            foreach (var metric in metrics)
            {
                response.Metrics.Add(new CpuMetricDto { Time = DateTimeOffset.FromUnixTimeSeconds(metric.Time), Value = metric.Value});
                _logger.LogInformation($"api/metrics/cpu/from/{fromTime}/to/{toTime}");
            }
            return Ok(response);
        }

    }
}
