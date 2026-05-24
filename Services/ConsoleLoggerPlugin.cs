using System;
using System.IO;

using ResourceMonitor.Models;

namespace ResourceMonitor.Services
{
    public class ConsoleLoggerPlugin: IMonitorPlugin
    {
        public string Name => "Terminal Console Monitor";

        public Task ExecuteAsync(SystemStat stats)
        {
            Console.WriteLine($"=== [{DateTime.Now:HH:mm:ss}] Telemetry Update ===");
            Console.WriteLine($"CPU Usage:  {stats.cpu:F2} %");
            Console.WriteLine($"RAM Used:   {stats.ram_used:F2} MB");
            Console.WriteLine($"Disk Used:  {stats.disk_used:F2} MB");
            Console.WriteLine("=====================================\n");

            return Task.CompletedTask;
        }
    }
}