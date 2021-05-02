using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestStation.Models
{
    public class ROSAOutput : Output
    {
        public double V_Test { get; set; }
        public double P_Laser { get; set; }
        public double I_TIA { get; set; }
        public double I_PD { get; set; }
        public double Responsivity { get; set; }
        public double I_Dark { get; set; }
        public bool I_TIA_Pass { get; set; }
        public bool RESP_Pass { get; set; }
        public bool Wiggle_Pass { get; set; }
        public bool Result { get; set; }
    }
}
