using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestStation.Models
{
    public class ROSAOutput
    {
        int id { get; set; }
        string Part_Number { get; set; }
        string Job_Number { get; set; }
        string Unit_Number { get; set; }
        string Operator { get; set; }
        DateTime Timestamp { get; set; }
        int Repeat_Number { get; set; }
        double V_Test { get; set; }
        double P_Laser { get; set; }
        double I_TIA { get; set; }
        double I_PD { get; set; }
        double Responsivity { get; set; }
        double I_Dark { get; set; }
        bool I_TIA_Pass { get; set; }
        bool RESP_Pass { get; set; }
        bool Wiggle_Pass { get; set; }
        bool Result { get; set; }
    }
}
