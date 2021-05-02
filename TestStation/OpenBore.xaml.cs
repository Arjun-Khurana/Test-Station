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
        public OpenBore()
        {
            InitializeComponent();
            numTries = 0;
            passed = false;

            var w = Window.GetWindow(this) as MainWindow;
            d = w.device;
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
            bool continuityTestResult = TestCalculations.ContinuityTest(device.I_Continuity, device.V_Continuity_Min, device.V_Continuity_Max, device.I_Continuity_Tol);

            if (continuityTestResult)
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
