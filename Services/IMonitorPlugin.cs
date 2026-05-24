using ResourceMonitor.Models;

namespace ResourceMonitor.Services
{
    public interface IMonitorPlugin
    {
        string Name { get; }
        Task ExecuteAsync(SystemStat stat);
    }
}