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
        public static int CountTabCrash;

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
                    if (!process.Responding || process.MainWindowTitle.ToString().Contains("Oops"))
                    {
                        try
                        {
                            process.Kill();
                            CountTabCrash++;
                        } 
                        catch 
                        { }
                        return;
                    }
                    PerformanceCounter pf1 = new PerformanceCounter("Process", "Working Set - Private", process.ProcessName);
                    ProcessGame.ListProcessGame.Add(new ProcessGame
                    {
                        PID = process.Id,
                        NAMETAB = process.MainWindowTitle,
                        RAM = pf1.NextValue() / (1024*1024),
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
                        string.Format("{0:0.##}", processGame.RAM) + " MB",
                        processGame.PID,
                    });
                    i++;
                }
            }
            else
            {
                dg.Rows.Clear();
            }
        }
    }
}
