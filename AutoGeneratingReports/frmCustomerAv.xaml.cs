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
    /// Interaction logic for frmCustomerAv.xaml
    /// </summary>
    public partial class frmCustomerAv : Window
    {
        public frmCustomerAv()
        {
            frmCustomerAvViewModel VM = new frmCustomerAvViewModel();
            this.DataContext = VM;
            if (VM.CloseAction == null)
            {
                VM.CloseAction = new Action(() => this.Close());
            }
            InitializeComponent();
        }
        public frmCustomerAv(string m_strPosName)
        {
            frmCustomerAvViewModel VM = new frmCustomerAvViewModel(m_strPosName);
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
            DSQuayAvWindow dSQuayAv = new DSQuayAvWindow();
            dSQuayAv.ShowDialog();
        }

        private void Barcode_TableRow(object sender, DataGridRowEventArgs e)
        {
            DataGridAVRow.Columns[0].Visibility = Visibility.Hidden;
        }
    }
}
