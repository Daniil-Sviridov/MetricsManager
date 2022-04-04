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
    [Route("api/metrics/dotnet")]
    [ApiController]
    public class DotNetMetricsController : ControllerBase
    {
        private readonly IDotNetMetricsRepository _repository;
        private readonly ILogger<DotNetMetricsController> _logger;
        private readonly IMapper _mapper;

        public DotNetMetricsController(ILogger<DotNetMetricsController> logger, IDotNetMetricsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;

            _logger.LogInformation("NLog встроен в DotNetMetricsController");
        }

        [HttpGet("from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent([FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            var metrics = _repository.GetMetricsOutPeriod(fromTime.ToUnixTimeSeconds(), toTime.ToUnixTimeSeconds());
            var response = new AllMetricsResponse<DotNetMetricDto>();

            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<DotNetMetricDto>(metric));
            }

            return Ok(response);
        }

        /* [HttpPost("create")]
         public IActionResult Create([FromBody] DotNetMetricCreateRequest request)
         {
             _repository.Create(new DotNetMetric
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
         public IActionResult Update([FromBody] DotNetMetric item)
         {
             _repository.Update(item);
             return Ok();
         }

         [HttpGet("all")]
         public IActionResult GetAll()
         {
             var metrics = _repository.GetAll();

             var response = new AllMetricsResponse<DotNetMetricDto>()
             {
                 Metrics = new List<DotNetMetricDto>()
             };

             foreach (var metric in metrics)
             {
                 response.Metrics.Add(_mapper.Map<DotNetMetricDto>(metric));
             }

             return Ok(response);
         }

         [HttpGet("get")]
         public IActionResult Get([FromQuery] int id)
         {
             var metric = _mapper.Map<DotNetMetricDto>(_repository.GetById(id));

             return Ok(metric);
         }

    */
    }
}
