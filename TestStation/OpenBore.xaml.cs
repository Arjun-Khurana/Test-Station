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
            else if (numTries < 3 && passed)
            {
                NavigationService.Navigate(new Sweep());
                return;
            }

            numTries++;
            TOSAStep1();
            
        }

        private void TOSAStep1()
        {
            TOSADevice device = d as TOSADevice;
            TOSAOutput output = o as TOSAOutput;

            bool result = true;

            Debug.Print(Instruments.Instance.QuerySMU("CURR:RANG? (@2)"));

            double P_Test_OB = TestCalculations.Power(device.I_Test, 3);

            testPower.Text = P_Test_OB.ToString();
            output.P_Test_OB = P_Test_OB;
            //Debug.Print("P_Total: {0}", P_Total);
            //output.P_Total = P_Total;

            double I = TestCalculations.Current(device.I_Test, 1);
            double V = TestCalculations.Voltage(device.I_Test);

            //Debug.Print("I: {0}", I);
            testCurrent.Text = I.ToString();
            testVoltage.Text = V.ToString();
            output.I_Test = I;
            output.V_Test = V;

            double IBM_Test = TestCalculations.IBM_Test(device.I_Test);
            monitorCurrent.Text = IBM_Test.ToString();
            output.IBM_Test_OB = IBM_Test;

            double IBR = TestCalculations.IBR(device.VBR_Test);

            //Debug.Print("IBR: {0}", IBR);
            reverseBreakdownCurrent.Text = IBR.ToString();
            reverseBreakdownVoltage.Text = device.VBR_Test.ToString();
            output.IBR = IBR;

            if (Math.Abs(device.I_Test - I) > device.I_Test_Tol)
            {
                //result = false;
            }

            if (P_Test_OB < device.P_Test_OB_Min || P_Test_OB > device.P_Test_OB_Max)
            {
                //result = false;
            }

            if (IBR > device.IBR_Max)
            {
                //result = false;
            }

            if (result)
            {
                passed = true;
                StartTestButton.Content = "Next step";
                var w = Window.GetWindow(this) as MainWindow;
                d = device;
                w.device = d;
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
