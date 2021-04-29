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
        public Models.TOSADevice tosaDevice { get; set; }
        public Step1()
        {
            InitializeComponent();
        }
       
        private void Start_Test_Button_Click(object sender, RoutedEventArgs e)
        {
            ContinuityTest();
        }

        private void ContinuityTest()
        {
            try
            {
                bool continuityTestResult = true;
                int tries = 0;

                do
                {
                    if (continuityTestResult == false)
                        MessageBox.Show("Reinsert and check orientation");

                    continuityTestResult = TestCalculations.ContinuityTest(tosaDevice.I_Continuity,
                                                            tosaDevice.V_Continuity_Min,
                                                            tosaDevice.V_Continuity_Max,
                                                            tosaDevice.I_Continuity_Tol);
                    tries++;
                }
                while (!continuityTestResult && tries < 3);

                if(continuityTestResult == true)
                {
                    MessageBox.Show("Success!");
                    //NavigationService.Navigate(new Step2());
                    return;
                }
                else
                {
                    MessageBox.Show("Failure!");
                    return;
                }

            }
            catch (Exception e)
            {
                MessageBox.Show("Failure!");
            }
        }
    }
}
