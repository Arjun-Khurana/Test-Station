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

namespace TestStation
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Page
    {

        public Settings()
        {
            InitializeComponent();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void New_Rosa_Button_Click(object sender, RoutedEventArgs e)
        {
            //string[] inputs =
            //{
            //    "V_Test",
            //    "RESP_Min",
            //    "ICC_Max",
            //    "I_Wiggle_Max",
            //    "Wiggle_Time",
            //    "RSSSI_VPD"
            //};

            //Debug.Print("@\"insert into ROSADevice\n(");
            //foreach (string s in inputs)
            //{
            //    Debug.Print("\t{0},", s);
            //}
            //Debug.Print(")\nvalues\n(");
            //foreach (string s in inputs)
            //{
            //    Debug.Print("\t@{0},", s);
            //}
            //Debug.Print(")\"");
            //Debug.Print("new {");
            //foreach (string s in inputs)
            //{
            //    Debug.Print("\t{0} = device.{0},", s);
            //}
            //Debug.Print("}");

            newROSAPanel.Visibility = Visibility.Visible;
            addNewROSAButton.Visibility = Visibility.Collapsed;
            addNewTOSAButton.Visibility = Visibility.Collapsed;
        }

        private void New_Tosa_Button_Click(object sender, RoutedEventArgs e)
        {
            //string[] inputs =
            //{
            //    "I_Continuity",
            //    "V_Continuity_Min",
            //    "V_Continuity_Max",
            //    "I_Continuity_Tol",
            //    "I_Start",
            //    "I_Step",
            //    "I_Stop",
            //    "P_OP",
            //    "I_OP_Min",
            //    "I_OP_Max",
            //    "Wiggle_Time",
            //    "VBR_Test",
            //    "V_OP_Min",
            //    "V_OP_Max",
            //    "RS_Min",
            //    "RS_Max",
            //    "SE_Min",
            //    "SE_Max",
            //    "Ith_Min",
            //    "Ith_Max",
            //    "Pwiggle_Max",
            //    "POPCT_Min",
            //    "IBM_Min",
            //    "IBM_Max",
            //    "IBM_Tracking_Min",
            //    "IBM_Tracking_Max",
            //    "IBR_Max"
            //};

            //Debug.Print("@\"insert into TOSADevice\n(");
            //foreach (string s in inputs)
            //{
            //    Debug.Print("\t{0},", s);
            //}
            //Debug.Print(")\nvalues\n(");
            //foreach (string s in inputs)
            //{
            //    Debug.Print("\t@{0},", s);
            //}
            //Debug.Print(")\"");
            //Debug.Print("new {");
            //foreach (string s in inputs)
            //{
            //    Debug.Print("\t{0} = device.{0},", s);
            //}
            //Debug.Print("}");

            //foreach (string s in inputs)
            //{
            //    Debug.Print("{0} = Double.Parse(TOSA_{0}_Input.Text),",s);
            //}

            //foreach (string s in inputs)
            //{
            //    Debug.Print("<StackPanel Margin=\"10, 0, 10, 10\" Orientation=\"Vertical\">");
            //    Debug.Print("\t<Label Padding=\"0, 0, 0, 5\" Content=\"{0}\"></Label>", s);
            //    Debug.Print("\t<TextBox Width=\"100\" x:Name=\"TOSA_{0}_Input\"></TextBox>", s);
            //    Debug.Print("</StackPanel>");
            //    Debug.WriteLine("");
            //}


            newTOSAPanel.Visibility = Visibility.Visible;
            addNewROSAButton.Visibility = Visibility.Collapsed;
            addNewTOSAButton.Visibility = Visibility.Collapsed;
        }

        private void Save_ROSA_Device_Button_Click(object sender, RoutedEventArgs e)
        {
            var rosa = new Models.ROSADevice
            {
                Part_Number = ROSA_Part_Number_Input.Text,
                V_Test = Double.Parse(ROSA_V_Test_Input.Text),
                RESP_Min = Double.Parse(ROSA_RESP_Min_Input.Text),
                ICC_Max = Double.Parse(ROSA_ICC_Max_Input.Text),
                I_Wiggle_Max = Double.Parse(ROSA_I_Wiggle_Max_Input.Text),
                Wiggle_Time = Double.Parse(ROSA_Wiggle_Time_Input.Text),
                RSSSI_VPD = Double.Parse(ROSA_RSSSI_VPD_Input.Text)
            };

            MainWindow.Conn.SaveROSADevice(rosa);

            newROSAPanel.Visibility = Visibility.Collapsed;
            addNewROSAButton.Visibility = Visibility.Visible;
            addNewTOSAButton.Visibility = Visibility.Visible;
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        private void Cancel_Rosa_Click(object sender, RoutedEventArgs e)
        {
            newROSAPanel.Visibility = Visibility.Collapsed;
            addNewROSAButton.Visibility = Visibility.Visible;
            addNewTOSAButton.Visibility = Visibility.Visible;
        }

        private void Save_TOSA_Device_Button_Click(object sender, RoutedEventArgs e)
        {
            var tosa = new Models.TOSADevice
            {
                Part_Number = TOSA_Part_Number_Input.Text,
                I_Continuity = Double.Parse(TOSA_I_Continuity_Input.Text),
                V_Continuity_Min = Double.Parse(TOSA_V_Continuity_Min_Input.Text),
                V_Continuity_Max = Double.Parse(TOSA_V_Continuity_Max_Input.Text),
                I_Continuity_Tol = Double.Parse(TOSA_I_Continuity_Tol_Input.Text),
                I_Start = Double.Parse(TOSA_I_Start_Input.Text),
                I_Step = Double.Parse(TOSA_I_Step_Input.Text),
                I_Stop = Double.Parse(TOSA_I_Stop_Input.Text),
                P_OP = Double.Parse(TOSA_P_OP_Input.Text),
                I_OP_Min = Double.Parse(TOSA_I_OP_Min_Input.Text),
                I_OP_Max = Double.Parse(TOSA_I_OP_Max_Input.Text),
                Wiggle_Time = Double.Parse(TOSA_Wiggle_Time_Input.Text),
                VBR_Test = Double.Parse(TOSA_VBR_Test_Input.Text),
                V_OP_Min = Double.Parse(TOSA_V_OP_Min_Input.Text),
                V_OP_Max = Double.Parse(TOSA_V_OP_Max_Input.Text),
                RS_Min = Double.Parse(TOSA_RS_Min_Input.Text),
                RS_Max = Double.Parse(TOSA_RS_Max_Input.Text),
                SE_Min = Double.Parse(TOSA_SE_Min_Input.Text),
                SE_Max = Double.Parse(TOSA_SE_Max_Input.Text),
                Ith_Min = Double.Parse(TOSA_Ith_Min_Input.Text),
                Ith_Max = Double.Parse(TOSA_Ith_Max_Input.Text),
                Pwiggle_Max = Double.Parse(TOSA_Pwiggle_Max_Input.Text),
                POPCT_Min = Double.Parse(TOSA_POPCT_Min_Input.Text),
                IBM_Min = Double.Parse(TOSA_IBM_Min_Input.Text),
                IBM_Max = Double.Parse(TOSA_IBM_Max_Input.Text),
                IBM_Tracking_Min = Double.Parse(TOSA_IBM_Tracking_Min_Input.Text),
                IBM_Tracking_Max = Double.Parse(TOSA_IBM_Tracking_Max_Input.Text),
                IBR_Max = Double.Parse(TOSA_IBR_Max_Input.Text)
            };

            MainWindow.Conn.SaveTOSADevice(tosa);

            newTOSAPanel.Visibility = Visibility.Collapsed;
            addNewROSAButton.Visibility = Visibility.Visible;
            addNewTOSAButton.Visibility = Visibility.Visible;
        }

        private void Cancel_Tosa_Click(object sender, RoutedEventArgs e)
        {
            newTOSAPanel.Visibility = Visibility.Collapsed;
            addNewROSAButton.Visibility = Visibility.Visible;
            addNewTOSAButton.Visibility = Visibility.Visible;
        }
    }
}
