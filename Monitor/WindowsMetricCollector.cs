using System.IO;
using System.Linq;
using Hardware.Info;
using System.Diagnostics;
using System.Collections.Generic;

using ResourceMonitor.Models;

namespace ResourceMonitor.Monitor;

public class WindowsMetricCollector : IMetricCollector
{
    private readonly HardwareInfo _hardwareInfo;

    public WindowsMetricCollector()
    {
        _hardwareInfo = new HardwareInfo();
    }



    public SystemStat GetStats()
    {
        _hardwareInfo.RefreshCPUList();
        _hardwareInfo.RefreshMemoryStatus();
        _hardwareInfo.RefreshDriveList();

        double cpu_percent = 0.0;

        if (_hardwareInfo.CpuList.Count > 0)
        {
            cpu_percent = _hardwareInfo.CpuList.Average(c => (double)c.PercentProcessorTime);
        }


        double totalRAMBytes = _hardwareInfo.MemoryStatus.TotalPhysical;
        double freeRAMBytes = _hardwareInfo.MemoryStatus.AvailablePhysical;
        double usedRAMBytes = totalRAMBytes - freeRAMBytes;

        double ram_used_mb = usedRAMBytes / (1024 * 1024);

        double totalDiskUsedMB = 0.0;

        foreach(var drive in DriveInfo.GetDrives())
        {
            if(drive.IsReady)
            {
                double totalSpaceMB = drive.TotalSize / (1024 * 1024);
                double freeSpaceMB = drive.AvailableFreeSpace / ( 1024 * 1024);

                totalDiskUsedMB += (totalSpaceMB - freeSpaceMB);
            }

        }

        return new SystemStat
        {
            cpu = Math.Round(cpu_percent, 2),
            ram_used = Math.Round(ram_used_mb, 2),
            disk_used = Math.Round(totalDiskUsedMB, 2)
        };
    }
}