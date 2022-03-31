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
        public IActionResult GetMetricsFromAgent([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            var metrics = _repository.GetMetricsOutPeriod(fromTime, toTime);
            var response = new AllMetricsResponse<CpuMetricDto>();

            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<CpuMetricDto>(metric));
            }
            return Ok(response);
        }

        /*  [HttpPost("create")]
          public IActionResult Create([FromBody] CpuMetricCreateRequest request)
          {
              _repository.Create(new CpuMetric
              {
                  Time = request.Time,
                  Value = request.Value
              });

              _logger.LogInformation("CpuMetrics.create");

              return Ok();
          }

          [HttpDelete("delete")]
          public IActionResult Delete([FromQuery] int id)
          {
              _repository.Delete(id);
              return Ok();
          }

          [HttpPut("put")]
          public IActionResult Update([FromBody] CpuMetric item)
          {
              _repository.Update(item);
              return Ok();
          }

          [HttpGet("all")]
          public IActionResult GetAll()
          {
              IList<CpuMetric> metrics = _repository.GetAll();

              var response = new AllMetricsResponse<CpuMetricDto>()
              {
                  Metrics = new List<CpuMetricDto>()
              };

              foreach (var metric in metrics)
              {
                  // response.Metrics.Add(new CpuMetricDto { Time = metric.Time, Value = metric.Value, Id = metric.Id });
                  response.Metrics.Add(_mapper.Map<CpuMetricDto>(metric));

              }

              return Ok(response);
          }

          [HttpGet("get")]
          public IActionResult Get([FromQuery] int id)
          {
              var metric = _mapper.Map<CpuMetricDto>(_repository.GetById(id));

              return Ok(metric);
          }*/

    }
}
