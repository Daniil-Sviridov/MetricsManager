namespace MetricsManager.Model
{
    public class AgentInfo
    {
        public int Id { get; set; }
        public  Uri? AgentAddress { get; set; }
        public bool IsEnabled { get; set; }
    }
}
