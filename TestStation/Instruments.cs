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

        ~Instruments()
        {
            SourceMeasureUnit.Dispose();
        }

        public static Instruments Instance
        {
            get { return lazy.Value; }
        }

        public void TOSALimits()
        {
            WriteSMU("CURR:RANG R120mA, (@1)");
            WriteSMU("CURR:RANG R1mA, (@2)");
            WriteSMU("CURR:RANG R100uA, (@3)");

            WriteSMU("VOLT:RANG R20V, (@1)");
            WriteSMU("VOLT:RANG R2V, (@2)");
            WriteSMU("VOLT:RANG R2V, (@3)");

            WriteSMU("CURR:LIM .02, (@1)");
            WriteSMU("CURR:LIM .005, (@2)");
            WriteSMU("CURR:LIM .005, (@3)");

            WriteSMU("VOLT:LIM 4, (@1)");
            WriteSMU("VOLT:LIM 1, (@2)");
            WriteSMU("VOLT:LIM 1, (@3)");
        }

        public void ROSALimits()
        {
            WriteSMU("CURR:RANG R120mA, (@1)");
            WriteSMU("CURR:RANG R1mA, (@2)");

            WriteSMU("VOLT:RANG R20V, (@1)");
            WriteSMU("VOLT:RANG R20V, (@2)");

            WriteSMU("CURR:LIM .06, (@1)");
            WriteSMU("CURR:LIM .001, (@2)");

            WriteSMU("VOLT:LIM 4, (@1)");
            WriteSMU("VOLT:LIM 4, (@2)");
        }

        public void BreakdownLimits()
        {
            WriteSMU("CURR:RANG R10uA, (@1)");
        }

        private void WriteSMU(string command)
        {
            SourceMeasureUnit.RawIO.Write(command);
        }

        public string QuerySMU(string command)
        {
            SourceMeasureUnit.RawIO.Write(command);
            return SourceMeasureUnit.RawIO.ReadString();
        }

        public double GetVoltage(int channel)
        {
            return (Double.Parse(QuerySMU("MEAS:VOLT? (@" + channel + ")")));
        }

        public double GetCurrent(int channel)
        {
            return (Double.Parse(QuerySMU("MEAS:CURR? (@" + channel + ")")));
        }

        //public double GetPower(int channel)
        //{
        //    WriteSMU("VOLT 0, (@" + channel + ")");
        //    return GetCurrent(channel) / RESPONSIVITY;
        //}

        public void ChannelPower(int channel, bool on)
        {
            int activate = Convert.ToInt32(on);
            WriteSMU("OUTP " + activate + ", (@" + channel + ")");
        }

        public void SourceCurrent(int channel, double sourceCurrent)
        {
            sourceCurrent = sourceCurrent / 1000;
            WriteSMU("CURR " + sourceCurrent + ", (@" + channel + ")");
        }

        public void SourceVoltage(int channel, double sourceVoltage)
        {
            WriteSMU("VOLT " + sourceVoltage + ", (@" + channel + ")");
        }
    }
}
