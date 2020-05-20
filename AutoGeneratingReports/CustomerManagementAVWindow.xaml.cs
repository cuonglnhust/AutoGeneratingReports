using AutoGeneratingReports.Common;
using AutoGeneratingReports.ViewModel;
using AutoGenReport.Model;
using System;
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
    /// Interaction logic for DSQuayAvWindow.xaml
    /// </summary>
    public partial class DSQuayAvWindow : Window
    {
        public DSQuayAvWindow()
        {
            CustomerManagementAvViewModel avViewModel = new CustomerManagementAvViewModel();
            this.DataContext = avViewModel;
            if (avViewModel.CloseAction == null)
            {
                avViewModel.CloseAction = new Action(() => this.Close());
            }
            InitializeComponent();
        }

        private void DataTableCustomerManagerAV_Loading(object sender, DataGridRowEventArgs e)
        {
            DataTableCustomerManagerAV.Columns[0].Width = 0;
            DataTableCustomerManagerAV.Columns[6].Visibility = Visibility.Hidden;
            DataTableCustomerManagerAV.Columns[8].Visibility = Visibility.Hidden;

            DataTableCustomerManagerAV.Columns[0].IsReadOnly = true;
            DataTableCustomerManagerAV.Columns[1].IsReadOnly = true;
            DataTableCustomerManagerAV.Columns[2].IsReadOnly = true;
            DataTableCustomerManagerAV.Columns[3].IsReadOnly = true;
            DataTableCustomerManagerAV.Columns[4].IsReadOnly = true;
            DataTableCustomerManagerAV.Columns[5].IsReadOnly = true;
            DataTableCustomerManagerAV.Columns[6].IsReadOnly = true;
            DataTableCustomerManagerAV.Columns[7].IsReadOnly = true;
            DataTableCustomerManagerAV.Columns[8].IsReadOnly = true;
            DataTableCustomerManagerAV.Columns[9].IsReadOnly = true;
        }

        private void btnDeleteAv_Click(object sender, RoutedEventArgs e)
        {
            var dialogRst = MessageBox.Show("Bạn chắc chắn muốn xóa?", "Xác nhận", MessageBoxButton.OKCancel);
            var temp = 0;
            if (dialogRst.ToString() == "OK")
            {
                AutoGenReportDbContext m_safenetLocalContext = new AutoGenReportDbContext();
                object item = DataTableCustomerManagerAV.SelectedItem;

                //string ID = (GridDataAM.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
                //MessageBox.Show(ID);
                var row_list =GetDataRowGridCommon.GetDataGridRows(DataTableCustomerManagerAV);
                foreach (DataGridRow single_row in row_list)
                {
                    if (single_row.IsSelected == true)
                    {
                        temp += 1;
                        var AvCustomerRow = (DataTableCustomerManagerAV.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
                        var CustomerAvID = int.Parse(AvCustomerRow);
                        var CusAv = from p in m_safenetLocalContext.AeonVNCustomers where p.AVCustomerID == CustomerAvID select p;
                        if (CusAv.Count() > 0)
                        {
                            var CustomerAvSelect = CusAv.SingleOrDefault();
                            m_safenetLocalContext.AeonVNCustomers.Remove(CustomerAvSelect);
                            m_safenetLocalContext.SaveChanges();
                        }                                   
                    }

                }
                if (temp == 0)
                {
                    MessageBox.Show("Chọn người dùng cần xóa", "Xác nhận", MessageBoxButton.OKCancel);
                }
            }
        }

        private void Click_Double(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                var curentCell = DataTableCustomerManagerAV.CurrentCell;
                var row = (DataRowView)curentCell.Item;

                if (row != null)
                {
                    var m_strPosName = row.Row["Tên quầy"].ToString();
                    //var row2 = row.Item as DataRow;
                    //var idCus = int.Parse(IDCustomer);
                    frmCustomerAv userForm = new frmCustomerAv(m_strPosName);
                    this.Close();
                    userForm.ShowDialog();
                }
            }
        }
    }
}
