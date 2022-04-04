using MetricsManager.Responses;
using MetricsManager.Requests;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text.Json;
using MetricsManager.Models;

namespace MetricsManager.Client
{
    public class MetricsAgentClient : IMetricsAgentClient
    {
        private readonly HttpClient _httpClient;
        //private readonly ILogger _logger;
        public MetricsAgentClient(HttpClient httpClient) //, ILogger logger
        {
            _httpClient = httpClient;
            // _logger = logger;
        }

        public MetricsApiResponse<CpuMetric> GetCpuMetrics(MetricsApiRequest request)
        {
            var fromParameter = request.FromTime;
            var toParameter = request.ToTime;

            var sr = $"{request.AgentUrl}/api/metrics/cpu/from/{fromParameter}/to/{toParameter}";

            var httpRequest = new HttpRequestMessage(HttpMethod.Get, sr);
            httpRequest.Headers.Add("Accept", "application/vnd.github.v3+json");


            try
            {
                HttpResponseMessage response = _httpClient.Send(httpRequest);
                if (response.IsSuccessStatusCode)
                {
                    var responseStream = response.Content.ReadAsStream();
                     var responseJSON = JsonSerializer.Deserialize<MetricsApiResponse<CpuMetric>>(responseStream);

                    return responseJSON;
                }
                int a = 1;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message);
                return null;
            }

            return null;
        }

        public MetricsApiResponse<DotNetMetric> GetDonNetMetrics(MetricsApiRequest request)
        {
            throw new NotImplementedException();
        }

        public MetricsApiResponse<HddMetric> GetHddMetrics(MetricsApiRequest request)
        {
            throw new NotImplementedException();
        }

        public MetricsApiResponse<NetworkMetric> GetNetworkMetrics(MetricsApiRequest request)
        {
            throw new NotImplementedException();
        }

        public MetricsApiResponse<RamMetric> GetRamMetrics(MetricsApiRequest request)
        {
            throw new NotImplementedException();
        }
    }
}

