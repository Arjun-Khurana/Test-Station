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
        public double ICC_Max { get; set; }
        public double I_Wiggle_Max { get; set; }
        public double Wiggle_Time { get; set; }
        public double RSSSI_VPD { get; set; }

    }
}
