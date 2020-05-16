using AutoGeneratingReports.Common;
using AutoGeneratingReports.Custom;
using AutoGenReport.Model;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Input;
using Excel = Microsoft.Office.Interop.Excel;

namespace AutoGeneratingReports.ViewModel
{
    public class MainWindowViewModel: ObservableObject
    {
        private readonly AutoGenReportDbContext _context = new AutoGenReportDbContext();
        object obj2LockInsertFile = new object();
        bool m_bPermission2GenCheckList = false;
        CultureInfo m_cul = null;            // declare culture info
        ResourceManager m_res_man = null;

        public DataGrid DataGridAm;
        public System.Action CloseAction { get; set; }
        public ICommand btnDsNguoiDung { get; set; }
        public ICommand btnDsQuayAv { get; set; }
        public ICommand btnDsNguoiGiamSatAv { get; set; }
        public ICommand btnDanhKHAeonMall { get; set; }
        public ICommand btnThayMatKhau { get; set; }
        public ICommand btnDangXuat { get; set; }
        public ICommand btnXuatDuLieu { get; set; }
        public ICommand btnTimKiem { get; set; }
        public ICommand btnTimKiemAV { get; set; }
        public ICommand btnXoa { get; set; }
        public ICommand ExportAeonMall { get; set; }
        public ICommand SinhBangKiemQuyTongAV { get; set; }
        public ICommand SinhBangKiemQuyTongAM { get; set; }
        public ICommand SinhBangKiemQuyChoKiemDemAM { get; set; }
        public ICommand SinhBangKiemQuyChoKiemDemAV { get; set; }
        public ICommand SaoChepCotAM { get; set; }
        public ICommand SaoChepCotAV { get; set; }
        public ICommand ResetCheckAM { get; set; }
        public ICommand ResetCheckAV { get; set; }
        private DateTime _startDate = DateTime.Now;
        public int IndexTab = -1;
        public int SelectedIndex
        {
            get { return IndexTab; }
            set { IndexTab = value; OnPropertyChanged("SelectedIndex"); }
        }
        private string _ThoiGianTaoCheckList;
        public string ThoiGianTaoCheckList
        {
            get { return _ThoiGianTaoCheckList; }
            set { _ThoiGianTaoCheckList = value; OnPropertyChanged("ThoiGianTaoCheckList"); }
        }
        private string _TuGioPhut;
        public string TuGioPhut
        {
            get { return _TuGioPhut; }
            set { _TuGioPhut = value; OnPropertyChanged("TuGioPhut"); }
        }
        private string _DenGioPhut;
        public string DenGioPhut
        {
            get { return _DenGioPhut; }
            set { _DenGioPhut = value; OnPropertyChanged("DenGioPhut"); }
        }

        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                OnPropertyChanged("StartDate");
            }
        }
        private DateTime _endDate = DateTime.Now;
        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                _endDate = value;
                OnPropertyChanged("StartDate");
            }
        }
        private string _InputFolder = "C:\\Users\\cuong\\Desktop";
        public string barEditItemInputFolder
        {
            get { return _InputFolder; }
            set
            {
                _InputFolder = value;
                OnPropertyChanged("barEditItemInputFolder");
            }
        }
        private System.Data.DataTable _DataTableAM;
        public System.Data.DataTable DataTableAM
        {
            get { return _DataTableAM; }
            set
            {
                _DataTableAM = value;
                OnPropertyChanged("DataTableAM");
            }
        }
        private System.Data.DataTable _DataTableAV;
        public System.Data.DataTable DataTableAV
        {
            get { return _DataTableAV; }
            set
            {
                _DataTableAV = value;
                OnPropertyChanged("DataTableAV");
            }
        }      
        private string btn_DangNhap_visiblity;

        public string Btn_Update_Visibility
        {
            get { return btn_DangNhap_visiblity;}
            set
            {
                btn_DangNhap_visiblity = value;
                OnPropertyChanged("btn_Enable");
            }
        }
        private string _txtSearch;
        public string txtTimKiem
        {
            get { return _txtSearch; }
            set
            {
                _txtSearch = value;
                OnPropertyChanged("txtTimKiem");
            }
        }
        private string _NoiXuatAeonMall = "C:\\Users\\cuong\\Desktop";
        public string NoiXuatAeonMall
        {
            get { return _NoiXuatAeonMall; }
            set
            {
                _NoiXuatAeonMall = value;
                OnPropertyChanged("NoiXuatAeonMall");
                
            }
        }
        private string _NoiXuatAeonVN = "C:\\Users\\cuong\\Desktop";
        public string barEditItemOutputAVFolder
        {
            get { return _NoiXuatAeonVN; }
            set
            {
                _NoiXuatAeonVN = value;
                OnPropertyChanged("NoiXuatViettinbank");
            }
        }
        private string _Pass7zip = "amv";
        public string barEditItemPassword7z
        {
            get { return _Pass7zip; }
            set
            {
                _Pass7zip = value;
                OnPropertyChanged("MatKhauFile7z");
            }
        }
        public ICommand btn_Thoat { get; set; }
        
        public MainWindowViewModel(AutoGenReportDbContext context)
        {
              _context = context;
            Properties.Settings.Default.OutputFolderAV = barEditItemOutputAVFolder;
            Properties.Settings.Default.Save();
            btnDsNguoiDung = new RelayCommand<System.Windows.Window>((p) => { return true; }, (p) => { ShowDsNguoiDung(p); });
            btnDsQuayAv = new RelayCommand<System.Windows.Window>((p) => { return true; }, (p) => { ShowDsQuayAv(p); });
            btnDsNguoiGiamSatAv = new RelayCommand<System.Windows.Window>((p) => { return true; }, (p) => { ShowDsNguoiGiamSat(p); });
            btnDanhKHAeonMall = new RelayCommand<System.Windows.Window>((p) => { return true; }, (p) => { ShowDsAeonMall(p); });
            btnThayMatKhau = new RelayCommand<System.Windows.Window>((p) => { return true; }, (p) => { ShowThayMatKhau(p); });
            btnDangXuat = new RelayCommand<System.Windows.Window>((p) => { return true; }, (p) => { DangXuat(p); });
            btnXuatDuLieu = new RelayCommand<object>((p) => { return true; }, (p) => { XuatDuLieuClick(p); });
            btnTimKiem = new RelayCommand<object>((p) => { return true; }, (p) => { XuatDuLieuClick(p); });
            btnTimKiemAV = new RelayCommand<object>((p) => { return true; }, (p) => { XuatDuLieuClick(p); });
            btnXoa = new RelayCommand<object>((p) => { return true; }, (p) => { DeleteSearch(p); });
            ExportAeonMall = new RelayCommand<object>((p) => { return true; }, (p) => { ExportAM(p); });
            SinhBangKiemQuyTongAV = new RelayCommand<object>((p) => { return true; }, (p) => { SinhBangKiemQuyAV(p); });
            SinhBangKiemQuyTongAM = new RelayCommand<object>((p) => { return true; }, (p) => { SinhBangKiemQuyAM(p); });
            SinhBangKiemQuyChoKiemDemAM = new RelayCommand<object>((p) => { return true; }, (p) => { SinhBangKiemQuyKiemDemAM(p); });
            SinhBangKiemQuyChoKiemDemAV = new RelayCommand<object>((p) => { return true; }, (p) => { SinhBangKiemQuyAV(p); });
            SaoChepCotAM = new RelayCommand<object>((p) => { return true; }, (p) => { CopySelectionAM(p); });
            SaoChepCotAV = new RelayCommand<object>((p) => { return true; }, (p) => { CopySelectionAV(p); });
            ResetCheckAM = new RelayCommand<object>((p) => { return true; }, (p) => { ResetCheckBoxAM(p); });
            ResetCheckAV = new RelayCommand<object>((p) => { return true; }, (p) => { ResetCheckBoxAM(p); });
            InitGridAM();
            InitGridAV();
            applyConfiguration();


            //var x = Properties.Settings.Default.Username;
            //_context = context;
            //if(x == "admin")
            //{
            //    MessageBox.Show(x);
            //}

        }
        #region Hàm show form 
        public void ShowDsNguoiDung(System.Windows.Window wd)
        {
            DSNguoiDungWindow dSNguoiDung = new DSNguoiDungWindow();
            dSNguoiDung.ShowDialog();
        }
        public void ShowDsQuayAv(System.Windows.Window wd)
        {
            DSQuayAvWindow QuayAv = new DSQuayAvWindow();
            QuayAv.ShowDialog();
        }
        public void ShowDsNguoiGiamSat(System.Windows.Window wd)
        {
            DSNguoiDamSatAV dSNguoiGiamSat = new DSNguoiDamSatAV();
            dSNguoiGiamSat.ShowDialog();
        }
        public void ShowDsAeonMall(System.Windows.Window wd)
        {
            QuanLyKhachHangAeonMall dSKhAeonMall = new QuanLyKhachHangAeonMall();
            dSKhAeonMall.ShowDialog();
        }
        public void ShowThayMatKhau(System.Windows.Window wd)
        {
            ChangePasswordWindow changePassword = new ChangePasswordWindow(Properties.Settings.Default.Username);
            changePassword.ShowDialog();
            
            
        }
        public void DangXuat(System.Windows.Window window)
        {
            try
            {
                var result = MessageBox.Show("Bạn chắc chắn muốn thoát ?", "Xác nhận", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result.ToString() == "OK")
                {
                    LoginWindow login = new LoginWindow();
                    login.Show();
                    CloseAction();
                    Properties.Settings.Default.Username = null;
                }
            }
            catch (Exception ex)
            {
                HelperClass.writeExceptionToDebugger(ex);
            }
           
          
        }
        private void CloseWindow(System.Windows.Window window)
        {
            CloseAction();
        }
        #endregion Hàm show form 

        public void DeleteSearch(object obj)
        {
            txtTimKiem = "";
        }
        //public void ShowDateTime(Window win)
        //{
        //    var dialog = new Dialog() { Message = StartDate.ToString() };            
        //    dialog.ShowDialog();
        //}
        #region Hàm gọi
        private void CopySelectionAM(object obj)
        {
            copyActual2DeclaredColumn_SelectedRowsAM();
        }
        private void CopySelectionAV(object obj)
        {
            copyActual2DeclaredColumn_SelectedRowsAV();
        }
        private void SinhBangKiemQuyKiemDemAM(object obj)
        {
            SinhBangKiemDemAM(DataTableAM, "BKQ-AEONMALL-");
        }
        private void SinhBangKiemQuyKiemDemAV(object obj)
        {
            SinhBangKiemDemAM(DataTableAV, "BKQ-AEONVN-");
        }
        private void SinhBangKiemQuyAV(object obj){
            SinhKiemQuyAV();
        }
        private void SinhBangKiemQuyAM(object obj)
        {
            SinhKiemQuyAM();
        }
        private void ResetCheckBoxAM(object obj)
        {
            if(SelectedIndex == 0)
            {
                for (int nIdx = 0; nIdx < DataTableAM.Rows.Count; nIdx++)
                {
                    var row = DataTableAM.Rows[nIdx];
                    if (row.Field<bool>("Đã kiểm tra") == true)
                    {
                        updateMonitorRow(false, nIdx);
                        row["Đã kiểm tra"] = false;
                    }

                }
            }else if(SelectedIndex == 1)
            {
                for (int nIdx = 0; nIdx < DataTableAV.Rows.Count; nIdx++)
                {
                    var row = DataTableAV.Rows[nIdx];
                    if (row.Field<bool>("Đã kiểm tra") == true)
                    {
                        updateMonitorRow(false, nIdx);
                        row["Đã kiểm tra"] = false;
                    }

                }
            }
            
        }
        #endregion Hàm gọi

        #region Sao chép từ cột kiểm đếm đến cột khai báo AM
        private void copyActual2DeclaredColumn_SelectedRowsAM()
        {
            // Create an empty list.
            ArrayList rows = new ArrayList();
            // Add the selected rows to the list.
            for (int i = 0; i < DataTableAM.Rows.Count; i++)
            {
                if (DataTableAM.Rows[i].RowState >= 0)
                    rows.Add(DataTableAM.Rows[i]);
                    //rows.Add(DataTableAM.row(gridViewMain.GetSelectedRows()[i]));
            }
            try
            {
                //DataTableAM.Rows.BeginEdit();
                //SplashScreenManager.ShowForm(this, typeof(SplashScreen2), true, true, false);
                //SplashScreenManager.Default.SendCommand(SplashScreen2.SplashScreenCommand.SetCaption, "Generating DATA...");
                for (int i = 0; i < rows.Count; i++)
                {
                    DataRow row = rows[i] as DataRow;
                    row.BeginEdit();
                    // Change the field value.

                    if (row["Số tiền theo bảng kê"] != row["Thành tiền sau kiểm kê"])
                    {
                        row["Số tiền theo bảng kê"] = row["Thành tiền sau kiểm kê"];
                        row[m_res_man.GetString("LastEdit", m_cul)] = Properties.Settings.Default.Username;
                        string DateTimeNow = DateTime.Now.ToString("yyyyMMddHHmmss");
                        row[m_res_man.GetString("TimeTag", m_cul)] = DateTimeString2String(DateTimeNow);
                        var result = (from p in _context.DepositHistories where p.DepositHistoryID == row["ID Lịch sử gửi tiền"] select p).SingleOrDefault();
                        result.DeclaredAmount = "" + row["Số tiền theo bảng kê"];
                        result.TimeTag = DateTimeNow;
                        result.LastEdit = "" + row["Cập nhật gần nhất"];
                        
                        _context.SaveChanges();
                    }
                    row.EndEdit();

                }
            }
            catch (Exception ex)
            {
                HelperClass.writeExceptionToDebugger(ex);
            }
            finally
            {
                //SplashScreenManager.CloseForm(false);
                //gridViewMain.EndUpdate();
            }
        }
        #endregion Sao chép từ cột kiểm đếm đến cột khai báo AM

        #region Sao chép từ cột kiểm đếm đến cột khai báo AV
        private void copyActual2DeclaredColumn_SelectedRowsAV()
        {
            // Create an empty list.
            ArrayList rows = new ArrayList();
            // Add the selected rows to the list.
            for (int i = 0; i < DataTableAV.Rows.Count; i++)
            {
                if (DataTableAV.Rows[i].RowState >= 0)
                    rows.Add(DataTableAV.Rows[i]);
                //rows.Add(DataTableAM.row(gridViewMain.GetSelectedRows()[i]));
            }
            try
            {
                //DataTableAM.Rows.BeginEdit();
                //SplashScreenManager.ShowForm(this, typeof(SplashScreen2), true, true, false);
                //SplashScreenManager.Default.SendCommand(SplashScreen2.SplashScreenCommand.SetCaption, "Generating DATA...");
                for (int i = 0; i < rows.Count; i++)
                {
                    DataRow row = rows[i] as DataRow;
                    row.BeginEdit();
                    // Change the field value.

                    if (row["Số tiền theo bảng kê"] != row["Thành tiền sau kiểm kê"])
                    {
                        row["Số tiền theo bảng kê"] = row["Thành tiền sau kiểm kê"];
                        row[m_res_man.GetString("LastEdit", m_cul)] = Properties.Settings.Default.Username;
                        string DateTimeNow = DateTime.Now.ToString("yyyyMMddHHmmss");
                        row[m_res_man.GetString("TimeTag", m_cul)] = DateTimeString2String(DateTimeNow);
                        var result = (from p in _context.DepositHistories where p.DepositHistoryID == row["ID Lịch sử gửi tiền"] select p).SingleOrDefault();
                        result.DeclaredAmount = "" + row["Số tiền theo bảng kê"];
                        result.TimeTag = DateTimeNow;
                        result.LastEdit = "" + row["Cập nhật gần nhất"];

                        _context.SaveChanges();
                    }
                    row.EndEdit();

                }
            }
            catch (Exception ex)
            {
                HelperClass.writeExceptionToDebugger(ex);
            }
            finally
            {
                //SplashScreenManager.CloseForm(false);
                //gridViewMain.EndUpdate();
            }
        }
        #endregion Sao chép từ cột kiểm đếm đến cột khai báo AV

        private void SinhBangKiemDemAM(System.Data.DataTable dataTable, string strFileNameBeginning)
        {
            try
            {
                string startDate = "" + StartDate.ToString();
                string endDate = "" + EndDate.ToString();
                DateTime dtStartAeonMall = DateTime.Parse(startDate);
                DateTime dtEndAeonMall = DateTime.Parse(endDate);
                DateTime dt = dtStartAeonMall;

                //Create Data file
                //SplashScreenManager.Default.SendCommand(SplashScreen2.SplashScreenCommand.SetCaption, "Generating DATA...");
                DateTime dtGeneratedFile = DateTime.Now;

                while (dt <= dtEndAeonMall)
                {
                    string strDTFormat = dt.ToString("yyyyMMdd");
                    var queryCountingPeople =
                        from order in dataTable.AsEnumerable()
                        where DateTime.ParseExact(order.Field<string>(4), "yyyy-MM-dd", CultureInfo.InvariantCulture).Date == dt.Date
                        group order by order.Field<string>(12)
                            into g
                        select new
                        {
                            CountingPeople = g.First().Field<string>(12)
                        };
                    foreach (var countingPeople in queryCountingPeople)
                    {
                        IEnumerable<DataRow> query =
                       from order in dataTable.AsEnumerable()
                       where (DateTime.ParseExact(order.Field<string>(4), "yyyy-MM-dd", CultureInfo.InvariantCulture).Date == dt.Date)
                            && (order.Field<string>(12) == countingPeople.CountingPeople)
                       orderby order.Field<string>(5) ascending
                       select order;

                        // Create a table from the query.
                        if (query.Count() > 0)
                        {
                            System.Data.DataTable boundTable = query.CopyToDataTable<DataRow>();
                            int strPage = IndexTab;
                            switch (strPage)
                            {
                                case 0:
                                    generateDenominationSlipsAeonByDay(boundTable, barEditItemOutputAVFolder + "\\" + strFileNameBeginning + countingPeople.CountingPeople + "-" + dt.ToString("yyyyMMdd") + ".xlsx", dt.ToString("yyyyMMdd"), Path.GetFullPath("DLL\\BKQ_AM.xltx"), true);
                                    break;
                                case 1:
                                    generateDenominationSlipsAeonByDay(boundTable, barEditItemOutputAVFolder + "\\" + strFileNameBeginning + countingPeople.CountingPeople + "-" + dt.ToString("yyyyMMdd") + ".xlsx", dt.ToString("yyyyMMdd"), Path.GetFullPath("DLL\\BKQ_AV.xltx"), true);
                                    break;
                            }

                        }
                    }

                    dt = dt.AddDays(1);
                }
                //SplashScreenManager.CloseForm(false);
                //MessageBox.Show("Generating AEON MALL Reports successfully", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
               //SplashScreenManager.CloseForm(false);
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                HelperClass.writeExceptionToDebugger(ex);
            }
            finally
            {

            }
        }
        private void SinhKiemQuyAM()
        {
            try
            {
                string startDate = "" + StartDate.ToString();
                string endDate = "" + EndDate.ToString();
                DateTime dtStartAeonMall = DateTime.Parse(startDate);
                DateTime dtEndAeonMall = DateTime.Parse(endDate);
                DateTime dt = dtStartAeonMall;

                //Create Data file
                //SplashScreenManager.Default.SendCommand(SplashScreen2.SplashScreenCommand.SetCaption, "Generating DATA...");
                DateTime dtGeneratedFile = DateTime.Now;
                generateDATFile(dtGeneratedFile);
                generateENDFile(dtGeneratedFile);
                //SplashScreenManager.Default.SendCommand(SplashScreen2.SplashScreenCommand.SetCaption, "Generating AEONMALL REPORTS...");

                while (dt <= dtEndAeonMall)
                {
                    string strDTFormat = dt.ToString("yyyyMMdd");
                    IEnumerable<DataRow> query =
                        from order in DataTableAM.AsEnumerable()
                        where DateTime.ParseExact(order.Field<string>(4), "yyyy-MM-dd", CultureInfo.InvariantCulture).Date == dt.Date
                        orderby order.Field<string>(5) ascending
                        select order;

                    // Create a table from the query.
                    if (query.Count() > 0)
                    {
                        System.Data.DataTable boundTable = query.CopyToDataTable<DataRow>();
                        generateDenominationSlipsAeonByDay(boundTable, barEditItemOutputAVFolder + "\\" + "BKQ-AEONMALL-" + dt.ToString("yyyyMMdd") + ".xlsx", dt.ToString("yyyyMMdd"), Path.GetFullPath("DLL\\BKQ_AM.xltx"), true);
                    }


                    dt = dt.AddDays(1);
                }
                //SplashScreenManager.CloseForm(false);
                //MessageBox.Show("Generating AEON MALL Reports successfully", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                //SplashScreenManager.CloseForm(false);
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                HelperClass.writeExceptionToDebugger(ex);
            }
            finally
            {

            }
        }
        private void SinhKiemQuyAV()
        {
            try
            {
                string startDate = "" + StartDate.ToString();
                string endDate = "" + EndDate.ToString();
                DateTime dtStartAeonMall = DateTime.Parse(startDate);
                DateTime dtEndAeonMall = DateTime.Parse(endDate);
                DateTime dt = dtStartAeonMall;

                //SplashScreenManager.Default.SendCommand(SplashScreen2.SplashScreenCommand.SetCaption, "Generating AEON VN REPORTS...");
                while (dt <= dtEndAeonMall)
                {
                    string strDTFormat = dt.ToString("yyyyMMdd");

                    IEnumerable<DataRow> queryTotal =
                      from order in DataTableAV.AsEnumerable()
                      where (DateTime.ParseExact(order.Field<string>(4), "yyyy-MM-dd", CultureInfo.InvariantCulture).Date == dt.Date)
                      select order;
                    if (queryTotal.Count() > 0)
                    {
                        System.Data.DataTable boundTable = queryTotal.CopyToDataTable<DataRow>();
                        //Generate Daily Report
                        generateDenominationSlipsAeonByDay(boundTable, barEditItemOutputAVFolder + "\\" + "BKQ-AEONVN-" + dt.ToString("yyyyMMdd") + ".xlsx", dt.ToString("yyyyMMdd"), Path.GetFullPath("DLL\\BKQ_AV.xltx"), true);
                    }
                    dt = dt.AddDays(1);
                }
                //SplashScreenManager.CloseForm();

                // toolStripStatusLabel1.Text = "Generarating Report Completed!!!";
                //MessageBox.Show("Excel file created , you can find the file d:\\csharp-Excel.xls");
            }
            catch (Exception ex)
            {
                //SplashScreenManager.CloseForm();
                MessageBox.Show(ex.Message);
                HelperClass.writeExceptionToDebugger(ex);
            }
        }
        private void generateDenominationSlipsAeonByDay(System.Data.DataTable dtAeonVNByDay, string strOutputFileName, string strDateTime, string strTemplateFile, bool bVisible = false)
        {
            try
            {
                Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                xlApp.DisplayAlerts = false;
                xlApp.Visible = bVisible;
                if (bVisible)
                    xlApp.WindowState = XlWindowState.xlMaximized;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlWorkSheet;
                object misValue = System.Reflection.Missing.Value;

                xlWorkBook = xlApp.Workbooks.Add(misValue);
                //xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                xlWorkSheet = xlApp.Application.Sheets.Add(Type.Missing, Type.Missing, Type.Missing, strTemplateFile);
                xlWorkSheet.Cells[3, 2] = DateTime.ParseExact(strDateTime, "yyyyMMdd", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");
                double[] totalQuantity = new double[11];
                for (int i = 0; i < 11; i++)
                {
                    totalQuantity[i] = (double)dtAeonVNByDay.AsEnumerable().Sum(item => str2Money(item.Field<string>(2), i));
                    xlWorkSheet.Cells[6 + i, 2] = totalQuantity[i];
                }


                xlWorkBook.SaveAs(Path.GetFullPath(strOutputFileName), Excel.XlFileFormat.xlOpenXMLWorkbook, Missing.Value,
                                         Missing.Value, false, false, Excel.XlSaveAsAccessMode.xlNoChange,
                                         Excel.XlSaveConflictResolution.xlUserResolution, true,
                                         Missing.Value, Missing.Value, Missing.Value);

                if (!bVisible)
                {
                    xlWorkBook.Close(true, misValue, misValue);
                    xlApp.Quit();

                    releaseObject(xlWorkSheet);
                    releaseObject(xlWorkBook);
                    releaseObject(xlApp);
                }



            }
            catch (Exception ex)
            {
                HelperClass.writeExceptionToDebugger(ex);
            }

        }
        private void ExportAM(object obj)
        {
            var result =  MessageBox.Show("Bạn có chắc chắn muốn xuất báo cáo", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result.ToString() == "OK")
            {
               
                generateReportAeonMall();
            }
        }
        private void generateReportAeonMall()
        {
            //SplashScreenManager.ShowForm(this, typeof(SplashScreen2), true, true, false);
            try
            {
                string startDate = "" + StartDate.ToString();
                string endDate = "" + EndDate.ToString();
                
                DateTime dtStartAeonMall = DateTime.Parse(startDate);
                DateTime dtEndAeonMall = DateTime.Parse(endDate);
                DateTime dt = dtStartAeonMall;

                //Create Data file
                //SplashScreenManager.Default.SendCommand(SplashScreen2.SplashScreenCommand.SetCaption, "Generating DATA...");
                DateTime dtGeneratedFile = DateTime.Now;
                generateDATFile(dtGeneratedFile);
                generateENDFile(dtGeneratedFile);
                //SplashScreenManager.Default.SendCommand(SplashScreen2.SplashScreenCommand.SetCaption, "Generating AEONMALL REPORTS...");

                while (dt <= dtEndAeonMall)
                {
                    string strDTFormat = dt.ToString("yyyyMMdd");
                    IEnumerable<DataRow> query =
                        from order in DataTableAM.AsEnumerable()
                        where DateTime.ParseExact(order.Field<string>(4), "yyyy-MM-dd", CultureInfo.InvariantCulture).Date == dt.Date
                        orderby order.Field<string>(5) ascending
                        select order;

                    // Create a table from the query.
                    if (query.Count() > 0)
                    {
                        System.Data.DataTable boundTable = query.CopyToDataTable<DataRow>();
                        string strTemplateFile = Path.GetFullPath("DLL\\AM_Counterfeit.dll");
                        string strOutputFile = "" + barEditItemOutputAVFolder;
                        generateCounterfeitReport(boundTable, strTemplateFile, strOutputFile, "CustomerID");
                        strTemplateFile = Path.GetFullPath("DLL\\AEONMALL_BBXNSS.dll");

                        //generateReportAeonMallByDay(boundTable, barEditItemOutputAVFolder.EditValue + "\\" + "BBKD-" + dt.ToString("yyyyMMdd") + ".docx", dt.ToString("yyyyMMdd"));
                        //generateMistakeReport(boundTable, strTemplateFile, strOutputFile, "CustomerID");
                        //generateDenominationSlipsAeonByDay(boundTable, barEditItemOutputAVFolder.EditValue + "\\" + "BKQ-AEONMALL-" + dt.ToString("yyyyMMdd") + ".xlsx", dt.ToString("yyyyMMdd"), Path.GetFullPath("DLL\\BKQ_AM.xltx"));
                    }


                    dt = dt.AddDays(1);
                }
               // SplashScreenManager.CloseForm(false);
                MessageBox.Show("Generating AEON MALL Reports successfully", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            catch (Exception ex)
            {
                //SplashScreenManager.CloseForm(false);
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                HelperClass.writeExceptionToDebugger(ex);
            }
            finally
            {

            }
        }
        private void generateDATFile(DateTime dtGeneratedFile)
        {
            try
            {
                var results = (from dataTable in DataTableAM.AsEnumerable()
                               group dataTable by new { CustomerName = dataTable.Field<string>(6), SaleDateTime = dataTable.Field<string>(4) }
                                   into g
                               orderby g.First().Field<string>(5) ascending
                               orderby g.First().Field<string>(4) ascending
                               select
                               new
                               {
                                   SaleDateTime = DateTime.ParseExact(g.First().Field<string>(4), "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("yyyyMMdd"),
                                   strReportDT = g.First().Field<string>(3),
                                   strCustomerID = g.First().Field<string>(5),
                                   strCustomerName = g.First().Field<string>(6),
                                   dActualAmount = g.Sum(item => str2Money(item.Field<string>(9)))
                               });
                string strFileName = barEditItemOutputAVFolder + "\\" + "1003_DATA" + dtGeneratedFile.ToString("yyyyMMddHHmmss") + ".dat";
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(strFileName))
                {
                    foreach (var result in results)
                    {
                        DateTime dtReportDate = DateTime.Parse(result.strReportDT);
                        string strCustomerID_formated = result.strCustomerID.Length > 7 ? result.strCustomerID.Substring(0, 7) : result.strCustomerID;
                        string strCustomerName_formated = result.strCustomerName.Length > 8 ? result.strCustomerName.Substring(0, 8) : result.strCustomerName;
                        string strActualAmount = "" + result.dActualAmount;

                        file.WriteLine(result.SaleDateTime + "," + result.strCustomerID + "," + result.strCustomerName + "," + result.dActualAmount);
                    }
                }
                string strCompressedFile = NoiXuatAeonMall + "\\" + "1003_DATA" + dtGeneratedFile.ToString("yyyyMMddHHmmss") + ".7z";
                //Delete all files in the same day
                deleteAllFilesByDay(dtGeneratedFile);

                compress7z(Path.GetFullPath(strFileName), strCompressedFile, "" + barEditItemPassword7z);
                File.Delete(strFileName);



            }
            catch (Exception ex)
            {
                HelperClass.writeExceptionToDebugger(ex);
            }
        }
        private void generateCounterfeitReport(System.Data.DataTable dtAeonMallByDay, string strTemplateFile, string strOutputPath, string strGroupField)
        {
            try
            {
                //string strName;
                //if (ribbonControlMain.SelectedPage.Name == "ribbonPageAeonVN")
                //    strName = m_res_man.GetString("CashierName", m_cul);
                //else
                //    strName = m_res_man.GetString("CustomerName", m_cul);


                //dActualAmount = g.Sum(item => str2Money(item.Field<string>(m_res_man.GetString("ActualAmount", m_cul))))
                var result =
                    from order in dtAeonMallByDay.AsEnumerable()
                    group order by order.Field<string>(5) into g
                    orderby g.First().Field<string>(5) ascending
                    select
                    new
                    {
                        CustomerID = g.First().Field<string>(5),
                        BarcodeID = g.First().Field<string>(7),
                        CustomerName = g.First().Field<string>(6),
                        SaleDate = g.First().Field<string>(4),
                        NumBag = g.Count(),
                        DeclaredAmount = g.Sum(item => str2Money(item.Field<string>(8))),
                        ActualAmount = g.Sum(item => str2Money(item.Field<string>(9))),
                        CounterfeitAmount = g.Sum(item => str2Money(item.Field<string>(10))),
                        DiscrepancyAmount = g.Sum(item => str2Money(item.Field<string>(11))),
                        QuantityC500k = g.Sum(item => extractValue(item.Field<string>(2), 11)),
                        QuantityC200k = g.Sum(item => extractValue(item.Field<string>(2), 12)),
                        QuantityC100k = g.Sum(item => extractValue(item.Field<string>(2), 13)),
                        QuantityC50k = g.Sum(item => extractValue(item.Field<string>(2), 14)),
                        QuantityC20k = g.Sum(item => extractValue(item.Field<string>(2), 15)),
                        QuantityC10k = g.Sum(item => extractValue(item.Field<string>(2), 16)),
                        QuantityC5k = g.Sum(item => extractValue(item.Field<string>(2), 17)),
                        QuantityC2k = g.Sum(item => extractValue(item.Field<string>(2), 18)),
                        QuantityC1k = g.Sum(item => extractValue(item.Field<string>(2), 19)),
                        QuantityC500 = g.Sum(item => extractValue(item.Field<string>(2), 20)),
                        QuantityC200 = g.Sum(item => extractValue(item.Field<string>(2), 21))

                    };

                foreach (var customer in result)
                {

                    double totalCounterfeitAmount = customer.CounterfeitAmount;//str2Money(row.Field<string>(m_res_man.GetString("CounterfeitAmount", m_cul)));
                    if (totalCounterfeitAmount > 0)
                    {
                        string strActualAmount = "" + customer.ActualAmount;// row.Field<string>(m_res_man.GetString("ActualAmount", m_cul));

                        string strTenantName = customer.CustomerName;

                        //if(ribbonControlMain.SelectedPage.Name=="ribbonPageAeonVN")
                        //   strTenantName=row.Field<string>(m_res_man.GetString("CashierName", m_cul));
                        //else
                        //    strTenantName = row.Field<string>(m_res_man.GetString("CustomerName", m_cul));
                        string strTenantCode = customer.CustomerID;//row.Field<string>(m_res_man.GetString("CustomerID", m_cul));
                        string strSaleDate = DateTime.ParseExact(customer.SaleDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"); ;// ateTime.ParseExact(row.Field<string>(m_res_man.GetString("SaleDate", m_cul)), "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");
                        string strSaleDateddmmyyyy = DateTime.ParseExact(customer.SaleDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("ddMMyyyy"); ;//DateTime.ParseExact(row.Field<string>(m_res_man.GetString("SaleDate", m_cul)), "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("ddMMyyyy");

                        object oMissing = System.Reflection.Missing.Value;
                        object oEndOfDoc = "\\endofdoc";
                        Microsoft.Office.Interop.Word._Application objWord;
                        Microsoft.Office.Interop.Word._Document objDoc;

                        objWord = new Microsoft.Office.Interop.Word.Application();
                        objWord.Visible = true;

                        objDoc = objWord.Documents.Add(strTemplateFile);


                        var sel = objWord.Selection;


                        //[CURRENTDATE]
                        replaceWord(sel, "[CURRENTDATE]", DateTime.Now.ToString("dd/MM/yyyy"));

                        //[TENANTNAME]
                        replaceWord(sel, "[TENANTNAME]", strTenantName);
                        //TENANTCODE
                        //string strPage = ribbonControlMain.SelectedPage.Name;

                        //switch (strPage)
                        //{
                        //    case "ribbonPageAeonMall":
                        //        replaceWord(sel, "[TENANTCODE]", strTenantCode);
                        //        break;
                        //    case "ribbonPageAeonVN":
                        //        replaceWord(sel, "[BARCODE]", customer.BarcodeID);
                        //        break;
                        //}

                        //SALEDATE
                        replaceWord(sel, "[SALESDATE]", strSaleDate);

                        //  
                        //   replaceWord(sel, "[NUMBAG]", ""+customer.NumBag);

                        double totalDeclaredAmount = customer.DeclaredAmount;//str2Money(row.Field<string>(m_res_man.GetString("DeclaredAmount", m_cul)));
                        double totalActualAmount = customer.ActualAmount;//str2Money(row.Field<string>(m_res_man.GetString("ActualAmount", m_cul)));

                        double totalDiscrepancyAmount = customer.DiscrepancyAmount;//str2Money(row.Field<string>(m_res_man.GetString("DiscrepancyAmount", m_cul)));

                        //[TOTALAMOUNT]
                        replaceWord(sel, "[TOTALAMOUNT]", "" + (totalDeclaredAmount > 0.0 ? totalDeclaredAmount.ToString("#,#", CultureInfo.InvariantCulture) : "0"));

                        //[ACTUALAMOUNT]	VND
                        replaceWord(sel, "[ACTUALAMOUNT]", "" + totalActualAmount.ToString("#,#", CultureInfo.InvariantCulture));
                        //[ACTUALAMOUNT]	VND
                        replaceWord(sel, "[COUNTERFEITAMOUNT]", "" + (totalCounterfeitAmount != 0.0 ? totalCounterfeitAmount.ToString("#,#", CultureInfo.InvariantCulture) : "0"));
                        //  replaceWord(sel, "[DECLAREDAMOUNT]", ""+customer.DeclaredAmount);
                        int i = 0;
                        int j = 0;
                        Microsoft.Office.Interop.Word.Table objTable;
                        Microsoft.Office.Interop.Word.Range wrdRng = objDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;


                        objTable = objDoc.Tables[2];


                        int nOrder = 0;
                        string strQuantity = "" + customer.QuantityC500k + "," +
                                                    customer.QuantityC200k + "," +
                                                    customer.QuantityC100k + "," +
                                                    customer.QuantityC50k + "," +
                                                    customer.QuantityC20k + "," +
                                                    customer.QuantityC10k + "," +
                                                    customer.QuantityC5k + "," +
                                                    customer.QuantityC2k + "," +
                                                    customer.QuantityC1k + "," +
                                                    customer.QuantityC500 + "," +
                                                    customer.QuantityC200;
                        //row.Field<string>(m_res_man.GetString("Quantity", m_cul));
                        string[] strDenominationQuantity;
                        double[] fDenomination = new double[] { 500000, 200000, 100000, 50000, 20000, 10000, 5000, 2000, 1000, 500, 200 };
                        if (strQuantity != string.Empty)
                        {
                            string[] strs = strQuantity.Split(',');
                            strDenominationQuantity = new string[11];
                            for (int iC = 0; iC < 11; iC++)
                            {

                                strDenominationQuantity[iC] = strs[iC];
                                int nIndex = objTable.Rows.Count - 1;
                                if (str2Money(strDenominationQuantity[iC]) > 0)
                                {
                                    objTable.Rows.Add(objTable.Rows[nIndex]);

                                    objTable.Cell(nIndex, 1).Range.Text = "" + (nIndex - 2);
                                    objTable.Cell(nIndex, 2).Range.Text = "VND";
                                    objTable.Cell(nIndex, 3).Range.Text = "Polyme";

                                    objTable.Cell(nIndex, 4).Range.Text = fDenomination[iC].ToString("#,#", CultureInfo.InvariantCulture);
                                    objTable.Cell(nIndex, 5).Range.Text = strDenominationQuantity[iC];
                                    double fAmount = (fDenomination[iC]) * str2Money(strDenominationQuantity[iC]);
                                    objTable.Cell(nIndex, 8).Range.Text = "" + (fAmount != 0.0 ? fAmount.ToString("#,#", CultureInfo.InvariantCulture) : "0");
                                }

                            }
                            objTable.Rows[objTable.Rows.Count - 1].Delete();
                        }
                        else
                            strDenominationQuantity = new string[] { "", "", "", "", "", "", "", "", "", "", "" };


                        ////Last row
                        int nlastRow = objTable.Rows.Count;
                        objTable.Cell(nlastRow, 2).Range.Text = "" + (totalCounterfeitAmount != 0.0 ? totalCounterfeitAmount.ToString("#,#", CultureInfo.InvariantCulture) : "0");

                        string strOutputFile = Path.Combine(strOutputPath, "BBTG" + strTenantName + strSaleDateddmmyyyy + ".docx");

                        objDoc.SaveAs(strOutputFile);
                        //objDoc.Close();
                        //objWord.Quit();
                        releaseObject(objTable);
                        //releaseObject(objWord);
                        //releaseObject(objDoc);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                HelperClass.writeExceptionToDebugger(ex);
            }

        }
        public void XuatDuLieuClick(object obj)
        {
            AutoGenReportDbContext _contextGen = new AutoGenReportDbContext();
            try
            {
                var txtSearch = txtTimKiem;
                insertGridviewAeonTable(txtSearch, _contextGen);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK);
                HelperClass.writeExceptionToDebugger(ex);
            }
        }
        public void InitGridAM()
        {
            try
            {
                DataTableAM = new System.Data.DataTable();                              
                DataTableAM.Columns.Add("Thiết bị");
                DataTableAM.Columns.Add("ID Lịch sử gửi tiền");
                DataTableAM.Columns.Add("Số lượng");
                DataTableAM.Columns.Add("Ngày nộp túi");
                DataTableAM.Columns.Add("Ngày bán hàng");
                DataTableAM.Columns.Add("Mã cửa hàng");
                DataTableAM.Columns.Add("Tên cửa hàng");
                DataTableAM.Columns.Add("Mã vạch");
                DataTableAM.Columns.Add("Số tiền theo bảng kê");
                DataTableAM.Columns.Add("Thành tiền sau kiểm kê");
                DataTableAM.Columns.Add("Tiền giả");
                DataTableAM.Columns.Add("Tiền không hợp lệ");
                DataTableAM.Columns.Add("Người kiểm đếm");
                DataTableAM.Columns.Add("Cập nhật gần nhất");
                DataTableAM.Columns.Add("Thời gian cập nhật");
                DataTableAM.Columns.Add("Đã kiểm tra", typeof(bool));
                


            }
            catch(Exception ex)
            {
                HelperClass.writeExceptionToDebugger(ex);
            }
            
        }
        public void InitGridAV()
        {
            try
            {
                DataTableAV = new System.Data.DataTable();
                DataTableAV.Columns.Add("Thiết bị");
                DataTableAV.Columns.Add("ID Lịch sử gửi tiền");
                DataTableAV.Columns.Add("Số lượng");
                DataTableAV.Columns.Add("Ngày nộp túi");
                DataTableAV.Columns.Add("Ngày bán hàng");
                DataTableAV.Columns.Add("Tên quầy");
                DataTableAV.Columns.Add("Mã vạch");
                DataTableAV.Columns.Add("Số tiền theo bảng kê");
                DataTableAV.Columns.Add("Thành tiền sau kiểm kê");
                DataTableAV.Columns.Add("Tiền giả");
                DataTableAV.Columns.Add("Tiền không hợp lệ");
                DataTableAV.Columns.Add("Người kiểm đếm");
                DataTableAV.Columns.Add("Cập nhật gần nhất");
                DataTableAV.Columns.Add("Thời gian cập nhật");
                DataTableAV.Columns.Add("Đã kiểm tra", typeof(bool));
                
                //DataTableAV.Columns[0].ColumnMapping = MappingType.Hidden;
            }
            catch(Exception e)
            {
                HelperClass.writeExceptionToDebugger(e);
            }
        }
        private void insertGridviewAeonTable(string txtSearch, AutoGenReportDbContext _contextGen)
        {
            try
            {
                string startDate = "" + StartDate.ToString();
                string endDate = "" + EndDate.ToString();
                DateTime dtStartAeonMall = DateTime.Parse(startDate);
                DateTime dtEndAeonMall = DateTime.Parse(endDate);
                DateTime dt = dtStartAeonMall;

                DataTableAM.Rows.Clear();
                DataTableAV.Rows.Clear();
                while (dt <= dtEndAeonMall)
                {

                    string[] strFileNames = Directory.GetFiles(Path.GetFullPath("" + barEditItemInputFolder));

                    for (int i = 0; i < strFileNames.Length; i++)
                    {
                        string strFileName = Path.GetFileName(strFileNames[i]);
                        for (int nAdditionalDay = 0; nAdditionalDay <= 1; nAdditionalDay++)
                        {
                            DateTime dtGetFile = dt.AddDays(nAdditionalDay);
                            if (m_bPermission2GenCheckList)
                            {
                                if (strFileName.ToUpper().StartsWith("EVENTEXPORT-" + dtGetFile.ToString("yyyyMMdd")) && strFileName.ToUpper().EndsWith(".CSV"))
                                {
                                    lock (obj2LockInsertFile)
                                        if (!isProcessedFile(strFileName) || !isCompleted(strFileName))
                                        {
                                            string strInputFileName = strFileNames[i];
                                            insertCustomerReport2DB(strInputFileName, dtGetFile);
                                        }
                                    //List<string[]> lstResult = safeNetDB.getAM_HISTORYByDay(dt.ToString("yyyyMMdd"));

                                }
                            }
                        }
                    }                    
                    loadTableAM(dt, txtSearch, _contextGen);
                    //AEONVN
                    loadTableAV(dt, txtSearch, _contextGen);
                    dt = dt.AddDays(1);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                HelperClass.writeExceptionToDebugger(ex);
            }
        }

        public bool isCompleted(string strFileName)
        {

            bool bCompleted = false;
            try
            {
                var processedFileLst = from p in _context.ProcessedFiles where p.FileName == strFileName select p;
                
                if (processedFileLst.Count() > 0)
                {
                    string strStatus = processedFileLst.SingleOrDefault().Status;
                    if (strStatus != null && strStatus.ToUpper() == "COMPLETED")
                    {
                        bCompleted = true;
                    }
                }
            }
            catch (Exception ex)
            {
                HelperClass.writeExceptionToDebugger(ex);
            }
            return bCompleted;
        }
        public bool isProcessedFile(string strFileName)
        {

            bool bResult = false;
            try
            {
                var processedFileLst = from p in _context.ProcessedFiles where p.FileName == strFileName select p;
                if (processedFileLst.Count() > 0)
                    bResult = true;
                else
                    bResult = false;
            }
            catch (Exception ex)
            {
                HelperClass.writeExceptionToDebugger(ex);
            }
            return bResult;
        }
        private void insertCustomerReport2DB(string strInputFileName, DateTime dtSaleDate)
        {
            //Process file:
            // Write the processing file to DB
            // Process that file
            // Move to Processed Folder

            try
            {

                insertProcessedFile2DB(strInputFileName);
                int nNumOfDevices = int.Parse(Properties.Settings.Default.NoMachine);
                //load the lastest deposit time for each Device in DB
                DateTime[] dtLatestTime = new DateTime[nNumOfDevices];
                //for(int nDevice=0;nDevice<nNumOfDevices;nDevice++)
                //{
                //    var rst = m_safenetLocalContext.DepositHistories.OrderByDescending(s => s.DepositDate).FirstOrDefault(s => s.Device == ""+(nDevice+1));
                //    if(rst!=null)
                //    {
                //        dtLatestTime[nDevice] = DateTime.ParseExact(rst.DepositDate, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                //    }
                //}

                using (StreamReader sr = new StreamReader(strInputFileName))
                {
                    string line;
                    sr.ReadLine();//Skip header line
                    string[] str = strInputFileName.Split('-');
                    str = str[1].Split('.');
                    string strReportDate = str[0];
                    string strSaleDate = string.Empty;//DateTime.ParseExact(strReportDate, "yyyyMMddHHmmss", CultureInfo.InvariantCulture).AddDays(-1).ToString("yyyyMMdd");
                    string strStatus = null;
                    try
                    {
                        while ((line = sr.ReadLine()) != null)
                        {

                            str = line.Split(';');
                            //1;20160517210000;34037;test,   (SID=1);[ID=34037] Deposit passage: bagId=30,bval=00.00,bc=000001870177000,info=Seen,kto=12356,bic=0000001,bankAccount=,csid=334
                            if (str[2] == "34037")
                            {
                                string[] data = str[4].Split(',');
                                string strDevice = str[0];
                                string strDepositDate = str[1];
                                DateTime dtDepositDate = DateTime.ParseExact(strDepositDate, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                                int nCurrentDevice = int.Parse(strDevice) - 1;
                                string strCustomerID = data[5].Split('=')[1];
                                string strBarcodeIDTmp = data[2].Split('=')[1];
                                string strBarcodeID = strBarcodeIDTmp.Split('(')[0];

                                string strDeclaredAmount = data[1].Split('=')[1];
                                string strActualAmount = "";
                                string strCounterfeitAmount = "";
                                string strDiscrepancyAmount = "";
                                string strQuantity = "";
                                //if(dtDepositDate>dtLatestTime[nCurrentDevice])
                                {

                                    if (dtDepositDate >= dtSaleDate.AddHours(10))
                                        strSaleDate = dtSaleDate.ToString("yyyyMMdd");
                                    else
                                        strSaleDate = dtSaleDate.AddDays(-1).ToString("yyyyMMdd"); ;

                                    DepositHistory deposit = new DepositHistory();
                                    deposit.DepositHistoryID = strDepositDate + strDevice + strCustomerID + strBarcodeID;
                                    deposit.ActualAmount = strActualAmount;
                                    deposit.BarcodeID = strBarcodeID;
                                    deposit.CounterfeitAmount = strCounterfeitAmount;
                                    deposit.CustomerID = strCustomerID;

                                    deposit.DeclaredAmount = strDeclaredAmount;
                                    deposit.DepositDate = strDepositDate;
                                    deposit.SaleDate = strSaleDate;
                                    deposit.Quantity = strQuantity;
                                    deposit.Device = strDevice;
                                    deposit.DiscrepancyAmount = strDiscrepancyAmount;
                                    deposit.TimeTag = DateTime.Now.ToString("yyyyMMddHHmmss");
                                    //safeNetDB.insertAM_HISTORY(strDevice, strSaleDate, strDateTime, strCustomerID, strCustomerName, strBarcode, strDeclaredAmount, strActualAmount, strCounterfeitAmount, strDiscrepancyAmount, strQuantity);
                                    //Insert
                                    var rst = from p in _context.DepositHistories
                                              where p.DepositHistoryID == deposit.DepositHistoryID
                                              select p;
                                    if (rst == null || rst.Count() == 0)
                                    {
                                        _context.DepositHistories.Add(deposit);
                                        _context.SaveChanges();
                                    }

                                }

                            }
                        }
                        insertProcessedFile2DB(strInputFileName, "COMPLETED");
                    }
                    catch (Exception ex)
                    {
                        strStatus = ex.ToString();
                    }
                    finally
                    {
                        //insertProcessedFile2DB(strInputFileName, strStatus);
                    }

                }
            }
            catch (Exception ex)
            {
                HelperClass.writeExceptionToDebugger(ex);
            }
        }
        private void insertProcessedFile2DB(string strInputFileName, string strStatus = null)
        {
            try
            {

                ProcessedFile processedFile = new ProcessedFile();
                processedFile.FileName = Path.GetFileName(strInputFileName);
                processedFile.Status = strStatus;
                processedFile.TimeTag = DateTime.Now.ToString("yyyyMMddHHmmss");
                if (isProcessedFile(processedFile.FileName) == false)
                {
                    _context.ProcessedFiles.Add(processedFile);
                    _context.SaveChanges();
                }
                else
                {
                    processedFile = (from p in _context.ProcessedFiles where p.FileName == processedFile.FileName select p).SingleOrDefault();
                    processedFile.Status = strStatus;
                    _context.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                HelperClass.writeExceptionToDebugger(ex);
            }

        }
        private void loadTableAM(DateTime dt, string txtSearch, AutoGenReportDbContext _contextGen)
        {
            if (txtSearch == null)
            {
                txtSearch = "";

            }
            var DateTime2String = dt.ToString("yyyyMMdd");
            var lstDeposit = (from p in _contextGen.DepositHistories
                             join q in _contextGen.AeonMallCustomers on p.CustomerID equals q.TenantCode
                             where p.SaleDate.Contains(DateTime2String)
                             select new { p, q });
            var lstDepositSearch = lstDeposit.Where(x => x.p.Device.ToLower().Contains(txtSearch)
            || x.p.CustomerID.ToLower().Contains(txtSearch)
            || x.q.TenantShortName.ToLower().Contains(txtSearch)).ToList();
            //|| x.p.CountingPeople.ToLower().Contains(txtSearch)
            //|| x.p.BarcodeID.ToLower().Contains(txtSearch)).ToList();



            //AEONMALL
            foreach (var deposit in lstDepositSearch)
            {

                DataRow dr = DataTableAM.NewRow();
                               
                dr[0] = deposit.p.Device;//Device
                dr[1] = deposit.p.DepositHistoryID;//ID
                dr[2] = deposit.p.Quantity;//Quantity
                dr[3] = DateTime.ParseExact(deposit.p.DepositDate, "yyyyMMddHHmmss", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");//Date
                dr[4] = DateTime.ParseExact(deposit.p.SaleDate, "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");//SaleDate
                dr[5] = deposit.p.CustomerID;//CustomerID
                dr[6] = deposit.q.TenantShortName;//Customer Name;
                dr[7] = deposit.p.BarcodeID;//BarcodeID
                dr[8] = deposit.p.DeclaredAmount;//Declared amount
                dr[9] = deposit.p.ActualAmount;//Actual amount
                dr[10] = deposit.p.CounterfeitAmount;//Counterfeit amount
                dr[11] = deposit.p.DiscrepancyAmount;//Discrepancy amount
                dr[12] = deposit.p.CountingPeople;//Counting People             
                dr[13] = deposit.p.LastEdit;//Quantity
                dr[14] = DateTimeString2String(deposit.p.TimeTag);//ID
                dr[15] = deposit.p.Checked == "T" ? true : false;
                




                // if (m_aeonMallDB.isExist(deposit.p.CustomerID))
                DataTableAM.Rows.Add(dr);

            }
        }
        private void loadTableAV(DateTime dt, string txtSearch, AutoGenReportDbContext _contextGen)
        {
            if (txtSearch == null)
            {
                txtSearch = "";

            }
            var DateTime2String = dt.ToString("yyyyMMdd");
            var lstDepositVN = (from p in _contextGen.DepositHistories
                               from q in _contextGen.AeonVNCustomers
                               where ((q.IntermediateBagBarcode == p.BarcodeID) || (q.SaleBagBarcode == p.BarcodeID)) && p.SaleDate.Contains(DateTime2String)
                               select new { p, q });
            var lstDepositVNSearch = lstDepositVN.Where(x => x.p.Device.ToLower().Contains(txtSearch)
           || x.p.CustomerID.ToLower().Contains(txtSearch)
           || x.q.PosName.ToLower().Contains(txtSearch)).ToList();
            //|| x.p.CountingPeople.Contains(txtSearch)
            //|| x.p.BarcodeID.Contains(txtSearch)).ToList();

            foreach (var depositVN in lstDepositVNSearch)
            {

                if (depositVN.p.BarcodeID.Length == 16)
                {
                    DataRow dr = DataTableAV.NewRow();


                    dr[0] = depositVN.p.Device;//Device
                    dr[1] = depositVN.p.DepositHistoryID;//ID
                    dr[2] = depositVN.p.Quantity;//Quantity
                    dr[3] = DateTime.ParseExact(depositVN.p.DepositDate, "yyyyMMddHHmmss", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");//Date
                    dr[4] = DateTime.ParseExact(depositVN.p.SaleDate, "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");//SaleDate
                    dr[5] = depositVN.q.PosName;//CustomerID                   
                    dr[6] = depositVN.p.BarcodeID;//BarcodeID
                    dr[7] = depositVN.p.DeclaredAmount;//Declared amount
                    dr[8] = depositVN.p.ActualAmount;//Actual amount
                    dr[9] = depositVN.p.CounterfeitAmount;//Counterfeit amount
                    dr[10] = depositVN.p.DiscrepancyAmount;//Discrepancy amount
                    dr[11] = depositVN.p.CountingPeople;//Counting People              
                    dr[12] = depositVN.p.LastEdit;//Quantity
                    dr[13] = DateTimeString2String(depositVN.p.TimeTag);//ID
                    dr[14] = depositVN.p.Checked == "T" ? true : false;
                    

                    // if (m_aeonMallDB.isExist(deposit.p.CustomerID))
                    DataTableAV.Rows.Add(dr);                                      
                }

            }
        }
        string DateTimeString2String(string DatetimeYYYYMMDDHHMMSS)
        {
            return DateTime.ParseExact(DatetimeYYYYMMDDHHMMSS, "yyyyMMddHHmmss", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");//Date
        }
        private double str2Money(string strMoney, int i = -1)
        {
            if (i < 0)
                return double.Parse(strMoney == "" ? "0" : strMoney);
            else
            {
                if (strMoney == "")
                    return 0;
                string[] str = strMoney.Split(',');
                if (i < str.Length)
                    return double.Parse(str[i] == "" ? "0" : str[i]);
                else
                    return 0;
            }
        }

        private static void compress7z(string strFullFilePath, string strCompressedFilePath, string password)
        {

            try
            {
                ProcessStartInfo p = new ProcessStartInfo();

                p.FileName = "7zip\\7z.exe";
                p.Arguments = "a " + "\"" + strCompressedFilePath + "\" \"" + strFullFilePath + "\" -p" + password;
                p.WindowStyle = ProcessWindowStyle.Hidden;

                // 3.
                // Start process and wait for it to exit
                //
                Process x = Process.Start(p);
                x.WaitForExit();
            }
            catch (Exception ex)
            {
                HelperClass.writeExceptionToDebugger(ex);
            }

        }
        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Exception Occured while releasing object " + ex.ToString());
                HelperClass.writeExceptionToDebugger(ex);
            }
            finally
            {
                GC.Collect();
            }
        }
        private static void replaceWord(Selection sel, string strText, string strReplacement)
        {
            sel.Find.Text = strText;
            sel.Find.Replacement.Text = strReplacement;
            sel.Find.Wrap = WdFindWrap.wdFindContinue;
            sel.Find.Forward = true;

            sel.Find.Format = false;
            sel.Find.MatchCase = false;
            sel.Find.MatchWholeWord = false;
            sel.Find.Execute(Replace: WdReplace.wdReplaceAll);
        }
        double extractValue(string inputData, int idx)
        {
            string[] strs = inputData.Split(',');
            if (idx < strs.Length && strs[idx] != "")
                return double.Parse(strs[idx]);
            else
                return 0;
        }
        private void deleteAllFilesByDay(DateTime dtGeneratedFile)
        {
            string[] strFileNames = Directory.GetFiles(Path.GetFullPath("" + NoiXuatAeonMall));

            for (int i = 0; i < strFileNames.Length; i++)
            {
                string strFileName = Path.GetFileName(strFileNames[i]);
                if (strFileName.ToUpper().StartsWith("1003_DATA" + dtGeneratedFile.ToString("yyyyMMdd")))
                {
                    File.Delete(strFileNames[i]);
                }
            }
        }
        private void generateENDFile(DateTime dtGeneratedFile)
        {
            using (System.IO.StreamWriter file =
           new System.IO.StreamWriter(NoiXuatAeonMall + "\\" + "1003_DATA" + dtGeneratedFile.ToString("yyyyMMddHHmmss") + ".end")) ;
        }
        private void updateMonitorRow(bool bValue, int nRowHandle)
        {
            AutoGenReportDbContext dbContext = new AutoGenReportDbContext();
            if(SelectedIndex == 0)
            {
                var row = DataTableAM.Rows[nRowHandle];
                var ID = row["ID Lịch sử gửi tiền"].ToString();
                //var result = (from p in _context.DepositHistories where p.DepositHistoryID == row[1].ToString() select p).SingleOrDefault();
                var result = dbContext.DepositHistories.Where(x => x.DepositHistoryID.Contains(ID)).SingleOrDefault();
                result.Checked = bValue ? "T" : "F";
            }
            if(SelectedIndex == 1)
            {
                var row = DataTableAV.Rows[nRowHandle];
                var ID = row["ID Lịch sử gửi tiền"].ToString();
                //var result = (from p in _context.DepositHistories where p.DepositHistoryID == row[1].ToString() select p).SingleOrDefault();
                var result = dbContext.DepositHistories.Where(x => x.DepositHistoryID.Contains(ID)).SingleOrDefault();
                result.Checked = bValue ? "T" : "F";
            }
            

            dbContext.SaveChanges();
        }

        private void applyConfiguration()
        {
            AutoGenReportDbContext safenetLocalContext = new AutoGenReportDbContext();
            try
            {
                var user = (from p in safenetLocalContext.Users
                            where p.Username == Properties.Settings.Default.Username
                select p).SingleOrDefault();
                ThoiGianTaoCheckList = user.Time2GenCheckList;
                //barEditItemTime2GenCheckList.EditValue = user.Time2GenCheckList;
                TuGioPhut = user.TimeStart2Edit;
                //barEditItemStartTime2Edit.EditValue = user.TimeStart2Edit;
                DenGioPhut = user.TimeEnd2Edit;
                //barEditItemEndTime2Edit.EditValue = user.TimeEnd2Edit;
            }
            catch (Exception ex)
            {
                HelperClass.writeExceptionToDebugger(ex);
            }

        }
        //private void SetText(string barEditItem, string text)
        //{
        //    barEditItem = text;
            
        //}
    }
}
