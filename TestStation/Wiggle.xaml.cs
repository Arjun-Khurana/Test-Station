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
    public partial class Wiggle : Page
    {
        private Device d;
        private Output o;
        private int numTries;
        private bool passed;
        public Wiggle()
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
                NavigationService.Navigate(new Wiggle());
                return;
            }

            numTries++;
            TOSAStep3();
        }

        private async void TOSAStep3()
        {
            TOSADevice device = d as TOSADevice;
            TOSAOutput output = o as TOSAOutput;

            bool wiggleTestResult = true;

            WiggleData wiggleData = await TestCalculations.WiggleTest((int)device.Wiggle_Time, wiggleProgress);

            Debug.Print("min: {0} max: {1} avg: {2}", wiggleData.min, wiggleData.max, wiggleData.avg);
        }
    }
}
