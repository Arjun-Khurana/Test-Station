using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestStation.Models
{
    public class TOSAOutput : Output
    {
        public double I_Test { get; set; }
        public double P_Test_OB { get; set; }
        public bool P_OB_Pass { get; set; }
        public double V_Test { get; set; }
        public bool V_Test_Pass { get; set; }
        public double IBM_Test_OB { get; set; }
        public bool IBM_Pass { get; set; }
        public double VBR_Test { get; set; }
        public double IBR { get; set; }
        public bool IBR_Pass { get; set; }
        public double P_Test_FC { get; set; }
        public bool P_Test_FC_Pass { get; set; }
        public double RS { get; set; }
        public bool RS_Pass { get; set; }
        public double SE { get; set; }
        public bool SE_Pass { get; set; }
        public double Ith { get; set; }
        public bool Ith_Pass { get; set; }
        public double POPCT { get; set; }
        public bool POPCT_Pass { get; set; }
        public double I_BM_Slope { get; set; }
        public double I_BM_P_BM_Test { get; set; }
        public bool I_BM_P_BM_Test_Pass { get; set; }
        public double IBM_Track { get; set; }
        public bool I_BM_Track_Pass { get; set; }
        public double POPCT_Wiggle_Min { get; set; }
        public bool POPCT_Wiggle_Min_Pass { get; set; }
        public double Wiggle_dB { get; set; }

    }
}
