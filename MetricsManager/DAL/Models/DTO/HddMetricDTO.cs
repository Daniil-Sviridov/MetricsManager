namespace MetricsManager.Models
{
    public class HddMetricDTO
    {
        public int AgentId { get; set; }

        public int Value { get; set; }

        public DateTimeOffset Time { get; set; }
    }

}
