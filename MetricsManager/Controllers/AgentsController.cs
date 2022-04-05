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

        /// <summary>
        /// Регистрация агента для опроса менеджером
        /// </summary>
        /// <param name="agentInfo"> Json поля AgentAddress (пример: https://localhost:7297 ) статус агента IsEnabled  - treu/false  </param>
        /// <returns></returns>
        [HttpPost("register")]
        public IActionResult RegisterAgent([FromBody] AgentInfo agentInfo)
        {

            _repository.Create(new AgentInfo() { AgentAddress = agentInfo.AgentAddress, IsEnabled = true });

            return Ok();
        }

        /// <summary>
        /// Заглушка
        /// </summary>
        /// <param name="agentId"></param>
        /// <returns></returns>
        [HttpPut("enable/{agentId}")]
        public IActionResult EnableAgentById([FromRoute] int agentId)
        {
            return Ok();
        }

        /// <summary>
        /// Заглушка
        /// </summary>
        /// <param name="agentId"></param>
        /// <returns></returns>
        [HttpPut("disable/{agentId}")]
        public IActionResult DisableAgentById([FromRoute] int agentId)
        {
            return Ok();
        }

        /// <summary>
        /// Возвращает список всех зарегистрированных агентов
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        public IActionResult GetAll()
        {
            return Ok(_repository.GetAll());
        }
    }
}

