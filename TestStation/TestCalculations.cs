using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Controls;
using System.Threading;

namespace TestStation
{
    class TestCalculations
    {
        private const double RESPONSIVITY = 0.52;
        public async static Task<SweepData> SweepTest(double I_Start, double I_Stop, double I_Step, ProgressBar pb)
        {
            var progress = new Progress<double>(value => pb.Value = value);

            Instruments.Instance.ChannelPower(1, true);
            Instruments.Instance.ChannelPower(2, true);
            Instruments.Instance.ChannelPower(3, true);
            SweepData sweepData = new SweepData();

            int count = (int)((I_Stop - I_Start) / I_Step);

            double current_Iterator = I_Start;

            await Task.Run(() =>
            {
                for (int i = 0; i <= count; i++)
                {
                    ((IProgress<double>)progress).Report(100*((double)i/(double)count));
                    Instruments.Instance.SourceCurrent(1, current_Iterator);
                    Instruments.Instance.SourceVoltage(2, 0);
                    Instruments.Instance.SourceVoltage(3, 0);
                    //Debug.WriteLine("Source Current: " + current_Iterator);

                    double c = Instruments.Instance.GetCurrent(1);
                    //Debug.Print("Source Current: {0}", c);
                    double v = Instruments.Instance.GetVoltage(1);
                    //Debug.Print("Voltage: {0}", v);

                    sweepData.setcurrents.Add(current_Iterator);
                    sweepData.currents.Add(c * 1000);
                    sweepData.voltages.Add(v);

                    double p = Instruments.Instance.GetCurrent(3) / RESPONSIVITY;
                    //Debug.Print("Power: {0}", p);
                    double ibm = Instruments.Instance.GetCurrent(2);
                    //Debug.Print("ibm: {0}", ibm);

                    //Debug.Print("{0}, {1}, {2}, {3}, {4},", current_Iterator, c, v, p, ibm);

                    sweepData.powers.Add(p * 1000);
                    sweepData.ibms.Add(ibm * 1000);

                    //Debug.Print("");

                    current_Iterator += I_Step;
                }
            });

            Instruments.Instance.ChannelPower(1, false);
            Instruments.Instance.ChannelPower(2, false);
            Instruments.Instance.ChannelPower(3, false);

            return sweepData;
        }

        public async static Task<WiggleData> WiggleTest(int sec, ProgressBar pb)
        {
            var progress = new Progress<double>(value => pb.Value = value);
            int ms = sec * 1000;
            int increment = 100;

            Instruments.Instance.ChannelPower(1, true);
            Instruments.Instance.ChannelPower(3, true);
            Instruments.Instance.SourceCurrent(1, 6);
            Instruments.Instance.SourceVoltage(3, 0);

            List<double> powers = new List<double>();

            await Task.Run(() =>
            {
                for (int i = 0; i <= ms; i += increment)
                {
                    powers.Add(Instruments.Instance.GetCurrent(3));
                    Thread.Sleep(increment);
                    ((IProgress<double>)progress).Report(100 * ((double)i / (double)ms));
                }
            });
            

            return new WiggleData
            {
                min = powers.Min(),
                max = powers.Max(),
                avg = powers.Average()
            };
        }

        public static OBData OpenBoreTest(double I_Test, double VBR_Test)
        {
            OBData ob = new OBData();

            Instruments.Instance.ChannelPower(1, true);
            Instruments.Instance.ChannelPower(2, true);
            Instruments.Instance.ChannelPower(3, true);

            Instruments.Instance.SourceCurrent(1, I_Test);
            Instruments.Instance.SourceVoltage(2, 0);
            Instruments.Instance.SourceVoltage(3, 0);

            ob.p_test = Instruments.Instance.GetCurrent(3) / RESPONSIVITY * 1000;
            ob.v_test = Instruments.Instance.GetVoltage(1);
            ob.ibm_test = Instruments.Instance.GetCurrent(2) * 1000;
            ob.i_test = Instruments.Instance.GetCurrent(1) * 1000;

            Instruments.Instance.ChannelPower(1, false);
            Instruments.Instance.ChannelPower(2, false);
            Instruments.Instance.ChannelPower(3, false);

            Instruments.Instance.ChannelPower(1, true);
            Instruments.Instance.SourceVoltage(1, VBR_Test);
            ob.ibr = Instruments.Instance.GetCurrent(1) * 1000000;

            Instruments.Instance.ChannelPower(1, false);

            return ob;
        }

        public static double FindSlope(List<double> x, List<double> y, double min, double max)
        {
            double slope;
            double intercept;

            int low = x.IndexOf(x.Aggregate((cur, next) => Math.Abs(min - cur) < Math.Abs(min - next) ? cur : next));
            int high = x.IndexOf(x.Aggregate((cur, next) => Math.Abs(max - cur) < Math.Abs(max - next) ? cur : next));

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

            int low = x.IndexOf(x.Aggregate((cur, next) => Math.Abs(min - cur) < Math.Abs(min - next) ? cur : next));
            int high = x.IndexOf(x.Aggregate((cur, next) => Math.Abs(max - cur) < Math.Abs(max - next) ? cur : next));

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

        public static double ThresholdCurrent(SweepData sweepData, double I_OP_Min, double I_OP_Max)
        {
            List<double> currents = sweepData.currents;
            List<double> powers = sweepData.powers;

            double b;

            double m = FindSlope(currents, powers, I_OP_Min, I_OP_Max, out b);

            return -b / m;
        }

        public static double IBM_PBM(SweepData sweepData, double P_Test)
        {
            double PBM = sweepData.powers.Aggregate((cur, next) => Math.Abs(P_Test - cur) < Math.Abs(P_Test - next) ? cur : next);
            int i = sweepData.powers.IndexOf(PBM);
            return sweepData.ibms.ElementAt(i);
        }

    }
}
