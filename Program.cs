using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using ResourceMonitor.Models;
using ResourceMonitor.Monitor;
using ResourceMonitor.Services;

namespace ResourceMonitor;

public class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("=============== STARTING SYSTEM MONITOR SERVICE ===============");

        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var services = new ServiceCollection()
            .AddSingleton<IConfiguration>(configuration)
            .AddSingleton<IMonitorPlugin, FileLoggerPlugin>()
            .AddSingleton<IMonitorPlugin, ConsoleLoggerPlugin>();

        if (OperatingSystem.IsWindows())
        {
            services.AddSingleton<IMetricCollector, WindowsMetricCollector>();
        }
        else if (OperatingSystem.IsLinux())
        {
            services.AddSingleton<IMetricCollector, LinuxMetricCollector>();
        }
        else if (OperatingSystem.IsMacOS())
        {
            services.AddSingleton<IMetricCollector, MacMetricCollector>();
        }
        else
        {
            services.AddSingleton<IMetricCollector, WindowsMetricCollector>();
        }

        var serviceProvider = services.BuildServiceProvider();

        IMetricCollector collector = serviceProvider.GetRequiredService<IMetricCollector>();
        var plugins = new List<IMonitorPlugin>(serviceProvider.GetServices<IMonitorPlugin>());

        Console.WriteLine("Monitoring Started. Press Ctrl+C to exit.");

        int currentInterval = GetSafeInterval(configuration);
        string currentEndpoint = GetSafeEndpoint(configuration);

        var httpSender = new HttpSenderPlugin(currentEndpoint);
        plugins.Add(httpSender);

        while (true)
        {
            try
            {
                int freshInterval = GetSafeInterval(configuration);
                string freshEndpoint = GetSafeEndpoint(configuration);

                if (freshEndpoint != currentEndpoint)
                {
                    Console.WriteLine($"[Config] API Endpoint updated to: {freshEndpoint}");
                    plugins.Remove(httpSender);
                    httpSender = new HttpSenderPlugin(freshEndpoint);
                    plugins.Add(httpSender);
                    currentEndpoint = freshEndpoint;
                }

                if (freshInterval != currentInterval)
                {
                    Console.WriteLine($"[Config] Monitoring Interval updated from {currentInterval}s to {freshInterval}s");
                    currentInterval = freshInterval;
                }

                SystemStat stats = collector.GetStats();

                List<Task> pluginTasks = new List<Task>();
                foreach (IMonitorPlugin plugin in plugins)
                {
                    pluginTasks.Add(plugin.ExecuteAsync(stats));
                }

                await Task.WhenAll(pluginTasks);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ Error ] An error occurred during execution: {ex.Message}");
            }

            await Task.Delay(TimeSpan.FromSeconds(currentInterval));
        }
    }

    private static int GetSafeInterval(IConfiguration config)
    {
        string rawValue = config["MonitorSettings:IntervalSeconds"];
        if (int.TryParse(rawValue, out int interval) && interval > 0)
        {
            return interval;
        }
        return 5;
    }

    private static string GetSafeEndpoint(IConfiguration config)
    {
        return config["MonitorSettings:APIEndpoint"] ?? "https://webhook.site/4d855113-3b4e-47ce-85b5-0e9c5521ad44";
    }
}