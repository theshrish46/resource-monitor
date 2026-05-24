using System;

namespace ResourceMonitor.Models
{
    public class SystemStat
    {
        public double cpu {  get; set; }

        public double ram_used { get; set; }

        public double disk_used { get; set; }
    }
}