using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestStation
{
    class TestCalculations
    {
        public static bool ContinuityTest(double Isource, double vmin, double vmax, double Ierror)
        {
            try
            {
                //Convert to mA and mV
                Isource /= 1000;
                vmin /= 1000;
                vmax /= 1000;

                //Activate channel 1 with source current
                Instruments.Instance.ChannelPower(1, true);
                Instruments.Instance.SourceCurrent(1, Isource);

                double voltage = Instruments.Instance.GetVoltage(1);
                double current = Instruments.Instance.GetCurrent(1);

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
            catch (Exception continuityTestError)
            {
                return false;
            }
            finally
            {
                Instruments.Instance.ChannelPower(1, false);
            }

            return true;
        }

        public static SweepValue SweepTest(double I_Start, double I_Stop, double I_Step)
        {
            //Convert to mA
            I_Start /= 1000;
            I_Stop /= 1000;
            I_Step /= 1000;


            Instruments.Instance.ChannelPower(1, true);
            Instruments.Instance.ChannelPower(2, true);
            Instruments.Instance.ChannelPower(3, true);
            SweepValue sweepValues = new SweepValue();

            int count = (int)((I_Stop - I_Start) / I_Step);

            double current_Iterator = I_Start;
            for (int i = 0; i <= count; i++)
            {
                Instruments.Instance.SourceCurrent(1, current_Iterator);
                //Debug.WriteLine("Source Current: " + current_Iterator);

                double c = Instruments.Instance.GetCurrent(1);
                double v = Instruments.Instance.GetVoltage(1);

                sweepValues.current.Add(c);
                sweepValues.voltage.Add(v);
                sweepValues.power.Add(Instruments.Instance.GetPower(3));
                sweepValues.ibm.Add(Instruments.Instance.GetCurrent(2));

                current_Iterator += I_Step;
            }

            Instruments.Instance.ChannelPower(1, false);
            Instruments.Instance.ChannelPower(2, false);
            Instruments.Instance.ChannelPower(3, false);

            return sweepValues;
        }

        public static double PowerOP (SweepValue sweepValues, double POP)
        {
            double pClosest = Double.NegativeInfinity;

            foreach (double p in sweepValues.power)
            {
                if (p > pClosest && p < POP)
                    pClosest = p;
            }

            int index = sweepValues.power.FindIndex(d => d.Equals(pClosest));

            double y1 = sweepValues.power.ElementAt(index);
            double x1 = sweepValues.current.ElementAt(index);
            double y2 = sweepValues.power.ElementAt(index + 1);
            double x2 = sweepValues.current.ElementAt(index + 1);


            return y1 + ((POP - x1) * ((y2 - y1) / (x2 - x1)));
        }

        public static double Resistance(SweepValue sweepValues, double I_OP_Min, double I_OP_Max)
        {
            double vMin = sweepValues.voltage.Aggregate((cur, next) => Math.Abs(I_OP_Min - cur) < Math.Abs(I_OP_Min - next) ? cur : next);
            double vMax = sweepValues.voltage.Aggregate((cur, next) => Math.Abs(I_OP_Max - cur) < Math.Abs(I_OP_Max - next) ? cur : next);

            return (vMax - vMin) / (I_OP_Max - I_OP_Min);
        }

        public static double SlopeEfficiency(SweepValue sweepValues, double I_OP_Min, double I_OP_Max)
        {
            double pMin = sweepValues.power.Aggregate((cur, next) => Math.Abs(I_OP_Min - cur) < Math.Abs(I_OP_Min - next) ? cur : next);
            double pMax = sweepValues.power.Aggregate((cur, next) => Math.Abs(I_OP_Max - cur) < Math.Abs(I_OP_Max - next) ? cur : next);

            return (pMax - pMin) / (I_OP_Max - I_OP_Min);
        }

        public static double ThresholdCurrent(SweepValue sweepValues, double se)
        {
            //x = -b / se

            return 0;
        }

        public static double IBR(double VBR_Test)
        {
            Instruments.Instance.ChannelPower(1, true);
            Instruments.Instance.SourceCurrent(1, VBR_Test);

            double ibr = Instruments.Instance.GetCurrent(1);

            Instruments.Instance.ChannelPower(1, false);
            return ibr;
        }
        
        public static double P_Total(double I_Test)
        {
            Instruments.Instance.ChannelPower(1, true);
            Instruments.Instance.SourceCurrent(1, I_Test);

            double p_total = Instruments.Instance.GetPower(1);

            Instruments.Instance.ChannelPower(1, false);
            return p_total;
        }


    }
}
