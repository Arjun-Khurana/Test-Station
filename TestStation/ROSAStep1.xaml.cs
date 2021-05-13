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
using System.Diagnostics;
using TestStation.Models;

namespace TestStation
{
    /// <summary>
    /// Interaction logic for DarkLight.xaml
    /// </summary>
    public partial class ROSAStep1 : Page
    {
        private int numTries;
        private bool passed;
        private Device d;
        private Output o;

        public ROSAStep1()
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
                NavigationService.Navigate(new ROSAStep2());
                return;
            }

            numTries++;
            DarkLight();

        }

        private void DarkLight()
        {
            ROSADevice device = d as ROSADevice;
            ROSAOutput output = o as ROSAOutput;

            bool darkLightResult = true;

            DLData dl = TestCalculations.DarkLightTest(device.V_Test, (device.RSSI_VPD == "VPD"));

            foreach (TextBlock tb in Utils.FindVisualChildren<TextBlock>(measurementPanel))
            {
                tb.Foreground = Brushes.White;
            }

            vTest.Text = device.V_Test.ToString("F") + " V";
            output.V_Test = device.V_Test;

            iTIA.Text = dl.i_tia.ToString("F") + " mA";
            bool I_TIA_Pass = (dl.i_tia <= device.I_TIA_Max);

            output.I_TIA = dl.i_tia;
            output.I_TIA_Pass = I_TIA_Pass;

            if (!I_TIA_Pass)
            {
                iTIA.Foreground = Brushes.OrangeRed;
                darkLightResult = false;
            }

            iDark.Text = dl.i_dark.ToString("F") + " mA";

            output.I_Dark = dl.i_dark;

            iPD.Text = dl.i_pd.ToString("F") + " mA";

            responsivity.Text = dl.responsivity.ToString("F");
            bool Responsivity_Pass = (dl.responsivity >= device.RESP_Min);

            output.Responsivity = dl.responsivity;
            output.RESP_Pass = Responsivity_Pass;

            if (!Responsivity_Pass)
            {
                responsivity.Foreground = Brushes.OrangeRed;
                darkLightResult = false;
            }

            measurementPanel.Visibility = Visibility.Visible;
            var w = Window.GetWindow(this) as MainWindow;

            darkLightResult = true;

            if (darkLightResult)
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
                    MainWindow.Conn.SaveROSAOutput(output);
                }
                else
                {
                    StartTestButton.Content = "Retry test";
                }
            }
        }
    }
}
