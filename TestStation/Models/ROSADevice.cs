using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestStation.Models
{
    public class ROSADevice : Device
    {
        double V_Test { get; set; }
        double RESP_Min { get; set; }
        double ICC_Max { get; set; }
        double I_Wiggle_Max { get; set; }
        double Wiggle_Time { get; set; }
        double RSSSI_VPD { get; set; }

    }
}
