using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestStation.Models
{
    public class ROSADevice : Device
    {
        public double V_Test { get; set; }
        public double RESP_Min { get; set; }
        public double I_TIA_Max { get; set; }
        public double PD_Wiggle_Max { get; set; }
        public double Wiggle_Time { get; set; }
        public string RSSI_VPD { get; set; }
    }
}
