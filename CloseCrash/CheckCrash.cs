using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CloseCrash
{
    public class CheckCrash
    {
        public static bool IsRuning;

        public static void CheckCrashGame(DataGridView dg)
        {
            LoadTabRuning(dg);
        }

        private static void LoadTabRuning(DataGridView dg)
        {
            ProcessGame.ListProcessGame.Clear();
            Process[] processes = Process.GetProcesses();
            foreach(Process process in processes)
            {
                if(process != null && process.ProcessName.ToLower().Contains("dragonboy"))
                {
                    PerformanceCounter pf1 = new PerformanceCounter("Process", "Working Set - Private", process.ProcessName);
                    ProcessGame.ListProcessGame.Add(new ProcessGame
                    {
                        PID = process.Id.ToString(),
                        NAMETAB = process.MainWindowTitle,
                        RAM = (pf1.NextValue() / (1024*1024)).ToString(),
                        TAB = process.ProcessName,
                    });
                }
            }
            if (ProcessGame.ListProcessGame.Count > 0)
            {
                int i = 0;
                dg.Rows.Clear();
                foreach (ProcessGame processGame in ProcessGame.ListProcessGame)
                {
                    DataGridViewRowCollection rows = dg.Rows;
                    rows.Add(new object[]
                    {
                        i,
                        processGame.TAB,
                        processGame.NAMETAB,
                        processGame.RAM,
                        processGame.PID,
                    });
                    i++;
                }
            }
        }
    }
}
