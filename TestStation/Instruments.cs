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

        private double GetVoltage(int channel)
        {
            return (Double.Parse(ReadSMU("MEAS:VOLT? (@" + channel + ")"))) / 1000;
        }

        private double GetCurrent(int channel)
        {
            return (Double.Parse(ReadSMU("MEAS:CURR? (@" + channel + ")"))) / 1000;
        }

        public double GetPower(int channel)
        {
            WriteSMU("VOLT 0, (@" + channel + ")");
            return GetCurrent(channel);
        }

        private void ChannelPower(int channel, bool on)
        {
            int activate = Convert.ToInt32(on);
            WriteSMU("OUTP " + activate + ", (@" + channel + ")");
        }

        private void SourceCurrent(int channel, double sourceCurrent)
        {
            WriteSMU("CURR " + sourceCurrent + ", (@" + channel + ")");
        }

        public bool ContinuityTest(double Isource, double vmin, double vmax, double Ierror)
        {
            try
            {
                //Convert to mA and mV
                Isource /= 1000;
                vmin /= 1000;
                vmax /= 1000;

                //Activate channel 1 with source current
                ChannelPower(1, true);
                SourceCurrent(1, Isource);

                double voltage = GetVoltage(1);
                double current = GetCurrent(1);

                //Check voltage is within acceptable range
                if (voltage < vmin || voltage > vmax)
                {
                    return false;
                }

                //Check current is within acceptable range
                if (current < Isource * (1 - Ierror) || current > Isource * (1 + Ierror))
                {
                    return false;
                }

            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                ChannelPower(1, false);
            }

            return true;
        }

        public bool SweepTest(double I_Start, double I_Stop, double I_Step)
        {
            try
            {
                //Convert to mA
                I_Start /= 1000;
                I_Stop /= 1000;
                I_Step /= 1000;


                ChannelPower(1, true);
                ChannelPower(2, true);
                List<SweepValue> sweepValues = new List<SweepValue>();

                int count = (int)((I_Stop - I_Start) / I_Step);

                double current_Iterator = I_Start;
                for (int i = 0; i <= count; i++)
                {
                    SourceCurrent(1, current_Iterator);
                    //Debug.WriteLine("Source Current: " + current_Iterator);

                    SweepValue temp;
                    
                    double c = GetCurrent(1);
                    double v = GetVoltage(1);
                    
                    temp.current = c;
                    temp.voltage = v;
                    temp.power = c * v;
                    temp.ibm = GetCurrent(2);

                    sweepValues.Add(temp);

                    current_Iterator += I_Step;
                }
            }
            catch(Exception e)
            {
                return false;
            }
            finally
            {
                ChannelPower(1, false);
                ChannelPower(2, false);
            }

            return true;
        }

        struct SweepValue
        {
            public double current;
            public double voltage;
            public double power;
            public double ibm;
        }

    }
}
