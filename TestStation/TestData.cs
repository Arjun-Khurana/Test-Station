using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestStation
{
    class SweepData
    {
        public List<double> currents { get; set; }
        public List<double> voltages { get; set; }
        public List<double> powers { get; set; }
        public List<double> ibms { get; set; }
        public List<double> setcurrents { get; set; }

        public SweepData()
        {
            setcurrents = new List<double>();
            currents = new List<double>();
            voltages = new List<double>();
            powers = new List<double>();
            ibms = new List<double>();
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

    class DLData
    { 
        public double i_tia { get; set; }
        public double i_pd { get; set; }
        public double responsivity { get; set; }
        public double i_dark { get; set; }

        public DLData()
        {
            i_tia = 0;
            i_pd = 0;
            responsivity = 0;
            i_dark = 0;
        }
    }
    

}
