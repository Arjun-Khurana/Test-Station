using System;
using System.Collections.Generic;
using System.Windows;
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

        public static double FindSlope(List<double> x, List<double> y, double min, double max)
        {
            double slope;
            double intercept;

            int low = x.IndexOf(min);
            int high = x.IndexOf(max);

            x = x.GetRange(low, high - low);
            y = y.GetRange(low, high - low);

            List<Point> points = new List<Point>();

            int k = 0;
            foreach (double i in x)
            {
                points.Add(new Point(i, y.ElementAt<double>(k)));
                k++;
            }

            double r2 = FindLinearLeastSquaresFit(points, out slope, out intercept);

            return slope;
        }

        public static double FindSlope(List<double> x, List<double> y, double min, double max, out double intercept)
        {
            double slope;

            int low = x.IndexOf(min);
            int high = x.IndexOf(max);

            x = x.GetRange(low, high - low);
            y = y.GetRange(low, high - low);

            List<Point> points = new List<Point>();

            int k = 0;
            foreach (double i in x)
            {
                points.Add(new Point(i, y.ElementAt<double>(k)));
                k++;
            }

            double r2 = FindLinearLeastSquaresFit(points, out slope, out intercept);

            return slope;
        }

        private static double FindLinearLeastSquaresFit(List<Point> points, out double m, out double b)
        {
            // Perform the calculation.
            // Find the values S1, Sx, Sy, Sxx, and Sxy.
            double S1 = points.Count;
            double Sx = 0;
            double Sy = 0;
            double Sxx = 0;
            double Sxy = 0;
            foreach (Point pt in points)
            {
                Sx += pt.X;
                Sy += pt.Y;
                Sxx += pt.X * pt.X;
                Sxy += pt.X * pt.Y;
            }

            // Solve for m and b.
            m = (Sxy * S1 - Sx * Sy) / (Sxx * S1 - Sx * Sx);
            b = (Sxy * Sx - Sy * Sxx) / (Sx * Sx - S1 * Sxx);

            return Math.Sqrt(ErrorSquared(points, m, b));
        }

        private static double ErrorSquared(List<Point> points, double m, double b)
        {
            double total = 0;
            foreach (Point pt in points)
            {
                double dy = pt.Y - (m * pt.X + b);
                total += dy * dy;
            }
            return total;
        }

        public static double ThresholdCurrent(SweepValue sweepValues, double I_OP_Min, double I_OP_Max)
        {
            List<double> currents = sweepValues.current;
            List<double> powers = sweepValues.power;

            double b;

            double m = FindSlope(currents, powers, I_OP_Min, I_OP_Max, out b);

            return -b / m;
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
