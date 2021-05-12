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
    /// Interaction logic for OpenBore.xaml
    /// </summary>
    public partial class OpenBore : Page
    {
        private int numTries;
        private bool passed;
        private Device d;
        private Output o;

        //public event EventHandler Initialized;

        public OpenBore()
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
                NavigationService.Navigate(new Sweep());
                return;
            }

            numTries++;
            TOSAOpenBore();
            
        }

        private void TOSAOpenBore()
        {
            TOSADevice device = d as TOSADevice;
            TOSAOutput output = o as TOSAOutput;

            bool openBoreResult = true;

            OBData ob = TestCalculations.OpenBoreTest(device.I_Test, device.VBR_Test);
           
            testCurrent.Text = ob.i_test.ToString("F") + " mA";
            output.I_Test = ob.i_test;

            bool I_Test_Pass = (Math.Abs(device.I_Test - ob.i_test) / device.I_Test <= device.I_Test_Tol);

            if (!I_Test_Pass)
            {
                testCurrent.Foreground = Brushes.OrangeRed;
                openBoreResult = false;
            }

            testPower.Text = ob.p_test.ToString("F") + " mW";
            
            bool P_Test_OB_Pass = (ob.p_test >= device.P_Test_OB_Min && ob.p_test <= device.P_Test_OB_Max);
            
            output.P_Test_OB = ob.p_test;
            output.P_OB_Pass = P_Test_OB_Pass;
            
            if (!P_Test_OB_Pass)
            {
                testPower.Foreground = Brushes.OrangeRed;
                openBoreResult = false;
            }
            
            testVoltage.Text = ob.v_test.ToString("F") + " V";

            bool V_Test_OB_Pass = (ob.v_test >= device.V_Test_Min && ob.v_test <= device.V_Test_Max);

            output.V_Test = ob.v_test;
            output.V_Test_Pass = V_Test_OB_Pass;

            if (!V_Test_OB_Pass)
            {
                testVoltage.Foreground = Brushes.OrangeRed;
                openBoreResult = false;
            }

            monitorCurrent.Text = ob.ibm_test.ToString("F") + " mA";

            bool IBM_Test_Pass = (ob.ibm_test >= device.IBM_Min && ob.ibm_test <= device.IBM_Max);

            if (!IBM_Test_Pass)
            {
                monitorCurrent.Foreground = Brushes.OrangeRed;
                openBoreResult = false;
            }

            output.IBM_Test_OB = ob.ibm_test;
            output.IBM_Pass = IBM_Test_Pass;

            reverseBreakdownCurrent.Text = ob.ibr.ToString("F") + " µA";
            reverseBreakdownVoltage.Text = device.VBR_Test.ToString("F") + " V";

            bool IBR_Pass = (Math.Abs(ob.ibr) <= device.IBR_Max);

            if (!IBR_Pass)
            {
                reverseBreakdownCurrent.Foreground = Brushes.OrangeRed;
                openBoreResult = false;
            }

            output.IBR = ob.ibr;
            output.IBR_Pass = IBR_Pass;

            measurementPanel.Visibility = Visibility.Visible;

            if (openBoreResult)
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
    }
}
