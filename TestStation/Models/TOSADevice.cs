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
        public double I_Continuity { get; set; }

        // Minimum voltage for continuity test
        public double V_Continuity_Min { get; set; }

        // Maximum voltage for continuity test
        public double V_Continuity_Max { get; set; }

        // Current tolerance for continuity test
        public double I_Continuity_Tol { get; set; }

        // Start current for sweep
        public double I_Start { get; set; }

        // Step current for sweep
        public double I_Step { get; set; }

        // Stop current for sweep
        public double I_Stop { get; set; }

        // Current at which parameters are calculated
        public double I_Test { get; set; }

        // Power at which IBM is calculated
        public double P_Test { get; set; }

        // Minimum power for the range over which IBM_Tracking is calculated
        public double P_OP_Min { get; set; }

        //Maximum power for the range over which IBM_Tracking is calculated
        public double P_OP_Max { get; set; }

        // Minimum current for the range over which RS, SE and Ith is calculated
        public double I_OP_Min { get; set; }

        // Maximum current for the range over which RS, SE and Ith is calculated
        public double I_OP_Max { get; set; }

        // Time for wiggle test
        public double Wiggle_Time { get; set; }

        // Test point for reverse breakdown
        public double VBR_Test { get; set; }

        // Minimum voltage at I_Test
        public double V_OP_Min { get; set; }

        // Maximum voltage at I_Test
        public double V_OP_Max { get; set; }

        // Minimum series resistance
        public double RS_Min { get; set; }
        
        // Maximum series resistance
        public double RS_Max { get; set; }

        // Minimum slope efficiency
        public double SE_Min { get; set; }

        // Maximum slope efficiency
        public double SE_Max { get; set; }

        // Minimum threshold current
        public double Ith_Min { get; set; }

        // Maximum threshold current
        public double Ith_Max { get; set; }

        // Maximum allowed wiggle at I_Test
        public double Pwiggle_Max { get; set; }

        // Minumum coupling efficiency including wiggle at I_Test
        public double POPCT_Min { get; set; }

        // Minimum monitor current at P_Test
        public double IBM_Min { get; set; }

        // Maxmimum monitor current at P_Test
        public double IBM_Max { get; set; }

        // Minimum monitor current tracking
        public double IBM_Tracking_Min { get; set; }

        // Maximum monitor current tracking
        public double IBM_Tracking_Max { get; set; }

        // Maximum reverse breakdown current at VBR_Test
        public double IBR_Max { get; set; }
    }
}
