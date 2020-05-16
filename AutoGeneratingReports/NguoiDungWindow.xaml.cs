using AutoGeneratingReports.ViewModel;
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
using System.Windows.Shapes;

namespace AutoGeneratingReports
{
    /// <summary>
    /// Interaction logic for NguoiDungWindow.xaml
    /// </summary>
    public partial class NguoiDungWindow : Window
    {
        public NguoiDungWindow()
        {
            UserViewModel Vm = new UserViewModel();
            this.DataContext = Vm;
            if (Vm.CloseAction == null)
            {
                Vm.CloseAction = new Action(() => this.Close());
            }
            InitializeComponent();
        }
        public NguoiDungWindow(string UserName)
        {
            UserViewModel Vm = new UserViewModel(UserName);
            this.DataContext = Vm;
            Vm.TenDangNhap = UserName;
            //if (Vm.CloseAction == null)
            //{
            //    Vm.CloseAction = new Action(() => this.Close());
            //}
            InitializeComponent();
        }

        private void DataGridCheckBox_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            DataGridCheckBox.Columns[0].Visibility = Visibility.Hidden;
        }

        private void IconButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            DSNguoiDungWindow dSNguoiDung = new DSNguoiDungWindow();
            dSNguoiDung.Show();
        }
    }
}
