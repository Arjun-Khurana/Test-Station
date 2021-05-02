using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestStation.Models
{
    public class TOSAOutput : Output
    {
        public double I_OP { get; set; }
        public double P_OP { get; set; }
        public double VBR_Test { get; set; }
        public double P_Total { get; set; }
        public double V_OP { get; set; }
        public double RS { get; set; }
        public double SE { get; set; }
        public double Ith { get; set; }
        public double POPCT { get; set; }
        public double POPCT_Min { get; set; }
        public double Pwiggle { get; set; }
        public double IBM { get; set; }
        public double IBM_Tracking { get; set; }
        public double IBR { get; set; }
        public bool V_OP_Pass { get; set; }
        public bool I_OP_Pass { get; set; }
        public bool RS_Pass { get; set; }
        public bool SE_Pass { get; set; }
        public bool Ith_Pass { get; set; }
        public bool Pwiggle_Pass { get; set; }
        public bool POPCT_Min_Pass { get; set; }
        public bool IBM_Pass { get; set; }
        public bool IBM_Tracking_Pass { get; set; }
        public bool IBR_Pass { get; set; }
        public bool Result { get; set; }
    }
}
