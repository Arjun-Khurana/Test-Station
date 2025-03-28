﻿using System;
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
            string[] inputs =
            {
                "Part_Number",
                "Job_Number",
                "Unit_Number",
                "Operator",
                "Timestamp",
                "Repeat_Number",
                "I_Test",
                "P_Test_OB",
                "P_OB_Pass",
                "V_Test",
                "V_Test_Pass",
                "IBM_Test_OB",
                "IBM_Pass",
                "VBR_Test",
                "IBR",
                "IBR_Pass",
                "P_Test_FC",
                "P_Test_FC_Pass",
                "RS",
                "RS_Pass",
                "SE",
                "SE_Pass",
                "Ith",
                "Ith_Pass",
                "POPCT",
                "POPCT_Pass",
                "I_BM_Slope",
                "I_BM_P_BM_Test",
                "I_BM_P_BM_Test_Pass",
                "IBM_Track",
                "I_BM_Track_Pass",
                "POPCT_Wiggle_Min",
                "POPCT_Wiggle_Min_Pass",
                "Wiggle_dB",
                "Wiggle_dB_Pass",
                "Result"
            };

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

            foreach (string s in inputs)
            {
                Debug.Print("{0} = result.{0},", s);
            }

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
            bool nullsFound = false;

            foreach (TextBox tb in Utils.FindVisualChildren<TextBox>(newROSAPanel))
            {
                if (String.IsNullOrEmpty(tb.Text))
                {
                    tb.Style = (Style)Application.Current.Resources["ErrorTextField"];
                    nullsFound = true;
                }
            }

            if (nullsFound) return;

            var rosa = new Models.ROSADevice
            {
                Part_Number = ROSA_Part_Number_Input.Text,
                V_Test = Double.Parse(ROSA_V_Test_Input.Text),
                RESP_Min = Double.Parse(ROSA_RESP_Min_Input.Text),
                I_TIA_Max = Double.Parse(ROSA_ICC_Max_Input.Text),
                PD_Wiggle_Max = Double.Parse(ROSA_PD_Wiggle_Max_Input.Text),
                Wiggle_Time = Double.Parse(ROSA_Wiggle_Time_Input.Text),
                RSSI_VPD = ROSA_RSSI_VPD_Input.Text
            };

            MainWindow.Conn.SaveROSADevice(rosa);

            newROSAPanel.Visibility = Visibility.Collapsed;
            addNewROSAButton.Visibility = Visibility.Visible;
            addNewTOSAButton.Visibility = Visibility.Visible;
        }

        private void Cancel_Rosa_Click(object sender, RoutedEventArgs e)
        {
            foreach (TextBox tb in Utils.FindVisualChildren<TextBox>(newROSAPanel))
            {
                tb.Text = null;
                tb.Style = (Style)Application.Current.Resources["RegularTextField"];
            }

            newROSAPanel.Visibility = Visibility.Collapsed;
            addNewROSAButton.Visibility = Visibility.Visible;
            addNewTOSAButton.Visibility = Visibility.Visible;
        }

        private void Save_TOSA_Device_Button_Click(object sender, RoutedEventArgs e)
        {
            bool nullsFound = false; 

            foreach (TextBox tb in Utils.FindVisualChildren<TextBox>(newTOSAPanel))
            {
                if (String.IsNullOrEmpty(tb.Text))
                {
                    tb.Style = (Style)Application.Current.Resources["ErrorTextField"];
                    nullsFound = true;
                }
            }

            if (nullsFound) return;

            var tosa = new Models.TOSADevice
            {
                Part_Number = TOSA_Part_Number_Input.Text,
                I_Start = Double.Parse(TOSA_I_Start_Input.Text),
                I_Step = Double.Parse(TOSA_I_Step_Input.Text),
                I_Stop = Double.Parse(TOSA_I_Stop_Input.Text),
                I_Test = Double.Parse(TOSA_I_Test_Input.Text),
                P_Test_OB_Min = Double.Parse(TOSA_P_Test_OB_Min_Input.Text),
                P_Test_OB_Max = Double.Parse(TOSA_P_Test_OB_Max_Input.Text),
                V_Test_Min = Double.Parse(TOSA_V_Test_Min_Input.Text),
                V_Test_Max = Double.Parse(TOSA_V_Test_Max_Input.Text),
                VBR_Test = Double.Parse(TOSA_VBR_Test_Input.Text),
                IBM_Min = Double.Parse(TOSA_IBM_Min_Input.Text),
                IBM_Max = Double.Parse(TOSA_IBM_Max_Input.Text),
                P_Test_FC_Min = Double.Parse(TOSA_P_Test_FC_Min_Input.Text),
                P_Test_FC_Max = Double.Parse(TOSA_P_Test_FC_Max_Input.Text),
                I_OP_Min = Double.Parse(TOSA_I_OP_Min_Input.Text),
                I_OP_Max = Double.Parse(TOSA_I_OP_Max_Input.Text),
                P_BM_Test = Double.Parse(TOSA_P_BM_Test_Input.Text),
                POPCT_Min = Double.Parse(TOSA_POPCT_Min_Input.Text),
                IBM_Tracking_Min = Double.Parse(TOSA_IBM_Tracking_Min_Input.Text),
                IBM_Tracking_Max = Double.Parse(TOSA_IBM_Tracking_Max_Input.Text),
                RS_Min = Double.Parse(TOSA_RS_Min_Input.Text),
                RS_Max = Double.Parse(TOSA_RS_Max_Input.Text),
                SE_Min = Double.Parse(TOSA_SE_Min_Input.Text),
                SE_Max = Double.Parse(TOSA_SE_Max_Input.Text),
                Ith_Min = Double.Parse(TOSA_Ith_Min_Input.Text),
                Ith_Max = Double.Parse(TOSA_Ith_Max_Input.Text),
                Wiggle_Time = Double.Parse(TOSA_Wiggle_Time_Input.Text),
                Pwiggle_Max = Double.Parse(TOSA_Pwiggle_Max_Input.Text),
                IBR_Max = Double.Parse(TOSA_IBR_Max_Input.Text),
                POPCT_Wiggle_Min = Double.Parse(TOSA_POPCT_Wiggle_Min_Input.Text)
            };

            MainWindow.Conn.SaveTOSADevice(tosa);

            newTOSAPanel.Visibility = Visibility.Collapsed;
            addNewROSAButton.Visibility = Visibility.Visible;
            addNewTOSAButton.Visibility = Visibility.Visible;
        }

        private void Cancel_Tosa_Click(object sender, RoutedEventArgs e)
        {
            foreach (TextBox tb in Utils.FindVisualChildren<TextBox>(newTOSAPanel))
            {
                tb.Text = null;
                tb.Style = (Style)Application.Current.Resources["RegularTextField"];
            }    

            newTOSAPanel.Visibility = Visibility.Collapsed;
            addNewROSAButton.Visibility = Visibility.Visible;
            addNewTOSAButton.Visibility = Visibility.Visible;
        }

        private void String_Input_Text_Changed(object sender, TextChangedEventArgs e)
        {
            TextBox t = (TextBox)sender;
            var empty = String.IsNullOrEmpty(t.Text);

            if (empty)
            {
                t.Style = (Style)Application.Current.Resources["ErrorTextField"];
            }
            else
            {
                t.Style = (Style)Application.Current.Resources["RegularTextField"];
            }
        }

        private void Double_Input_Text_Changed(object sender, TextChangedEventArgs e)
        {
            double vpd;
            TextBox t = (TextBox)sender;
            var parsed = Double.TryParse(t.Text, out vpd);

            if (parsed)
            {

                t.Style = (Style)Application.Current.Resources["RegularTextField"];
            }
            else
            {
                t.Style = (Style)Application.Current.Resources["ErrorTextField"];
            }
        }
    }
}
