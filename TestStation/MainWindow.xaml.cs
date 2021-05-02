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
using TestStation.Data;
using TestStation.Models;

namespace TestStation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static DeviceRepository Conn = new DeviceRepository();
        public Device device;
        public TOSAOutput tosa_output;
        public MainWindow()
        {
            InitializeComponent();
            _mainFrame.Navigate(new HomePage());
        }
    }
}
