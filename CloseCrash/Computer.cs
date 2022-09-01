using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
namespace CloseCrash
{
    public class Computer
    {
        public static int PercentRam { get; set; }
        public static int PercentCpu { get; set; }
        public static int PercentDisk { get; set; }
        public static Thread GetRam;
        public static Thread GetCpu;
        private static bool IsGetingRam;
        private static bool IsGetingCpu;

        public static void Computed()
        {
            GetPercentRam();
            GetPercentCpu();
        }

        private static void GetPercentRam()
        {
            if (IsGetingRam)
            {
                return;
            }
            GetRam = new Thread(() =>
            {
                IsGetingRam = true;
                ManagementObjectSearcher wmiObject = new ManagementObjectSearcher("select * from Win32_OperatingSystem");
                var memoryValues = wmiObject.Get().Cast<ManagementObject>().Select(mo => new
                {
                    FreePhysicalMemory = Double.Parse(mo["FreePhysicalMemory"].ToString()),
                    TotalVisibleMemorySize = Double.Parse(mo["TotalVisibleMemorySize"].ToString()),
                }).FirstOrDefault();

                if (memoryValues != null)
                {
                    var percentRam = ((memoryValues.TotalVisibleMemorySize - memoryValues.FreePhysicalMemory) / memoryValues.TotalVisibleMemorySize) * 100;
                    PercentRam = (int)percentRam;
                }
                IsGetingRam = false;
            });
            GetRam.IsBackground = true;
            GetRam.Start();
        }

        private static void GetPercentCpu()
        {
            if (IsGetingCpu)
            {
                return;
            }
            GetCpu = new Thread(() =>
            {
                IsGetingCpu = true;
                ObjectQuery objQuery = new ObjectQuery("SELECT * FROM Win32_PerfFormattedData_PerfOS_Processor WHERE Name=\"_Total\"");
                ManagementObjectSearcher mngObjSearch = new ManagementObjectSearcher(objQuery);
                ManagementObjectCollection mngObjColl = mngObjSearch.Get();
                if (mngObjColl.Count > 0)
                {
                    foreach (ManagementObject mngObject in mngObjColl)
                    {
                        try
                        {
                            uint cpu_usage = 100 - Convert.ToUInt32(mngObject["PercentIdleTime"]);
                            PercentCpu = (int)cpu_usage;
                            break;
                        }
                        catch (Exception ex)
                        {
                            break;
                        }
                    }
                }
                IsGetingCpu = false;
            });
            GetCpu.IsBackground = true;
            GetCpu.Start();
        }
    }
}
