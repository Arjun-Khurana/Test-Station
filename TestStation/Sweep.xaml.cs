using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TestStation.Models;
using System.Diagnostics;

namespace TestStation
{
    /// <summary>
    /// Interaction logic for Sweep.xaml
    /// </summary>
    public partial class Sweep : Page
    {
        private Device d;
        private Output o;
        private int numTries;
        private bool passed;
        public Sweep()
        {
            InitializeComponent();
            numTries = 0;
            passed = false;
        }

        

        private void Start_Test_Button_Click(object sender, RoutedEventArgs e)
        {
            if (numTries == 0)
            {
                var w = Window.GetWindow(this) as MainWindow;
                d = w.device;
                o = w.output;
            }

            if (numTries == 3 && !passed)
            {
                NavigationService.Navigate(new HomePage());
                return;
            }
            else if (numTries < 3 && passed)
            {
                NavigationService.Navigate(new Wiggle());
            }

            numTries++;
            TOSAStep1();
        }

        private async void TOSAStep1()
        {
            TOSADevice device = d as TOSADevice;
            TOSAOutput output = o as TOSAOutput;

            bool sweepTestResult = true;

            SweepValue sweepValues = await TestCalculations.SweepTest(device.I_Start, device.I_Stop, device.I_Step, sweepProgress);
            double r = TestCalculations.FindSlope(sweepValues.current, sweepValues.voltage, device.I_OP_Min, device.I_OP_Max);
            //Debug.Print("Resistance: {0}", resistance);
            output.RS = r;
            resistance.Text = r.ToString() + " Ω";

            double se = TestCalculations.FindSlope(sweepValues.current, sweepValues.power, device.I_OP_Min, device.I_OP_Max);
            //Debug.Print("SE: {0}", slopeEfficiency);
            output.SE = se;
            slopeEfficiency.Text = se.ToString();

            double ith = TestCalculations.ThresholdCurrent(sweepValues, device.I_OP_Min, device.I_OP_Max);
            //Debug.Print("Ith: {0}", thresholdCurrent);
            output.Ith = ith;
            thresholdCurrent.Text = ith.ToString() + " mA";

            //double threshholdCurrent = TestCalculations.ThresholdCurrent(sweepValues, slopeEfficiency);



            //if (sweepTestResult)
            //{
            //    passed = true;
            //    StartTestButton.Content = "Next step";
            //}
            //else
            //{
            //    if (numTries >= 3)
            //    {
            //        StartTestButton.Content = "Go home";
            //    }
            //    else
            //    {
            //        StartTestButton.Content = "Retry test";
            //    }
            //}
        }

        private void ROSAStep1()
        {

        }

        private void ShowErrorPanel()
        {
            errorPanel.Visibility = Visibility.Visible;
        }

        private void HideErrorPanel()
        {
            errorPanel.Visibility = Visibility.Collapsed;
        }
    }
}
