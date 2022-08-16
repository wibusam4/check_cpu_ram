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
    class Computer
    {
        public static int PercentRam { get; set; }
        public static int PercentCpu { get; set; }
        public static int PercentDisk { get; set; }
        public static Thread GetRam;
        public static Thread GetCpu;

        public static void Computed()
        {
            GetPercentRam();
            GetPercentCpu();
        }

        private static void GetPercentRam()
        {
            GetRam = new Thread(() =>
            {
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
                    GetRam.Abort();
                }
            });
            GetRam.IsBackground = true;
            GetRam.Start();
        }

        private static void GetPercentCpu()
        {
            GetCpu = new Thread(() =>
            {
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
                            GetCpu.Abort();
                            break;
                        }
                        catch (Exception ex)
                        {
                            break;
                        }
                    }
                }
            });
            GetCpu.IsBackground = true;
            GetCpu.Start();
        }
    }
}
