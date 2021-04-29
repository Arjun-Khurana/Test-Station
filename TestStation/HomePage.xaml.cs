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
using TestStation.Data;
using TestStation.Models;

namespace TestStation
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        private List<TOSADevice> TOSADevices { get; set; } = new List<TOSADevice>();
        private List<ROSADevice> ROSADevices { get; set; } = new List<ROSADevice>();
        public HomePage()
        {
            InitializeComponent();

            TOSADevices.AddRange(MainWindow.Conn.GetAllTosaDevices());
            ROSADevices.AddRange(MainWindow.Conn.GetAllRosaDevices());
        }


        private void StartButton(object sender, RoutedEventArgs e)
        {
            TOSADevice td = (TOSADevice)DeviceSelector.SelectedItem;
            Step1 step1 = new Step1();
            step1.tosaDevice = td;
            NavigationService.Navigate(step1);
        }

        private void DeviceSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Settings());
        }

        private void ROSA_Radio_Checked(object sender, RoutedEventArgs e)
        {
            DeviceSelector.ItemsSource = ROSADevices;
        }

        private void TOSA_Radio_Checked(object sender, RoutedEventArgs e)
        {
            DeviceSelector.ItemsSource = TOSADevices;
        }

        private void DeviceSelector_MouseEnter(object sender, MouseEventArgs e)
        {
            if ((bool)TOSARadio.IsChecked)
            {
                TOSADevices.Clear();
                TOSADevices.AddRange(MainWindow.Conn.GetAllTosaDevices());
                DeviceSelector.ItemsSource = new List<TOSADevice>();
                DeviceSelector.ItemsSource = TOSADevices;
            }
            else if ((bool)ROSARadio.IsChecked)
            {
                ROSADevices.Clear();
                ROSADevices.AddRange(MainWindow.Conn.GetAllRosaDevices());
                DeviceSelector.ItemsSource = new List<ROSADevice>();
                DeviceSelector.ItemsSource = ROSADevices;
            }
        }
    }
}
