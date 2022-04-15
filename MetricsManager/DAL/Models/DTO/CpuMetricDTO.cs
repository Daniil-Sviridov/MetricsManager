namespace MetricsManager.Models
{
    public class CpuMetricDTO
    {
        public int AgentId { get; set; }

        public int Value { get; set; }

        public DateTimeOffset Time { get; set; }
    }

}
