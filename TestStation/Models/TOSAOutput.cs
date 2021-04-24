using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestStation.Models
{
    public class TOSAOutput : Device
    {
        // User entered
        string Job_Number { get; set; }

        // Sequentially determined in program
        string Unit_Number { get; set; }

        // User entered
        string Operator { get; set; }

        // Obtained at test start for each unit
        DateTime Timestamp { get; set; }

        // Sequentially determined in program (1 for first time test, increment for additional)
        int Repeat_Number { get; set; }

        double I_Test { get; set; }
        double P_Test { get; set; }
        double VBR_Test { get; set; }
        double P_Total { get; set; }
        double V_OP { get; set; }
        double P_OP { get; set; }
        double RS { get; set; }
        double SE { get; set; }
        double Ith { get; set; }
        double POPCT { get; set; }
        double POPCT_Min { get; set; }
        double Pwiggle { get; set; }
        double IBM { get; set; }
        double IBM_Tracking { get; set; }
        double IBR { get; set; }
        bool V_OP_Pass { get; set; }
        bool P_OP_Pass { get; set; }
        bool RS_Pass { get; set; }
        bool SE_Pass { get; set; }
        bool Ith_Pass { get; set; }
        bool Pwiggle_Pass { get; set; }
        bool POPCT_Min_Pass { get; set; }
        bool IBM_Pass { get; set; }
        bool IBM_Tracking_Pass { get; set; }
        bool IBR_Pass { get; set; }
        bool Result { get; set; }
    }
}
