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
    /// Interaction logic for OpenBore.xaml
    /// </summary>
    public partial class OpenBore : Page
    {
        private int numTries;
        private bool passed;
        private Device d;
        private Output o;
        public OpenBore()
        {
            InitializeComponent();
            numTries = 0;
            passed = false;

            var w = Window.GetWindow(this) as MainWindow;
            d = w.device;
            o = w.output;
        }

        private void Start_Test_Button_Click(object sender, RoutedEventArgs e)
        {
            if (numTries == 3 && !passed)
            {
                NavigationService.Navigate(new HomePage());
                return;
            }
            else if (numTries < 3 && passed)
            {
                NavigationService.Navigate(new Sweep());
            }

            numTries++;
            TOSAStep1();
            
        }

        private void TOSAStep1()
        {
            TOSADevice device = d as TOSADevice;
            TOSAOutput output = o as TOSAOutput;

            bool result = true;

            double I_Test = device.I_Test;

            double P_Total = TestCalculations.Power(I_Test);
            double I = TestCalculations.Current(I_Test, 1);

            double IBR = TestCalculations.IBR(device.VBR_Test);

            if (Math.Abs(I_Test - I) > device.I_Test_Tol)
            {
                result = false;
            }
            
            if (P_Total < device.P_Total_Min || P_Total > device.P_Total_Max)
            {
                result = false;
            }
            
            if (IBR > device.IBR_Max)
            {
                result = false;
            }

            if (result)
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
