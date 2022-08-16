using System;
using System.Collections.Generic;
using System.Drawing;
using System.Management;
using System.Windows.Forms;

namespace CloseCrash
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            ProcessGame.ListProcessGame = new List<ProcessGame>();
            timerComputed.Start();
            timerLoadProcess.Start();
        }

        private void btnCloseApp_Click(object sender, EventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate
            { 
                this.Close(); 
            });
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
            Computer.Computed();
            prbRam.Value = Computer.PercentRam;
            prbCPU.Value = Computer.PercentCpu;
            lbPercentRam.Text = Computer.PercentRam + "%";
            lbPercentCPU.Text = Computer.PercentCpu + "%";
        }

        private void timerLoadProcess_Tick(object sender, EventArgs e)
        {
            if (CheckCrash.IsRuning)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    CheckCrash.CheckCrashGame(dataGridAcc);
                });
                lbTabRuning.Text = ProcessGame.ListProcessGame.Count.ToString();
            }
        }
    }
}
