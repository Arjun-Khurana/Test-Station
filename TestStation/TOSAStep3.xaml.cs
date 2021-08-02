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
    /// Interaction logic for Wiggle.xaml
    /// </summary>
    public partial class TOSAStep3 : Page
    {
        private Device d;
        private Output o;
        private int numTries;
        private bool passed;
        public TOSAStep3()
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
            else if (numTries < 3 && passed)
            {
                NavigationService.Navigate(new HomePage());
                return;
            }

            numTries++;
            Wiggle();
        }

        private async void Wiggle()
        {
            TOSADevice device = d as TOSADevice;
            TOSAOutput output = o as TOSAOutput;

            bool wiggleTestResult = true;

            WiggleData wiggleData = await TestCalculations.TOSAWiggleTest((int)device.Wiggle_Time, wiggleProgress);

            wiggleMin.Text = wiggleData.min.ToString("F") + " mW";
            wiggleMax.Text = wiggleData.max.ToString("F") + " mW";
            wiggleAvg.Text = wiggleData.avg.ToString("F") + " mW";

            double popct_wiggle = wiggleData.min / output.P_Test_OB;
            popctWiggle.Text = (100 * popct_wiggle).ToString("F") + " %";
            bool POPCT_Wiggle_Pass = (popct_wiggle >= device.POPCT_Wiggle_Min);

            output.POPCT_Wiggle_Min = popct_wiggle;
            output.POPCT_Wiggle_Min_Pass = POPCT_Wiggle_Pass;

            if (!POPCT_Wiggle_Pass)
            {
                popctWiggle.Foreground = Brushes.OrangeRed;
                wiggleTestResult = false;
            }

            double pwiggle = 10 * Math.Log10(wiggleData.max / wiggleData.min);
            wiggleDb.Text = pwiggle.ToString("F") + " dB";
            bool Pwiggle_Pass = (pwiggle <= device.Pwiggle_Max);

            output.Wiggle_dB = pwiggle;
            output.Wiggle_dB_Pass = Pwiggle_Pass;

            if (!Pwiggle_Pass)
            {
                wiggleDb.Foreground = Brushes.OrangeRed;
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
                MainWindow.Conn.SaveTOSAOutput(output);
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
                    MainWindow.Conn.SaveTOSAOutput(output);
                }
                else
                {
                    StartTestButton.Content = "Retry test";
                }
            }

            NextDeviceButton.Visibility = Visibility.Visible;
        }

        private void Next_Device_Button_Click(object sender, RoutedEventArgs e)
        {
            var w = Window.GetWindow(this) as MainWindow;

            var d = w.device as TOSADevice;
            var currentOutput = w.output as TOSAOutput;
            var job = currentOutput.Job_Number;

            w.output = new TOSAOutput
            {
                Part_Number = d.Part_Number,
                Job_Number = job,
                Operator = currentOutput.Operator,
                Unit_Number = currentOutput.Unit_Number + 1,
                Timestamp = DateTime.Now
            };

            NavigationService.Navigate(new TOSAStep1());
        }
    }
}
