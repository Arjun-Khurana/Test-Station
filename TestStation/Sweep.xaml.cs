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
            else if (numTries <= 3 && passed)
            {
                NavigationService.Navigate(new Wiggle());
                return;
            }

            numTries++;
            TOSASweep();
        }

        private async void TOSASweep()
        {
            TOSADevice device = d as TOSADevice;
            TOSAOutput output = o as TOSAOutput;

            bool sweepResult = true;

            SweepData sweepData = await TestCalculations.SweepTest(device.I_Start, device.I_Stop, device.I_Step, sweepProgress);

            double r = TestCalculations.FindSlope(sweepData.currents, sweepData.voltages, device.I_OP_Min, device.I_OP_Max) * 1000;
            resistance.Text = r.ToString("F") + " Ω";
            bool R_Pass = (r >= device.RS_Min && r <= device.RS_Max);

            output.RS = r;
            output.RS_Pass = R_Pass;

            if (!R_Pass)
            {
                resistance.Foreground = Brushes.OrangeRed;
                sweepResult = false;
            }

            double se = TestCalculations.FindSlope(sweepData.currents, sweepData.powers, device.I_OP_Min, device.I_OP_Max);
            slopeEfficiency.Text = se.ToString("F");

            bool SE_Pass = (se >= device.SE_Min && se <= device.SE_Max);

            output.SE = se;
            output.SE_Pass = SE_Pass;            

            if (!SE_Pass)
            {
                slopeEfficiency.Foreground = Brushes.OrangeRed;
                sweepResult = false;
            }    

            double ith = TestCalculations.ThresholdCurrent(sweepData, device.I_OP_Min, device.I_OP_Max);
            thresholdCurrent.Text = ith.ToString("F") + " mA";
            bool ITH_Pass = (ith >= device.Ith_Min && ith <= device.Ith_Max);
            
            output.Ith = ith;
            output.Ith_Pass = ITH_Pass;

            measurementPanel.Visibility = Visibility.Visible;

            if (!ITH_Pass)
            {
                thresholdCurrent.Foreground = Brushes.OrangeRed;
                sweepResult = false;
            }

            double p_test = sweepData.powers.ElementAt(sweepData.setcurrents.IndexOf(device.I_Test));
            testPower.Text = p_test.ToString("F") + " mW";
            bool P_Test_Pass = (p_test >= device.P_Test_FC_Min && p_test <= device.P_Test_FC_Max);

            output.P_Test_FC = p_test;
            output.P_Test_FC_Pass = P_Test_Pass;

            if (!P_Test_Pass)
            {
                testPower.Foreground = Brushes.OrangeRed;
                sweepResult = false;
            }

            double popct = 100*output.P_Test_OB / p_test;
            POPCT.Text = popct.ToString("F") + " %";
            bool POPCT_Pass = (popct >= device.P_Test_FC_Min && popct <= device.P_Test_FC_Max);

            output.P_Test_FC = popct;
            output.P_Test_FC_Pass = POPCT_Pass;

            if (!POPCT_Pass)
            {
                testPower.Foreground = Brushes.OrangeRed;
                sweepResult = false;
            }

            double ibm = TestCalculations.IBM_PBM(sweepData, device.P_BM_Test);
            monitorCurrent.Text = ibm.ToString("F") + " mA";
            bool IBM_Pass = (ibm >= device.IBM_Min && ibm <= device.IBM_Max);

            if (!IBM_Pass)
            {
                monitorCurrent.Foreground = Brushes.OrangeRed;
                sweepResult = false;
            }

            //double ibmSlope = TestCalculations.FindSlope(sweepData.powers, sweepData.ibms, device.P_)

            if (sweepResult)
            {
                passed = true;
                testMessage.Text = "Test Passed";
                testMessage.Foreground = Brushes.ForestGreen;
                StartTestButton.Content = "Next step";
                var w = Window.GetWindow(this) as MainWindow;
                d = device;
                w.device = d;
            }
            else
            {
                testMessage.Text = "Test Failed";
                testMessage.Foreground = Brushes.OrangeRed;
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
