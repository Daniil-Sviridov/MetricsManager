using MetricsManager.Models;
using MetricsManager.Requests;
using MetricsManager.Responses;

namespace MetricsManager.Client
{
    public interface IMetricsAgentClient
    {
        MetricsApiResponse<CpuMetric> GetCpuMetrics(MetricsApiRequest request);
        MetricsApiResponse<DotNetMetric> GetDonNetMetrics(MetricsApiRequest request);
        MetricsApiResponse<HddMetric> GetHddMetrics(MetricsApiRequest request);
        MetricsApiResponse<NetworkMetric> GetNetworkMetrics(MetricsApiRequest request);
        MetricsApiResponse<RamMetric> GetRamMetrics(MetricsApiRequest request);
        
        
    }
}