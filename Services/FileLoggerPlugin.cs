using System.IO;
using ResourceMonitor.Models;

namespace ResourceMonitor.Services;

public class FileLoggerPlugin : IMonitorPlugin
{
    public string Name => "Local File Logger";

    private readonly string _logDirectory;
    private readonly string _logFile;

    public FileLoggerPlugin()
    {
        _logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");

        if (!Directory.Exists(_logDirectory))
        {
            Directory.CreateDirectory(_logDirectory);
        }
    }

    public async Task ExecuteAsync(SystemStat stats)
    {

        try
        {
            string dateStamp = DateTime.Now.ToString("yyyy-MM-dd");
            string fileName = $"metrics_{dateStamp}.log";
            string fullPath = Path.Combine(_logDirectory, fileName);

            string logLine = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] CPU: {stats.cpu}% | RAM: {stats.ram_used} MB | Disk: {stats.disk_used} MB{Environment.NewLine}";

            using (var stream = new FileStream(fullPath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite, bufferSize: 4096, useAsync: true))
            using (var writer = new StreamWriter(stream))
            {
                await writer.WriteAsync(logLine);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[File Logger Error] Failed to write telemetry to disk: {ex.Message}");
        }
    }
}