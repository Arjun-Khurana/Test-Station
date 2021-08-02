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

namespace TestStation
{
    /// <summary>
    /// Interaction logic for ROSAStep2.xaml
    /// </summary>
    public partial class ROSAStep2 : Page
    {
        private int numTries;
        private bool passed;
        private Device d;
        private Output o;

        public ROSAStep2()
        {
            InitializeComponent();
            numTries = 0;
            passed = false;
        }

        void OnLoad(object sender, RoutedEventArgs e)
        {
            var w = MainWindow.GetWindow(this) as MainWindow;
            UnitNumberText.Text = $"Unit number: {w.output.Unit_Number}";
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
            else if (numTries <= 3 && passed)
            {
                NavigationService.Navigate(new HomePage());
                return;
            }

            numTries++;
            Wiggle();
        }

        private async void Wiggle()
        {
            ROSADevice device = d as ROSADevice;
            ROSAOutput output = o as ROSAOutput;

            bool wiggleTestResult = true;

            WiggleData wiggleData = await TestCalculations.ROSAWiggleTest(device.V_Test, (device.RSSI_VPD == "VPD"), (int)device.Wiggle_Time, wiggleProgress);

            wiggleMin.Text = wiggleData.min.ToString("F") + " mW";
            wiggleMax.Text = wiggleData.max.ToString("F") + " mW";
            wiggleAvg.Text = wiggleData.avg.ToString("F") + " mW";

            double i_wiggle = 10 * Math.Log10(wiggleData.max / wiggleData.min);
            iWiggle.Text = i_wiggle.ToString("F") + " dB";
            bool I_Wiggle_Pass = (i_wiggle <= device.PD_Wiggle_Max);

            output.Wiggle_Pass = I_Wiggle_Pass;

            if (!I_Wiggle_Pass)
            {
                iWiggle.Foreground = Brushes.OrangeRed;
                wiggleTestResult = false;
            }

            measurementPanel.Visibility = Visibility.Visible;
            var w = Window.GetWindow(this) as MainWindow;

            if (wiggleTestResult)
            {
                passed = true;
                testMessage.Text = "Test Passed";
                testMessage.Foreground = Brushes.ForestGreen;
                StartTestButton.Content = "End job";
                output.Result = true;
                MainWindow.Conn.SaveROSAOutput(output);
                d = device;
                o = output;
                w.device = d;
                w.output = o;
            }
            else
            {
                testMessage.Text = "Test Failed";
                testMessage.Foreground = Brushes.OrangeRed;
                if (numTries >= 3)
                {
                    StartTestButton.Content = "Go home";
                    output.Result = false;
                    MainWindow.Conn.SaveROSAOutput(output);
                }
                else
                {
                    StartTestButton.Content = "Retry test";
                }
            }
        }

        private void Next_Device_Button_Click(object sender, RoutedEventArgs e)
        {
            var w = Window.GetWindow(this) as MainWindow;

            var d = w.device as ROSADevice;
            var currentOutput = w.output as ROSAOutput;
            var job = currentOutput.Job_Number;

            w.output = new ROSAOutput
            {
                Part_Number = d.Part_Number,
                Job_Number = job,
                Operator = currentOutput.Operator,
                Unit_Number = currentOutput.Unit_Number + 1,
                Timestamp = DateTime.Now
            };

            NavigationService.Navigate(new ROSAStep1());
        }
    }
}
