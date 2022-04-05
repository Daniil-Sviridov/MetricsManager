using MetricsManager.Responses;
using MetricsManager.Requests;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text.Json;
using MetricsManager.Models;
using System.Globalization;

namespace MetricsManager.Client
{
    public class MetricsAgentClient : IMetricsAgentClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;
        public MetricsAgentClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public MetricsApiResponse<CpuMetricDTO> GetCpuMetrics(MetricsApiRequest request)
        {
            DateTimeFormatInfo myDTFI = new CultureInfo("en-US", false).DateTimeFormat;

            var fromParameter = request.FromTime.ToString(myDTFI.SortableDateTimePattern);
            var toParameter = request.ToTime.ToString(myDTFI.SortableDateTimePattern);

            //https://localhost:5001/api/metrics/cpu/from/2022-04-05T14:00:45+000/to/2022-04-05T17:42:01+000

            var sr = $"{request.AgentUrl}/api/metrics/cpu/from/{fromParameter}+00:00/to/{toParameter}+00:00";

            var httpRequest = new HttpRequestMessage(HttpMethod.Get, sr);
            httpRequest.Headers.Add("Accept", "application/vnd.github.v3+json");

            try
            {
                HttpResponseMessage response = _httpClient.Send(httpRequest);
                if (response.IsSuccessStatusCode)
                {
                    var responseString = response.Content.ReadAsStringAsync().Result;
                    var deserializeJSON = JsonSerializer.Deserialize<MetricsApiResponse<CpuMetricDTO>>(responseString, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

                    return deserializeJSON is null ? new MetricsApiResponse<CpuMetricDTO>() : deserializeJSON;
                }

            }
            catch (Exception ex)
            {
                //  _logger.LogError(ex.Message);
            }

            return new MetricsApiResponse<CpuMetricDTO>();
        }

        public MetricsApiResponse<DotNetMetricDTO> GetDotNetMetrics(MetricsApiRequest request)
        {
            DateTimeFormatInfo myDTFI = new CultureInfo("en-US", false).DateTimeFormat;

            var fromParameter = request.FromTime.ToString(myDTFI.SortableDateTimePattern);
            var toParameter = request.ToTime.ToString(myDTFI.SortableDateTimePattern);

            //https://localhost:5001/api/metrics/cpu/from/2022-04-05T14:00:45+000/to/2022-04-05T17:42:01+000

            var sr = $"{request.AgentUrl}/api/metrics/dotnet/from/{fromParameter}+00:00/to/{toParameter}+00:00";

            var httpRequest = new HttpRequestMessage(HttpMethod.Get, sr);
            httpRequest.Headers.Add("Accept", "application/vnd.github.v3+json");

            try
            {
                HttpResponseMessage response = _httpClient.Send(httpRequest);
                if (response.IsSuccessStatusCode)
                {
                    var responseString = response.Content.ReadAsStringAsync().Result;
                    var deserializeJSON = JsonSerializer.Deserialize<MetricsApiResponse<DotNetMetricDTO>>(responseString, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

                    return deserializeJSON is null ? new MetricsApiResponse<DotNetMetricDTO>() : deserializeJSON;
                }

            }
            catch (Exception ex)
            {
                //  _logger.LogError(ex.Message);
            }

            return new MetricsApiResponse<DotNetMetricDTO>();
        }

        public MetricsApiResponse<HddMetricDTO> GetHddMetrics(MetricsApiRequest request)
        {
            DateTimeFormatInfo myDTFI = new CultureInfo("en-US", false).DateTimeFormat;

            var fromParameter = request.FromTime.ToString(myDTFI.SortableDateTimePattern);
            var toParameter = request.ToTime.ToString(myDTFI.SortableDateTimePattern);

            //https://localhost:5001/api/metrics/cpu/from/2022-04-05T14:00:45+000/to/2022-04-05T17:42:01+000

            var sr = $"{request.AgentUrl}/api/metrics/hdd/from/{fromParameter}+00:00/to/{toParameter}+00:00";

            var httpRequest = new HttpRequestMessage(HttpMethod.Get, sr);
            httpRequest.Headers.Add("Accept", "application/vnd.github.v3+json");

            try
            {
                HttpResponseMessage response = _httpClient.Send(httpRequest);
                if (response.IsSuccessStatusCode)
                {
                    var responseString = response.Content.ReadAsStringAsync().Result;
                    var deserializeJSON = JsonSerializer.Deserialize<MetricsApiResponse<HddMetricDTO>>(responseString, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

                    return deserializeJSON is null ? new MetricsApiResponse<HddMetricDTO>() : deserializeJSON;
                }

            }
            catch (Exception ex)
            {
                //  _logger.LogError(ex.Message);
            }

            return new MetricsApiResponse<HddMetricDTO>();
        }

        public MetricsApiResponse<NetworkMetricDTO> GetNetworkMetrics(MetricsApiRequest request)
        {
            DateTimeFormatInfo myDTFI = new CultureInfo("en-US", false).DateTimeFormat;

            var fromParameter = request.FromTime.ToString(myDTFI.SortableDateTimePattern);
            var toParameter = request.ToTime.ToString(myDTFI.SortableDateTimePattern);

            //https://localhost:5001/api/metrics/cpu/from/2022-04-05T14:00:45+000/to/2022-04-05T17:42:01+000

            var sr = $"{request.AgentUrl}/api/metrics/network/from/{fromParameter}+00:00/to/{toParameter}+00:00";

            var httpRequest = new HttpRequestMessage(HttpMethod.Get, sr);
            httpRequest.Headers.Add("Accept", "application/vnd.github.v3+json");

            try
            {
                HttpResponseMessage response = _httpClient.Send(httpRequest);
                if (response.IsSuccessStatusCode)
                {
                    var responseString = response.Content.ReadAsStringAsync().Result;
                    var deserializeJSON = JsonSerializer.Deserialize<MetricsApiResponse<NetworkMetricDTO>>(responseString, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

                    return deserializeJSON is null ? new MetricsApiResponse<NetworkMetricDTO>() : deserializeJSON;
                }

            }
            catch (Exception ex)
            {
                //  _logger.LogError(ex.Message);
            }

            return new MetricsApiResponse<NetworkMetricDTO>();
        }

        public MetricsApiResponse<RamMetricDTO> GetRamMetrics(MetricsApiRequest request)
        {
            DateTimeFormatInfo myDTFI = new CultureInfo("en-US", false).DateTimeFormat;

            var fromParameter = request.FromTime.ToString(myDTFI.SortableDateTimePattern);
            var toParameter = request.ToTime.ToString(myDTFI.SortableDateTimePattern);

            //https://localhost:5001/api/metrics/cpu/from/2022-04-05T14:00:45+000/to/2022-04-05T17:42:01+000

            var sr = $"{request.AgentUrl}/api/metrics/ram/from/{fromParameter}+00:00/to/{toParameter}+00:00";

            var httpRequest = new HttpRequestMessage(HttpMethod.Get, sr);
            httpRequest.Headers.Add("Accept", "application/vnd.github.v3+json");

            try
            {
                HttpResponseMessage response = _httpClient.Send(httpRequest);
                if (response.IsSuccessStatusCode)
                {
                    var responseString = response.Content.ReadAsStringAsync().Result;
                    var deserializeJSON = JsonSerializer.Deserialize<MetricsApiResponse<RamMetricDTO>>(responseString, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

                    return deserializeJSON is null ? new MetricsApiResponse<RamMetricDTO>() : deserializeJSON;
                }

            }
            catch (Exception ex)
            {
                //  _logger.LogError(ex.Message);
            }

            return new MetricsApiResponse<RamMetricDTO>();
        }
    }
}

