using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestStation.Models
{
    public class ROSADevice
    {
        int id { get; set; }
        string Part_Number { get; set; }
        double V_Test { get; set; }
        double RESP_Min { get; set; }
        double ICC_Max { get; set; }
        double I_Wiggle_Max { get; set; }
        double Wiggle_Time { get; set; }
        double RSSSI_VPD { get; set; }

    }
}
