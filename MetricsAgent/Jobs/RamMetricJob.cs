using MetricsAgent.DAL;
using Quartz;
using System.Diagnostics;

namespace MetricsAgent.Jobs
{
    public class RamMetricJob : IJob
    {
        private IRamMetricsRepository _repository;
        // Счётчик для метрики
        private PerformanceCounter _ramCounter;

        public RamMetricJob(IRamMetricsRepository repository)
        {
            _repository = repository;
            _ramCounter = new PerformanceCounter("Memory", "Available MBytes");
        }
        public Task Execute(IJobExecutionContext context)
        {
            // Получаем значение 
            var cpuUsageInPercents = Convert.ToInt32(_ramCounter.NextValue());
            // Узнаем, когда мы сняли значение метрики
            var time = DateTimeOffset.Now.ToUnixTimeSeconds();
            // Теперь можно записать что-то посредством репозитория
            _repository.Create(new Models.RamMetric
            {
                Time = time,
                Value = cpuUsageInPercents
            });
            return Task.CompletedTask;
        }

    }
}
