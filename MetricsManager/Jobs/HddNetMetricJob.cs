using MetricsManager.Client;
using MetricsManager.DAL.Repositories;
using MetricsManager.Model;
using MetricsManager.Models;
using MetricsManager.Requests;
using MetricsManager.Responses;
using Quartz;
using System.Diagnostics;

namespace MetricsManager.Jobs
{
    public class HddMetricJob : IJob
    {
        private readonly IHddMetricsRepository _repository;
        private readonly IAgentsRepository _agentsRepository;
        private readonly IMetricsAgentClient _metricsAgentClient;


        public HddMetricJob(IHddMetricsRepository repository, IAgentsRepository agentsRepository, IMetricsAgentClient metricsAgentClient)
        {
            _repository = repository;
            _agentsRepository = agentsRepository;
            _metricsAgentClient = metricsAgentClient;
        }
        public Task Execute(IJobExecutionContext context)
        {
            var agents = _agentsRepository.GetAll();

            foreach (AgentInfo agent in agents)
            {
                var minDate = _repository.GetMaxDate(agent.Id);

                MetricsApiResponse<HddMetricDTO> respMetrics = _metricsAgentClient.GetHddMetrics(new MetricsApiRequest() { AgentUrl = agent.AgentAddress, FromTime = minDate, ToTime = DateTimeOffset.Now });
                foreach (var metric in respMetrics.Metrics)
                {
                    _repository.Create(new Models.HddMetric
                    {
                        Time = metric.Time.ToUnixTimeSeconds(),
                        Value = metric.Value,
                        AgentId = agent.Id
                    });
                }

            }
            return Task.CompletedTask;
        }

    }
}
