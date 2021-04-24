using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestStation.Models
{
    public class TOSADevice : Device
    {

        // Source current for continuity test
        double I_Continuity { get; set; }

        // Minimum voltage for continuity test
        double V_Continuity_Min { get; set; }

        // Maximum voltage for continuity test
        double V_Continuity_Max { get; set; }

        // Current tolerance for continuity test
        double I_Continuity_Tol { get; set; }

        // Start current for sweep
        double I_Start { get; set; }

        // Step current for sweep
        double I_Step { get; set; }

        // Stop current for sweep
        double I_Stop { get; set; }

        // Power at which parameters are calculated
        double P_OP { get; set; }

        // Minimum current for the range over which RS, SE and Ith is calculated
        double I_OP_Min { get; set; }

        // Maximum current for the range over which RS, SE and Ith is calculated
        double I_OP_Max { get; set; }

        // Time for wiggle test
        double Wiggle_Time { get; set; }

        // Test point for reverse breakdown
        double VBR_Test { get; set; }

        // Minimum voltage at I_Test
        double V_OP_Min { get; set; }

        // Maximum voltage at I_Test
        double V_OP_Max { get; set; }

        // Minimum series resistance
        double RS_Min { get; set; }
        
        // Maximum series resistance
        double RS_Max { get; set; }

        // Minimum slope efficiency
        double SE_Min { get; set; }

        // Maximum slope efficiency
        double SE_Max { get; set; }

        // Minimum threshold current
        double Ith_Min { get; set; }

        // Maximum threshold current
        double Ith_Max { get; set; }

        // Maximum allowed wiggle at I_Test
        double Pwiggle_Max { get; set; }

        // Minumum coupling efficiency including wiggle at I_Test
        double POPCT_Min { get; set; }

        // Minimum monitor current at P_Test
        double IBM_Min { get; set; }

        // Maxmimum monitor current at P_Test
        double IBM_Max { get; set; }

        // Minimum monitor current tracking
        double IBM_Tracking_Min { get; set; }

        // Maximum monitor current tracking
        double IBM_Tracking_Max { get; set; }

        // Maximum reverse breakdown current at VBR_Test
        double IBR_Max { get; set; }
    }
}
