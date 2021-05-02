using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NationalInstruments.Visa;

namespace TestStation
{
    class Instruments
    {
        private UsbSession SourceMeasureUnit;
        private List<SweepValue> sweep1;
        private List<SweepValue> sweep2;

        private const double RESPONSIVITY = 1;

        private static readonly Lazy<Instruments> lazy = new Lazy<Instruments>(() => new Instruments());
        private Instruments()
        {
            using (var resourceManager = new ResourceManager())
            {
                IEnumerable<string> resources = resourceManager.Find("USB?*INSTR");

                string name = "";
                foreach (string s in resources)
                {
                    name = s;
                }

                SourceMeasureUnit = (UsbSession)resourceManager.Open(name);
            }
        }
        public static Instruments Instance
        {
            get { return lazy.Value; }
        }

        private void WriteSMU(string command)
        {
            SourceMeasureUnit.RawIO.Write(command);
        }

        private string ReadSMU(string command)
        {
            SourceMeasureUnit.RawIO.Write(command);
            return SourceMeasureUnit.RawIO.ReadString();
        }

        public double GetVoltage(int channel)
        {
            return (Double.Parse(ReadSMU("MEAS:VOLT? (@" + channel + ")")));
        }

        public double GetCurrent(int channel)
        {
            return (Double.Parse(ReadSMU("MEAS:CURR? (@" + channel + ")")));
        }

        public double GetPower(int channel)
        {
            WriteSMU("VOLT 0, (@" + channel + ")");
            return GetCurrent(channel) / RESPONSIVITY;
        }

        public void ChannelPower(int channel, bool on)
        {
            int activate = Convert.ToInt32(on);
            WriteSMU("OUTP " + activate + ", (@" + channel + ")");
        }

        public void SourceCurrent(int channel, double sourceCurrent)
        {
            WriteSMU("CURR " + sourceCurrent + ", (@" + channel + ")");
        }
    }
}
