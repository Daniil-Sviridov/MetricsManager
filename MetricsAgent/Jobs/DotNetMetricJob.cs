using MetricsAgent.DAL;
using Quartz;
using System.Diagnostics;

namespace MetricsAgent.Jobs
{
    public class DotNetMetricJob : IJob
    {
        private IDotNetMetricsRepository _repository;
        // Счётчик для метрики 
        private PerformanceCounter _dotnetCounter;

        public DotNetMetricJob(IDotNetMetricsRepository repository)
        {
            _repository = repository;
            _dotnetCounter = new PerformanceCounter(".NET CLR Memory", "Gen 0 heap size", "_Global_");
        }
        public Task Execute(IJobExecutionContext context)
        {
            // Получаем значение 
            var cpuUsageInPercents = Convert.ToInt32(_dotnetCounter.NextValue());
            // Узнаем, когда мы сняли значение метрики
            var time = DateTimeOffset.Now.ToUnixTimeSeconds();
            // Теперь можно записать что-то посредством репозитория
            _repository.Create(new Models.DotNetMetric
            {
                Time = time,
                Value = cpuUsageInPercents
            });
            return Task.CompletedTask;
        }

    }
}
