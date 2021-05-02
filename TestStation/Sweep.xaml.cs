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
    /// Interaction logic for Sweep.xaml
    /// </summary>
    public partial class Sweep : Page
    {
        private Device d;
        public Sweep()
        {
            InitializeComponent();

            var w = Window.GetWindow(this) as MainWindow;
            d = w.device;
        }

        private int numTries;
        private bool passed;

        private void Start_Test_Button_Click(object sender, RoutedEventArgs e)
        {
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

        private void TOSAStep1()
        {
            TOSADevice device = d as TOSADevice;

            SweepValue sweepValues = TestCalculations.SweepTest(device.I_Start, device.I_Stop, device.I_Step);
            double resistance = TestCalculations.Resistance(sweepValues, device.I_OP_Min, device.I_OP_Max);
            double slopeEfficiency = TestCalculations.SlopeEfficiency(sweepValues, device.I_OP_Min, device.I_OP_Max);
            double threshholdCurrent = TestCalculations.ThresholdCurrent(sweepValues, slopeEfficiency);
            

            if ()

            if (sweepTestResult)
            {
                passed = true;
                StartTestButton.Content = "Next step";
            }
            else
            {
                if (numTries >= 3)
                {
                    StartTestButton.Content = "Go home";
                }
                else
                {
                    StartTestButton.Content = "Retry test";
                }
            }
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
