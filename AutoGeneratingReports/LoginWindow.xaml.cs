using AutoGeneratingReports.Common;
using AutoGeneratingReports.Custom;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AutoGeneratingReports
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        MainWindow MainWindow = new MainWindow();
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Login(object sender, RoutedEventArgs e)
        {
            if (TaiKhoan.Text.Length == 0)
            {
                MessageBox.Show("Vui lòng điền tài khoản.");
                TaiKhoan.Focus();
            }
            //else if (!Regex.IsMatch(TaiKhoan.Text, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
            //{
            //    errormessage.Text = "Enter a valid email.";
            //    TaiKhoan.Select(0, TaiKhoan.Text.Length);
            //    TaiKhoan.Focus();
            //}
            else
            {
                string taikhoan = TaiKhoan.Text;
                string matkhau = MatKhau.Password;
                SqlConnection con = new SqlConnection(DataSource.ConnectionString());
                con.Open();
                SqlCommand cmd = new SqlCommand("Select * from Users where Username='" + taikhoan + "'  and Password='" + matkhau + "'", con);
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);
                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    
                    MainWindow.Show();
                    Close();
                }
                else
                {
                    MessageBox.Show("Sorry! Please enter existing emailid/password.");
                }
                con.Close();
            }
        }

    }
}
