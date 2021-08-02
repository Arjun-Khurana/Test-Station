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
using Microsoft.Win32;
using System.Numerics;
using System.IO;

namespace TestStation
{
    /// <summary>
    /// Interaction logic for Sweep.xaml
    /// </summary>
    public partial class TOSAStep2 : Page
    {
        private Device d;
        private Output o;
        private int numTries;
        private bool passed;
        SweepData sd;
        public TOSAStep2()
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
                NavigationService.Navigate(new TOSAStep3());
                return;
            }

            numTries++;

            Sweep();
        }

        private async void Sweep()
        {
            TOSADevice device = d as TOSADevice;
            TOSAOutput output = o as TOSAOutput;

            bool sweepResult = true;

            SweepData sweepData = await TestCalculations.SweepTest(device.I_Start, device.I_Stop, device.I_Step, sweepProgress);
            sd = sweepData;

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

            double popct = p_test/output.P_Test_OB;
            POPCT.Text = (100*popct).ToString("F") + " %";
            bool POPCT_Pass = (popct >= device.POPCT_Min);

            output.POPCT = popct;
            output.POPCT_Pass = POPCT_Pass;

            if (!POPCT_Pass)
            {
                testPower.Foreground = Brushes.OrangeRed;
                sweepResult = false;
            }

            double ibm = TestCalculations.IBM_PBM(sweepData, device.P_BM_Test);
            monitorCurrent.Text = ibm.ToString("F") + " mA";
            bool IBM_Pass = (ibm >= device.IBM_Min && ibm <= device.IBM_Max);

            output.I_BM_P_BM_Test = ibm;
            output.I_BM_P_BM_Test_Pass = IBM_Pass;

            if (!IBM_Pass)
            {
                monitorCurrent.Foreground = Brushes.OrangeRed;
                sweepResult = false;
            }

            double pmin = sweepData.powers.ElementAt(sweepData.setcurrents.IndexOf(device.I_OP_Min));
            double pmax = sweepData.powers.ElementAt(sweepData.setcurrents.IndexOf(device.I_OP_Max));


            double ibmslope = TestCalculations.FindSlope(sweepData.powers, sweepData.ibms, pmin, pmax);
            ibmSlope.Text = ibmslope.ToString("F") + " A/W";
            output.I_BM_Slope = ibmslope;

            double ibmtrack = TestCalculations.IBM_Track(sweepData, device.I_OP_Min, device.I_OP_Max);
            ibmTrack.Text = ibmtrack.ToString("F");
            bool IBM_Track_Pass = (ibmtrack >= device.IBM_Tracking_Min && ibmtrack <= device.IBM_Tracking_Max);

            output.IBM_Track = ibmtrack;
            output.I_BM_Track_Pass = IBM_Track_Pass;

            if (!IBM_Track_Pass)
            {
                ibmTrack.Foreground = Brushes.OrangeRed;
                sweepResult = false;
            }

            measurementPanel.Visibility = Visibility.Visible;
            var w = Window.GetWindow(this) as MainWindow;

            if (sweepResult)
            {
                passed = true;
                testMessage.Text = "Test Passed";
                testMessage.Foreground = Brushes.ForestGreen;
                StartTestButton.Content = "Next step";
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

            SaveLIButton.Visibility = Visibility.Visible;
        }

        private void Save_LI_Button_Click(object sender, RoutedEventArgs e)
        {
            var w = MainWindow.GetWindow(this) as MainWindow;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.OverwritePrompt = false;
            saveFileDialog.Filter = "CSV|*.csv";
            if (saveFileDialog.ShowDialog() == true)
            {
                var stream = File.Open(saveFileDialog.FileName, FileMode.Append);
                using (StreamWriter file = new StreamWriter(stream))
                {
                    file.WriteLine("Unit Number,Set Current,Current,Voltage,Power,Monitor Current");
                    for (int i = 0; i < sd.setcurrents.Count; i++)
                    {
                        file.WriteLine("{0},{1},{2},{3},{4},{5}", w.output.Unit_Number, sd.setcurrents[i], sd.currents[i], sd.voltages[i], sd.powers[i], sd.ibms[i]);
                    }
                }
            }
            //string setcurrents = String.Join(",", sd.setcurrents.Select(x => x.ToString()).ToArray());
            //string currents = String.Join(",", sd.currents.Select(x => x.ToString()).ToArray());
            //string voltages = String.Join(",", sd.voltages.Select(x => x.ToString()).ToArray());
            //string powers = String.Join(",", sd.powers.Select(x => x.ToString()).ToArray());
            //string ibms = String.Join(",", sd.ibms.Select(x => x.ToString()).ToArray());
        }
    }
}
