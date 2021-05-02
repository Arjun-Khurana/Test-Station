using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestStation
{
    class TestCalculations
    {
        public static SweepValue SweepTest(double I_Start, double I_Stop, double I_Step)
        {
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
            Instruments.Instance.ChannelPower(2, true);
            Instruments.Instance.SourceCurrent(1, VBR_Test);

            double ibr = Instruments.Instance.GetCurrent(2);

            Instruments.Instance.ChannelPower(1, false);
            Instruments.Instance.ChannelPower(2, false);
            return ibr;
        }
        
        public static double Power(double I_Test)
        {
            Instruments.Instance.ChannelPower(1, true);
            Instruments.Instance.ChannelPower(3, true);
            Instruments.Instance.SourceCurrent(1, I_Test);

            double p_total = Instruments.Instance.GetPower(3);

            Instruments.Instance.ChannelPower(1, false);
            Instruments.Instance.ChannelPower(3, false);

            return p_total;
        }

        public static double Voltage(double I_Test)
        {
            Instruments.Instance.ChannelPower(1, true);
            Instruments.Instance.SourceCurrent(1, I_Test);

            double voltage = Instruments.Instance.GetVoltage(1);

            Instruments.Instance.ChannelPower(1, false);

            return voltage;
        }

        public static double Current(double I_Test)
        {
            Instruments.Instance.ChannelPower(1, true);
            Instruments.Instance.ChannelPower(3, true);
            Instruments.Instance.SourceCurrent(1, I_Test);

            double current = Instruments.Instance.GetCurrent(3);

            Instruments.Instance.ChannelPower(1, false);
            Instruments.Instance.ChannelPower(3, false);

            return current;
        }

        public static double P_IBM(SweepValue sweepValues, double P_Test)
        {
            return sweepValues.power.Aggregate((cur, next) => Math.Abs(P_Test - cur) < Math.Abs(P_Test - next) ? cur : next);
        }

        public static double POPCT(double P_Test, double P_Total)
        {
            return P_Test / P_Total;
        }
    }
}
