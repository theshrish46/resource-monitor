using Hardware.Info;

using ResourceMonitor.Models;


namespace ResourceMonitor.Monitor
{
    public interface IMetricCollector
    {
        SystemStat GetStats();
    }
}