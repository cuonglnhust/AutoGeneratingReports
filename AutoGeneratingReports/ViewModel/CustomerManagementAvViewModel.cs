using AutoGeneratingReports.Common;
using AutoGenReport.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AutoGeneratingReports.ViewModel
{
    public class CustomerManagementAvViewModel : ObservableObject
    {
        AutoGenReportDbContext m_safenetLocalContext = new AutoGenReportDbContext();
        private DataTable _DataTableCustomerManagerAV;
        public ICommand btnDeleteAV { get; set; }
        public ICommand btnAddCusAV { get; set; }
        public ICommand btnSaveAll { get; set; }
        public Action CloseAction { get; set; }
        public DataTable DataTableCustomerManagerAV
        {
            get { return _DataTableCustomerManagerAV; }
            set { _DataTableCustomerManagerAV = value; OnPropertyChanged("DataTableCustomerManagerAV");}
        }
        public CustomerManagementAvViewModel()
        {
            btnDeleteAV = new RelayCommand<object>((p) => { return true; }, (p) => { AcionDelete(p); });
            btnAddCusAV = new RelayCommand<object>((p) => { return true; }, (p) => { ShowAddFormCusAv(p); });
            btnSaveAll = new RelayCommand<object>((p) => { return true; }, (p) => { SaveAll(p); });
            InitCustomerManagementAV();
        }
        private void InitCustomerManagementAV()
        {
            try
            {
                DataTableCustomerManagerAV = new System.Data.DataTable();
               
                DataTableCustomerManagerAV.Columns.Add("ID", typeof(int));
                DataTableCustomerManagerAV.Columns.Add("Số thứ tự");
                DataTableCustomerManagerAV.Columns.Add("Tầng");
                DataTableCustomerManagerAV.Columns.Add("Vị trí");
                DataTableCustomerManagerAV.Columns.Add("Số quầy");
                DataTableCustomerManagerAV.Columns.Add("Tên quầy");
                DataTableCustomerManagerAV.Columns.Add("SalesBagPOS");
                DataTableCustomerManagerAV.Columns.Add("Mã vạch túi đỏ");
                DataTableCustomerManagerAV.Columns.Add("IntermediateBagPOS");
                DataTableCustomerManagerAV.Columns.Add("Mã vạch túi xanh");                
                reloadGridView();
            }
            catch (Exception ex)
            {
                HelperClass.writeExceptionToDebugger(ex);
            }

        }
        public void reloadGridView()
        {
            AutoGenReportDbContext m_safenetLocalContext = new AutoGenReportDbContext();
            //m_safenetLocalContext = new AutoGenReportDbContext();
            var result = from p in m_safenetLocalContext.AeonVNCustomers select p;
            int idx = 1;
            //gridControl1.DataSource = null;
            DataTableCustomerManagerAV.Clear();
            string strPrevName = "";
            foreach (var customerAV in result)
            {
                DataRow row = DataTableCustomerManagerAV.NewRow();
                row["ID"] = customerAV.AVCustomerID;
                if (customerAV.PosName != strPrevName)
                {
                    row["Số thứ tự"] = "" + idx;
                    strPrevName = customerAV.PosName;
                    idx++;
                }
                else
                 row["Số thứ tự"] = "";
                row["Tầng"] = customerAV.Floor;
                row["Vị trí"] = customerAV.PosLocation;
                row["Số quầy"] = customerAV.PosNumber;
                row["Tên quầy"] = customerAV.PosName;
                row["SalesBagPOS"] = customerAV.SalesBagPOS;
                row["Mã vạch túi đỏ"] = customerAV.SaleBagBarcode;
                row["IntermediateBagPOS"] = customerAV.IntermediateBagPOS;
                row["Mã vạch túi xanh"] = customerAV.IntermediateBagBarcode;
                
                DataTableCustomerManagerAV.Rows.Add(row);
            }
            //gridControl1.DataSource = m_dataTableCustomer;

        }
        public void AcionDelete(object obj)
        {
            reloadGridView();

        }
        public void ShowAddFormCusAv(object obj)
        {
            frmCustomerAv frm = new frmCustomerAv();
            CloseAction();
            frm.ShowDialog();
        }
        public void SaveAll(object obj)
        {
            
            try
            {
                var result = from p in m_safenetLocalContext.AeonVNCustomers select p;
                int idx = 1;
                //SplashScreenManager.ShowForm(this, typeof(SplashScreen2), true, true, false);
                //SplashScreenManager.Default.SendCommand(SplashScreen2.SplashScreenCommand.SetCaption, "Saving AEON MALL data ...");
                foreach (var customer in result)
                {
                    //Find row 
                    var query =
                            (from p in DataTableCustomerManagerAV.AsEnumerable()
                             where p.Field<string>("ID") == "" + customer.AVCustomerID
                             select p);
                    if (query.Count() > 0)
                    {
                        DataRow row = query.SingleOrDefault();
                       
                        customer.Floor = "" + row["Tầng"];
                        customer.PosLocation = "" + row["Vị trí"];
                        customer.PosNumber = "" + row["Số quầy"];
                        customer.PosName = ("" + row["Tên quầy"]).ToUpper();
                        customer.SalesBagPOS = "" + row["SalesBagPOS"];
                        customer.SaleBagBarcode = "" + row["Mã vạch túi đỏ"];
                        customer.IntermediateBagPOS = "" + row["IntermediateBagPOS"];
                        customer.IntermediateBagBarcode = "" + row["Mã vạch túi xanh"];

                    }

                }
                m_safenetLocalContext.SaveChanges();
                //SplashScreenManager.CloseForm();
            }
            catch (Exception ex)
            {
                HelperClass.writeExceptionToDebugger(ex);
            }
        }
    }
}
