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
    public class CpuMetricJob : IJob
    {
        private readonly ICpuMetricsRepository _repository;
        private readonly IAgentsRepository _agentsRepository;
        private readonly IMetricsAgentClient _metricsAgentClient;


        public CpuMetricJob(ICpuMetricsRepository repository, IAgentsRepository agentsRepository, IMetricsAgentClient metricsAgentClient)
        {
            _repository = repository;
            _agentsRepository = agentsRepository;
            _metricsAgentClient = metricsAgentClient;
        }
        public Task Execute(IJobExecutionContext context)
        {
            var agents = _agentsRepository.GetAll();
            // Получаем значение 
            foreach (AgentInfo agent in agents)
            {

                MetricsApiResponse<CpuMetric>  respMetrics = _metricsAgentClient.GetCpuMetrics(new MetricsApiRequest() { AgentUrl = agent.AgentAddress, FromTime = DateTimeOffset.MinValue, ToTime = DateTimeOffset.UtcNow});
                foreach (var metric in respMetrics.Metrics)
                {
                    _repository.Create(new Models.CpuMetric
                    {
                        Time = metric.Time,
                        Value = metric.Value,
                        Id = metric.Id,
                        AgentId = agent.Id
                    });
                }
                
            }

             /*   // Узнаем, когда мы сняли значение метрики
                var time =
                TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            // Теперь можно записать что-то посредством репозитория
            _repository.Create(new Models.CpuMetric
            {
                Time = time,
                Value = cpuUsageInPercents
            });*/
            return Task.CompletedTask;
        }

    }
}
