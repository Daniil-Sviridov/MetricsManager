using System;
using System.Collections.Generic;

namespace MetricsManager.Responses
{
    public class MetricsApiResponse<T> where T : class
    {
        public List<T> Metrics { get; set; }

        public MetricsApiResponse()
        {
            Metrics = new List<T>();
        }

    }

}