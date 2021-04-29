using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestStation
{
    class SweepValue
    {
        public List<double> current { get; set; }
        public List<double> voltage { get; set; }
        public List<double> power { get; set; }
        public List<double> ibm { get; set; }

        public SweepValue()
        {
            current = new List<double>();
            voltage = new List<double>();
            power = new List<double>();
            ibm = new List<double>();
        }
    }

    class TestData
    {
        public SweepValue sweep1;
        public SweepValue sweep2;
    }
}
