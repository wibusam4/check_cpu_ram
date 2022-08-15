using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CloseCrash
{
    class Computer
    {
        public static int PercentRam { get; set; }
        public static int PercentCpu { get; set; }
        public static int PercentDisk { get; set; }

        public static void Computed()
        {
            GetPercentRam();
        }

        public static void GetPercentRam()
        {
            ManagementObjectSearcher wmiObject = new ManagementObjectSearcher("select * from Win32_OperatingSystem");
            var memoryValues = wmiObject.Get().Cast<ManagementObject>().Select(mo => new {
                FreePhysicalMemory = Double.Parse(mo["FreePhysicalMemory"].ToString()),
                TotalVisibleMemorySize = Double.Parse(mo["TotalVisibleMemorySize"].ToString()),
                MaxNumberOfProcesses = Double.Parse(mo["MaxNumberOfProcesses"].ToString()),
                MaxProcessMemorySize = Double.Parse(mo["MaxProcessMemorySize"].ToString()),
            }).FirstOrDefault();

            if (memoryValues != null)
            {
                var percentRam = ((memoryValues.TotalVisibleMemorySize - memoryValues.FreePhysicalMemory) / memoryValues.TotalVisibleMemorySize) * 100;
                var percentCpu =  memoryValues.MaxNumberOfProcesses / memoryValues.MaxProcessMemorySize * 100;
                ;
            }
        }
    }
}
