using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestStation
{
    class SweepData
    {
        public List<double> current { get; set; }
        public List<double> voltage { get; set; }
        public List<double> power { get; set; }
        public List<double> ibm { get; set; }
        public List<double> setcurrent { get; set; }

        public SweepData()
        {
            setcurrent = new List<double>();
            current = new List<double>();
            voltage = new List<double>();
            power = new List<double>();
            ibm = new List<double>();
        }
    }

    class WiggleData
    {
        public double max { get; set; }
        public double min { get; set; }
        public double avg { get; set; }

        public WiggleData()
        {
            max = 0;
            min = 0;
            avg = 0;
        }
    }

    class OBData
    {
        public double p_test { get; set; }
        public double v_test { get; set; }
        public double ibm_test { get; set; }
        public double ibr { get; set; }

        public double i_test { get; set; }

        public OBData()
        {
            p_test = 0;
            v_test = 0;
            ibm_test = 0;
            ibr = 0;
            i_test = 0;
        }
    }

}
