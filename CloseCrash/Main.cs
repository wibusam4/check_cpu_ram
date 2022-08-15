using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CloseCrash
{
    public partial class Main : Form
    {
        public PerformanceCounter total_cpu = new PerformanceCounter("Processor Information", "% Processor Time", "_Total");
        public Main()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            prbCPU.Value = 10;
        }

        private void btnCloseApp_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnMiniSize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void btnCheckCrash_Click(object sender, EventArgs e)
        {
            CheckCrash.IsRuning = !CheckCrash.IsRuning;
            SetColor(CheckCrash.IsRuning, this.btnCheckCrash);
        }

        private void btnCleanRam_Click(object sender, EventArgs e)
        {
            CleanRam.IsRuning = !CleanRam.IsRuning;
            SetColor(CleanRam.IsRuning, this.btnCleanRam);
            
            
            //Computer.Computed();
        }

        public void SetColor(bool IsRuning, ns1.SiticoneGradientButton button)
        {
            if (IsRuning)
            {
                button.FillColor = Color.FromArgb(156, 136, 255);
                button.FillColor2 = Color.FromArgb(78, 34, 147);
                return;
            }
            button.FillColor = Color.FromArgb(113, 128, 147);
            button.FillColor2 = Color.FromArgb(127, 143, 166);
            return;
        }

        private void timerComputed_Tick(object sender, EventArgs e)
        {
            float t = total_cpu.NextValue();
            prbCPU.Value = 10;
        }
    }
}
