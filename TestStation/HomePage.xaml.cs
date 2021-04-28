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
        public HomePage()
        {
            InitializeComponent();
            TOSADevices.AddRange(MainWindow.Conn.GetAllTosaDevices());
        }


        private void StartButton(object sender, RoutedEventArgs e)
        {
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

        }

        private void TOSA_Radio_Checked(object sender, RoutedEventArgs e)
        {
            DeviceSelector.ItemsSource = TOSADevices;
        }
    }
}
