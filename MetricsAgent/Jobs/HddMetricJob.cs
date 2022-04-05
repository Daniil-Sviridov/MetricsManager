using MetricsAgent.DAL;
using Quartz;
using System.Diagnostics;

namespace MetricsAgent.Jobs
{
    public class HddMetricJob : IJob
    {
        private IHddMetricsRepository _repository;
        // Счётчик для метрики 
        private PerformanceCounter _hddCounter;

        public HddMetricJob(IHddMetricsRepository repository)
        {
            _repository = repository;
            _hddCounter = new PerformanceCounter("PhysicalDisk", "Disk Bytes/sec", "_Total");
        }
        public Task Execute(IJobExecutionContext context)
        {
            // Получаем значение
            var cpuUsageInPercents = Convert.ToInt32(_hddCounter.NextValue());
            // Узнаем, когда мы сняли значение метрики
            var time = DateTimeOffset.Now.ToUnixTimeSeconds();
            // Теперь можно записать что-то посредством репозитория
            _repository.Create(new Models.HddMetric
            {
                Time = time,
                Value = cpuUsageInPercents
            });

            return Task.CompletedTask;
        }

    }
}
