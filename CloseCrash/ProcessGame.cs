using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloseCrash
{
    public class ProcessGame
    {
        public string TAB { get; set; }
        public float RAM { get; set; }
        public string NAMETAB { get; set; }
        public int PID { get; set; }
        public static List<ProcessGame> ListProcessGame;

        public static ProcessGame InStance;

        public static ProcessGame _InStance()
        {
            if(InStance == null)
            {
                InStance = new ProcessGame();
            }
            return InStance;
        }
    }
}
