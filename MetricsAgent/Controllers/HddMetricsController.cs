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

        [HttpGet("left/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            var metrics = _repository.GetMetricsOutPeriod(fromTime, toTime);
            var response = new AllMetricsResponse<HddMetricDto>();

            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<HddMetricDto>(metric));
            }

            return Ok(response);
        }

        /*     [HttpPost("create")]
             public IActionResult Create([FromBody] HddMetricCreateRequest request)
             {
                 _repository.Create(new HddMetric
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
             public IActionResult Update([FromBody] HddMetric item)
             {
                 _repository.Update(item);
                 return Ok();
             }

             [HttpGet("all")]
             public IActionResult GetAll()
             {
                 var metrics = _repository.GetAll();

                 var response = new AllMetricsResponse<HddMetricDto>()
                 {
                     Metrics = new List<HddMetricDto>()
                 };

                 foreach (var metric in metrics)
                 {
                     response.Metrics.Add(_mapper.Map<HddMetricDto>(metric));
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
