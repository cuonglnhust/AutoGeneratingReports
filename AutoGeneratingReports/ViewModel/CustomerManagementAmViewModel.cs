using AutoGeneratingReports.Common;
using AutoGenReport.Model;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AutoGeneratingReports.ViewModel
{
    public class CustomerManagementAmViewModel : ObservableObject
    {
        public Action CloseAction { get; set; }
        private readonly AutoGenReportDbContext _context = new AutoGenReportDbContext();
        private System.Data.DataTable _DataTableCustomerManagerAM;
        public System.Data.DataTable DataTableCustomerManagerAM
        {
            get { return _DataTableCustomerManagerAM; }
            set { _DataTableCustomerManagerAM = value; OnPropertyChanged("DataTableCustomerManagerAM"); }
        }

        public ICommand btnSaveAll { get; set; }
        public ICommand btnDeleteCustomer { get; set; }
        public ICommand btnAddCustomerAM { get; set; }
        public ICommand btnExportCustomerTxt { get; set; }


        public CustomerManagementAmViewModel()
        {
            btnSaveAll = new RelayCommand<object>((p) => { return true; }, (p) => { btnSaveAll_Click(p); });
            btnDeleteCustomer = new RelayCommand<object>((p) => { return true; }, (p) => { AcionDelete(p); });
            btnAddCustomerAM = new RelayCommand<object>((p) => { return true; }, (p) => { ShowAddCustomer(p); });
            btnExportCustomerTxt = new RelayCommand<object>((p) => { return true; }, (p) => { ExportCustomerTxt(p); });
            InitCustomerManagementAM();


        }
        private void InitCustomerManagementAM()
        {
            try
            {
                DataTableCustomerManagerAM = new System.Data.DataTable();
                DataTableCustomerManagerAM.Columns.Add("Chọn", typeof(bool));
                DataTableCustomerManagerAM.Columns.Add("ID", typeof(int));
                DataTableCustomerManagerAM.Columns.Add("Số thứ tự");
                DataTableCustomerManagerAM.Columns.Add("Cards");
                DataTableCustomerManagerAM.Columns.Add("Mã cửa hàng");
                DataTableCustomerManagerAM.Columns.Add("TenantName");
                DataTableCustomerManagerAM.Columns.Add("Tên cửa hàng");
                DataTableCustomerManagerAM.Columns.Add("Ngày mở cửa");
                DataTableCustomerManagerAM.Columns.Add("Ghi chú");
                DataTableCustomerManagerAM.Columns.Add("Type");
                reloadGridView();
            }
            catch (Exception ex)
            {
                HelperClass.writeExceptionToDebugger(ex);
            }

        }
        public void ExportCustomerTxt(object obj)
        {

            var query =
                           (from p in DataTableCustomerManagerAM.AsEnumerable()
                            where p.Field<bool>("Chọn").ToString() == "True"
                            select p.Field<int>("ID")).ToList();
            var lsiCustomerAm = _context.AeonMallCustomers.Where(x => query.Contains(x.AMCustomerID)).ToList();

            if (lsiCustomerAm.Count() > 0)
            {
                string strOutputFolder = Properties.Settings.Default.OutputFolderAV;
                string strFileName = Path.Combine(strOutputFolder, "CustomerAM.txt");
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(strFileName))
                {
                    foreach (var cus in lsiCustomerAm)
                    {
                        //DataRow row = rows[i] as DataRow;
                        // Change the field value.
                        string strTenantCode = cus.TenantCode;
                        string strTenantShortName = cus.TenantShortName;
                        string strTennantCodeFormat = string.Format("{0,-20}{1,-20}{2,1}{3,-7}{4,1}", strTenantCode, strTenantShortName, ";", strTenantCode, "?");
                        file.WriteLine(strTennantCodeFormat);

                        //Delete to lst 

                        var queryUpdate = from p in _context.AeonMallCustomers
                                          where p.AMCustomerID == cus.AMCustomerID
                                          select p;
                        if (queryUpdate.Count() > 0)
                        {
                            var customer = queryUpdate.SingleOrDefault();
                            customer.Type = "OUT TXT";
                        }
                    }
                }

                _context.SaveChanges();
                reloadGridView();
                MessageBox.Show("Đã tạo thành công " + strFileName, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }


        }
        public void reloadGridView()
        {
            AutoGenReportDbContext m_safenetLocalContext = new AutoGenReportDbContext();
            //m_safenetLocalContext = new AutoGenReportDbContext();
            var result = from p in m_safenetLocalContext.AeonMallCustomers select p;
            int idx = 1;
            //gridControl1.DataSource = null;
            DataTableCustomerManagerAM.Clear();
            foreach (var customer in result)
            {
                DataRow row = DataTableCustomerManagerAM.NewRow();
                row["Chọn"] = false;
                row["ID"] = customer.AMCustomerID;

                row["Cards"] = customer.Cards;
                row["Mã cửa hàng"] = customer.TenantCode;
                row["TenantName"] = customer.TenantName;
                row["Tên cửa hàng"] = customer.TenantShortName;
                row["Ngày mở cửa"] = customer.OpenningDate;
                row["Ghi chú"] = customer.Note;
                row["Type"] = customer.Type;

                if (customer.Type == "NEW")
                    DataTableCustomerManagerAM.Rows.InsertAt(row, 0);
                else
                    DataTableCustomerManagerAM.Rows.Add(row);


            }
            var query = from p in DataTableCustomerManagerAM.AsEnumerable() select p;
            foreach (var row in query)
            {
                row["Số thứ tự"] = "" + idx;
                idx++;
            }
            //gridControl1.DataSource = m_dataTableCustomer;

        }

        private void btnSaveAll_Click(object obj)
        {
            var result = from p in _context.AeonMallCustomers select p;
            int idx = 1;
            //SplashScreenManager.ShowForm(this, typeof(SplashScreen2), true, true, false);
            //SplashScreenManager.Default.SendCommand(SplashScreen2.SplashScreenCommand.SetCaption, "Saving AEON MALL data ...");
            foreach (var customer in result)
            {
                //Find row 
                var query =
                        (from p in DataTableCustomerManagerAM.AsEnumerable()
                         where p.Field<int>("ID") == customer.AMCustomerID
                         select p);
                if (query.Count() > 0)
                {
                    DataRow row = query.SingleOrDefault();
                    customer.Cards = "" + row["Cards"];
                    customer.TenantCode = "" + row["Mã cửa hàng"];
                    customer.TenantName = ("" + row["TenantName"]).ToUpper();
                    customer.TenantShortName = ("" + row["Tên cử hàng"]).ToUpper();
                    customer.OpenningDate = "" + row["Ngày mở cửa"];
                    customer.Note = "" + row["Ghi chú"];
                }

            }
            _context.SaveChanges();
            //SplashScreenManager.CloseForm();
        }
        public void AcionDelete(object obj)
        {
            var query =
                       (from p in DataTableCustomerManagerAM.AsEnumerable()
                        where p.Field<bool>("Chọn").ToString() == "True"
                        select p.Field<int>("ID")).ToList();
            var lsiCustomerAm = _context.AeonMallCustomers.Where(x => query.Contains(x.AMCustomerID)).ToList();
            _context.AeonMallCustomers.RemoveRange(lsiCustomerAm);
            var dialogRst = MessageBox.Show("Bạn chắc chắn muốn xóa?", "Xác nhận", MessageBoxButton.OKCancel);
            if (dialogRst.ToString() == "OK")
            {
                _context.SaveChanges();
                reloadGridView();
            }
        }
        public void ShowAddCustomer(object obj)
        {
            frmCustomerAM frmCustomerAM = new frmCustomerAM();
            CloseAction();
            frmCustomerAM.ShowDialog();
            
        }
        private void CloseWindow()
        {
            CloseAction();
        }
    }
}
