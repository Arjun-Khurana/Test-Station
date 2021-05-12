using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestStation.Models
{
    public class Output
    {
        public string Part_Number { get; set; }
        public string Job_Number { get; set; }
        public int Unit_Number { get; set; }
        public string Operator { get; set; }
        public DateTime Timestamp { get; set; }
        public int Repeat_Number { get; set; }
        public bool Result { get; set; }
    }
}
