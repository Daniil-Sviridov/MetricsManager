using MetricsManager.Models;
using MetricsManager.Requests;
using MetricsManager.Responses;

namespace MetricsManager.Client
{
    public interface IMetricsAgentClient
    {
        MetricsApiResponse<CpuMetricDTO> GetCpuMetrics(MetricsApiRequest request);
        MetricsApiResponse<DotNetMetricDTO> GetDotNetMetrics(MetricsApiRequest request);
        MetricsApiResponse<HddMetricDTO> GetHddMetrics(MetricsApiRequest request);
        MetricsApiResponse<NetworkMetricDTO> GetNetworkMetrics(MetricsApiRequest request);
        MetricsApiResponse<RamMetricDTO> GetRamMetrics(MetricsApiRequest request);
        
        
    }
}