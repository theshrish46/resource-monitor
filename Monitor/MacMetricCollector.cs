using System;
using ResourceMonitor.Models;

namespace ResourceMonitor.Monitor;

public class MacMetricCollector : IMetricCollector
{
    public SystemStat GetStats()
    {
        throw new NotImplementedException("Mac metric collection is not yet implemented. Primary platform is Windows.");
    }
}