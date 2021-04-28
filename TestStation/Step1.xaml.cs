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

namespace TestStation
{
    /// <summary>
    /// Interaction logic for Step1.xaml
    /// </summary>
    public partial class Step1 : Page
    {
        private int numTries;
        public Step1()
        {
            InitializeComponent();
            numTries = 0;
        }

        private void Start_Test_Button_Click(object sender, RoutedEventArgs e)
        {
            if (numTries == 3)
            {
                NavigationService.Navigate(new HomePage());
                return;
            }

            numTries++;
            TOSAStep1();

            
        }

        private void TOSAStep1()
        {
            bool continuityTestResult = false;
            bool sweepTestResult = true;

            if (continuityTestResult && sweepTestResult)
            {
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
