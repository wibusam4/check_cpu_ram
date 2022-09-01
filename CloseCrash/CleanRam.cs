using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CloseCrash
{
    public class CleanRam
    {
        public static bool IsRuning;
        public int DelayCleanRam { get; set; }
        public static CleanRam InStance;
        [DllImport("KERNEL32.DLL", EntryPoint = "SetProcessWorkingSetSize", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        internal static extern bool SetProcessWorkingSetSize32Bit (IntPtr pProcess, int dwMinimumWorkingSetSize, int dwMaximumWorkingSetSize);
        [DllImport("KERNEL32.DLL", EntryPoint = "SetProcessWorkingSetSize", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        internal static extern bool SetProcessWorkingSetSize64Bit (IntPtr pProcess, long dwMinimumWorkingSetSize, long dwMaximumWorkingSetSize);

        public void DoCleanRam(int delay)
        {
            if (DelayCleanRam > 0)
            {
                DelayCleanRam--;
                return;
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                SetProcessWorkingSetSize32Bit(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
            }
            DelayCleanRam = delay;
        }

        public static CleanRam _InStance()
        {
            if(InStance == null)
            {
                InStance = new CleanRam();
            }
            return InStance;
        }
    }
}
