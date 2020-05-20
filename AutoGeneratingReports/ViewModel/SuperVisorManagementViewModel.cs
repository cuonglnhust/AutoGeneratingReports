using AutoGeneratingReports.Common;
using AutoGenReport.Model;
using Microsoft.Office.Interop.Excel;
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
    public class SuperVisorManagementViewModel : ObservableObject
    {
        AutoGenReportDbContext m_safenetLocalContext = new AutoGenReportDbContext();
        public ICommand btnThemMoi { get; set; }
        public ICommand btnDeleteSuperVisor { get; set; }
        public ICommand btnSaveSuperVisor { get; set; }
        public ICommand btnExportTxtSuperVisor { get; set; }
        public System.Action CloseAction { get; set; }
        public System.Data.DataTable _DataTableSuperVisor;
        public System.Data.DataTable DataTableSuperVisor
        {
            get { return _DataTableSuperVisor; }
            set
            {
                _DataTableSuperVisor = value;
                OnPropertyChanged("DataTableSuperVisor");
            }
        }
        
        public SuperVisorManagementViewModel()
        {
            btnThemMoi = new RelayCommand<object>((p) => { return true; }, (p) => { ExcuteClose(p); });
            btnDeleteSuperVisor = new RelayCommand<object>((p) => { return true; }, (p) => { DeleteSuperVisor(p); });
            btnSaveSuperVisor = new RelayCommand<object>((p) => { return true; }, (p) => { SaveAllSup(p); });
            btnExportTxtSuperVisor = new RelayCommand<object>((p) => { return true; }, (p) => { ExportTxtFile(p); });
            InitSuperVisorTable();
        }
        private void InitSuperVisorTable()
        {
            try
            {
                DataTableSuperVisor = new System.Data.DataTable();
                DataTableSuperVisor.Columns.Add("Chọn", typeof(bool));
                DataTableSuperVisor.Columns.Add("ID", typeof(int));
                DataTableSuperVisor.Columns.Add("Số thứ tự");
                DataTableSuperVisor.Columns.Add("Mã");
                DataTableSuperVisor.Columns.Add("Tên người giám sát");
                DataTableSuperVisor.Columns.Add("Ngày đăng kí");
                DataTableSuperVisor.Columns.Add("Ghi chú");
                DataTableSuperVisor.Columns.Add("Type");
                reloadGridView();
            }
            catch (Exception ex)
            {
                HelperClass.writeExceptionToDebugger(ex);
            }

        }
        private void DeleteSuperVisor(object obj)
        {
            var query =
                      (from p in DataTableSuperVisor.AsEnumerable()
                       where p.Field<bool>("Chọn").ToString() == "True"
                       select p.Field<int>("ID")).ToList();
            var lstSupAV = m_safenetLocalContext.AeonVNSups.Where(x => query.Contains(x.AVSupID)).ToList();
            m_safenetLocalContext.AeonVNSups.RemoveRange(lstSupAV);
            var dialogRst = MessageBox.Show("Bạn chắc chắn muốn xóa?", "Xác nhận", MessageBoxButton.OKCancel);
            if (dialogRst.ToString() == "OK")
            {
                m_safenetLocalContext.SaveChanges();
                reloadGridView();
            }
        }
        private void ExportTxtFile(object obj)
        {
            var query =
                           (from p in DataTableSuperVisor.AsEnumerable()
                            where p.Field<bool>("Chọn").ToString() == "True"
                            select p.Field<int>("ID")).ToList();
            var lstSupVisor = m_safenetLocalContext.AeonVNSups.Where(x => query.Contains(x.AVSupID)).ToList();

            if (lstSupVisor.Count() > 0)
            {
                string strOutputFolder = Properties.Settings.Default.OutputFolderAV;
                string strFileName = Path.Combine(strOutputFolder, "Supervisor_AV.txt");
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(strFileName))
                {
                    foreach (var sup in lstSupVisor)
                    {
                        //DataRow row = rows[i] as DataRow;
                        // Change the field value.
                        string strTenantCode = sup.SupCode;
                        string strTenantShortName = sup.SupName;
                        string strTennantCodeFormat = string.Format("{0,-20}{1,-20}{2,1}{3,-7}{4,1}", strTenantCode, strTenantShortName, ";", strTenantCode, "?");
                        file.WriteLine(strTennantCodeFormat);

                        //Delete to lst 

                        var queryUpdate = from p in m_safenetLocalContext.AeonVNSups
                                          where p.AVSupID == sup.AVSupID
                                          select p;
                        if (queryUpdate.Count() > 0)
                        {
                            var customer = queryUpdate.SingleOrDefault();
                            customer.Type = "OUT TXT";
                        }
                    }
                }

                m_safenetLocalContext.SaveChanges();
                reloadGridView();
                MessageBox.Show("Đã tạo thành công " + strFileName, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void reloadGridView()
        {
            try
            {
                var result = from p in m_safenetLocalContext.AeonVNSups select p;
                int idx = 1;
                //gridControl1.DataSource = null;
                DataTableSuperVisor.Clear();
                foreach (var customer in result)
                {
                    DataRow row = DataTableSuperVisor.NewRow();
                    row["Chọn"] = false;
                    row["ID"] = customer.AVSupID;
                    row["Mã"] = customer.SupCode;
                    row["Tên người giám sát"] = customer.SupName;
                    row["Ngày đăng kí"] = customer.RegisterDay;
                    row["Ghi chú"] = customer.Note;
                    row["Type"] = customer.Type;
                    if (customer.Type == "NEW")
                        DataTableSuperVisor.Rows.InsertAt(row, 0);
                    else
                        DataTableSuperVisor.Rows.Add(row);

                }

                var query = from p in DataTableSuperVisor.AsEnumerable() select p;
                foreach (var row in query)
                {
                    row["Số thứ tự"] = "" + idx;
                    idx++;
                }
                //gridControl1.DataSource = m_dataTableCustomer;
            }
            catch (Exception ex)
            {
                HelperClass.writeExceptionToDebugger(ex);
            }

        }
        private void SaveAllSup(object obj)
        {
            try
            {
                var result = from p in m_safenetLocalContext.AeonVNSups select p;
                int idx = 1;
                //SplashScreenManager.ShowForm(this, typeof(SplashScreen2), true, true, false);
                //SplashScreenManager.Default.SendCommand(SplashScreen2.SplashScreenCommand.SetCaption, "Saving AEON MALL data ...");
                foreach (var customer in result)
                {
                    //Find row 
                    var query =
                            (from p in DataTableSuperVisor.AsEnumerable()
                             where p.Field<string>("ID") == "" + customer.AVSupID
                             select p);
                    if (query.Count() > 0)
                    {
                        DataRow row = query.SingleOrDefault();
                        customer.SupCode = "" + row["Mã"];
                        customer.SupName = ("" + row["Tên người giám sát"]).ToUpper();
                        customer.RegisterDay = "" + row["Ngày đăng kí"];
                        customer.Note = "" + row["Ghi chú"];
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
        private void ExcuteClose(object obj)
        {
            SuperVisorAV visorAV = new SuperVisorAV();
            CloseAction();
            visorAV.ShowDialog();
            
        }
    }
}
