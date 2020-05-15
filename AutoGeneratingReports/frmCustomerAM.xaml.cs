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
    /// Interaction logic for frmCustomerAM.xaml
    /// </summary>
    public partial class frmCustomerAM : Window
    {
        public frmCustomerAM()
        {
            frmCustomerAmViewModel VM = new frmCustomerAmViewModel();
            this.DataContext = VM;
            if (VM.CloseAction == null)
            {
                VM.CloseAction = new Action(() => this.Close());
            }
            InitializeComponent();
        }
        public frmCustomerAM(int idCust)
        {
            frmCustomerAmViewModel VM = new frmCustomerAmViewModel(idCust);
            this.DataContext = VM;
            if (VM.CloseAction == null)
            {
                VM.CloseAction = new Action(() => this.Close());
            }
            InitializeComponent();
        }

        private void IconButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            QuanLyKhachHangAeonMall quanLyKhach = new QuanLyKhachHangAeonMall();
            quanLyKhach.ShowDialog();


        }
    }
}
