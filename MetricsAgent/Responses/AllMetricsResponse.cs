using System;
using System.Collections.Generic;

namespace MetricsAgent.Responses
{
    public class AllMetricsResponse<T> where T : class
    {
        public List<T> Metrics { get; set; }

        public AllMetricsResponse()
        {
            Metrics = new List<T>();
        }

    }

}