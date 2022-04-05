using MetricsManager.DAL.Repositories;
using MetricsManager.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MetricsManager.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AgentsController : ControllerBase
    {
        private readonly IAgentsRepository _repository;

        public AgentsController(IAgentsRepository repository)
        {

            _repository = repository;
        }

        [HttpPost("register")]
        public IActionResult RegisterAgent([FromBody] AgentInfo agentInfo)
        {

            _repository.Create(new AgentInfo() { AgentAddress = agentInfo.AgentAddress, IsEnabled = true });

            return Ok();
        }

        [HttpPut("enable/{agentId}")]
        public IActionResult EnableAgentById([FromRoute] int agentId)
        {
            return Ok();
        }

        [HttpPut("disable/{agentId}")]
        public IActionResult DisableAgentById([FromRoute] int agentId)
        {
            return Ok();
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            return Ok(_repository.GetAll());
        }
    }
}

