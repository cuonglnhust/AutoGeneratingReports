using AutoGeneratingReports.Common;
using AutoGeneratingReports.ViewModel;
using AutoGenReport.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for QuanLyKhachHangAeonMall.xaml
    /// </summary>
    public partial class QuanLyKhachHangAeonMall : Window
    {
        public QuanLyKhachHangAeonMall()
        {
            CustomerManagementAmViewModel VM = new CustomerManagementAmViewModel();
            this.DataContext = VM;
            if (VM.CloseAction == null)
            {
                VM.CloseAction = new Action(() => this.Close());
            }
            InitializeComponent();
        }

        private void DataTableCustomerManagerAM_Loading(object sender, DataGridRowEventArgs e)
        {
            DataTableCustomerManagerAM.Columns[1].Visibility = Visibility.Collapsed;
            DataTableCustomerManagerAM.Columns[3].Visibility = Visibility.Hidden;
            DataTableCustomerManagerAM.Columns[5].Visibility = Visibility.Hidden;
            DataTableCustomerManagerAM.Columns[9].Visibility = Visibility.Hidden;

            
            DataTableCustomerManagerAM.Columns[1].IsReadOnly = true;
            DataTableCustomerManagerAM.Columns[2].IsReadOnly = true;
            DataTableCustomerManagerAM.Columns[3].IsReadOnly = true;
            DataTableCustomerManagerAM.Columns[4].IsReadOnly = true;
            DataTableCustomerManagerAM.Columns[5].IsReadOnly = true;
            DataTableCustomerManagerAM.Columns[6].IsReadOnly = true;
            DataTableCustomerManagerAM.Columns[7].IsReadOnly = true;
            DataTableCustomerManagerAM.Columns[8].IsReadOnly = true;
            DataTableCustomerManagerAM.Columns[9].IsReadOnly = true;

        }
     
        private void Click_Double(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                var curentCell = DataTableCustomerManagerAM.CurrentCell;
                var row = (DataRowView)curentCell.Item;

                if (row != null)
                {
                    var IDCustomer = row.Row["ID"].ToString();
                    //var row2 = row.Item as DataRow;
                    var idCus = int.Parse(IDCustomer);
                    frmCustomerAM userForm = new frmCustomerAM(idCus);
                    this.Close();
                    userForm.ShowDialog();                   
                }              
            }
        }
        public IEnumerable<DataGridRow> GetDataGridRows(DataGrid grid)
        {
            var itemsSource = grid.ItemsSource as IEnumerable;
            if (null == itemsSource) yield return null;
            foreach (var item in itemsSource)
            {
                var row = grid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (null != row) yield return row;
            }
        }
    }
}
