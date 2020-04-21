using AutoGeneratingReports.Common;
using AutoGeneratingReports.Custom;
using AutoGenReport.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Input;

namespace AutoGeneratingReports.ViewModel
{
    public class MainWindowViewModel: ObservableObject
    {
        private readonly AutoGenReportDbContext _context = new AutoGenReportDbContext();
        object obj2LockInsertFile = new object();
        bool m_bPermission2GenCheckList = false;
        CultureInfo m_cul = null;            // declare culture info
        ResourceManager m_res_man = null;

        public Action CloseAction { get; set; }        
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
        private DateTime _startDate = DateTime.Now;
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
                OnPropertyChanged("NoiChuaDuLieu");
            }
        }
        private DataTable _DataTableAM;
        public DataTable DataTableAM
        {
            get { return _DataTableAM; }
            set
            {
                _DataTableAM = value;
                OnPropertyChanged("DataTableAM");
            }
        }
        private DataTable _DataTableAV;
        public DataTable DataTableAV
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
        public ICommand btn_Thoat { get; set; } 
       
        public MainWindowViewModel(AutoGenReportDbContext context)
        {
            context = _context;
            btnDsNguoiDung = new RelayCommand<Window>((p) => { return true; }, (p) => { ShowDsNguoiDung(p); });
            btnDsQuayAv = new RelayCommand<Window>((p) => { return true; }, (p) => { ShowDsQuayAv(p); });
            btnDsNguoiGiamSatAv = new RelayCommand<Window>((p) => { return true; }, (p) => { ShowDsNguoiGiamSat(p); });
            btnDanhKHAeonMall = new RelayCommand<Window>((p) => { return true; }, (p) => { ShowDsAeonMall(p); });
            btnThayMatKhau = new RelayCommand<Window>((p) => { return true; }, (p) => { ShowThayMatKhau(p); });
            btnDangXuat = new RelayCommand<Window>((p) => { return true; }, (p) => { DangXuat(p); });
            btnXuatDuLieu = new RelayCommand<object>((p) => { return true; }, (p) => { XuatDuLieuClick(p); });
            btnTimKiem = new RelayCommand<object>((p) => { return true; }, (p) => { XuatDuLieuClick(p); });
            btnTimKiemAV = new RelayCommand<object>((p) => { return true; }, (p) => { XuatDuLieuClick(p); });
            btnXoa = new RelayCommand<object>((p) => { return true; }, (p) => { DeleteSearch(p); });
            InitGridAM();
            InitGridAV();
                      
            //var x = Properties.Settings.Default.Username;
            //_context = context;
            //if(x == "admin")
            //{
            //    MessageBox.Show(x);
            //}

        }
        public void ShowDsNguoiDung(Window wd)
        {
            DSNguoiDungWindow dSNguoiDung = new DSNguoiDungWindow();
            dSNguoiDung.ShowDialog();
        }
        public void ShowDsQuayAv(Window wd)
        {
            DSQuayAvWindow QuayAv = new DSQuayAvWindow();
            QuayAv.ShowDialog();
        }
        public void ShowDsNguoiGiamSat(Window wd)
        {
            DSNguoiDamSatAV dSNguoiGiamSat = new DSNguoiDamSatAV();
            dSNguoiGiamSat.ShowDialog();
        }
        public void ShowDsAeonMall(Window wd)
        {
            QuanLyKhachHangAeonMall dSKhAeonMall = new QuanLyKhachHangAeonMall();
            dSKhAeonMall.ShowDialog();
        }
        public void ShowThayMatKhau(Window wd)
        {
            ChangePasswordWindow changePassword = new ChangePasswordWindow();
            changePassword.ShowDialog();
        }
        public void DangXuat(Window window)
        {
            LoginWindow login = new LoginWindow();
            login.Show();
            CloseAction();
            Properties.Settings.Default.Username = null;
        }
        private void CloseWindow(Window window)
        {
            CloseAction();
        }
        public void DeleteSearch(object obj)
        {
            txtTimKiem = "";
        }
        //public void ShowDateTime(Window win)
        //{
        //    var dialog = new Dialog() { Message = StartDate.ToString() };            
        //    dialog.ShowDialog();
        //}
        public void XuatDuLieuClick(object obj)
        {
            AutoGenReportDbContext _context = new AutoGenReportDbContext();
            try
            {
                var txtSearch = txtTimKiem;
                insertGridviewAeonTable(txtSearch);
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
                DataTableAM = new DataTable();
                DataTableAM.Columns.Add("Thiết bị");
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
                DataTableAV = new DataTable();
                DataTableAV.Columns.Add("Thiết bị");
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
            }
            catch(Exception e)
            {
                HelperClass.writeExceptionToDebugger(e);
            }
        }
        private void insertGridviewAeonTable(string txtSearch)
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
                    loadTableAM(dt, txtSearch);
                    //AEONVN
                    loadTableAV(dt, txtSearch);
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
        private void loadTableAM(DateTime dt, string txtSearch)
        {
            if (txtSearch == null)
            {
                txtSearch = "";

            }
            var DateTime2String = dt.ToString("yyyyMMdd");
            var lstDeposit = (from p in _context.DepositHistories
                             join q in _context.AeonMallCustomers on p.CustomerID equals q.TenantCode
                             where p.SaleDate.Contains(DateTime2String)
                             select new { p, q }).ToList();
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
                dr[1] = DateTime.ParseExact(deposit.p.DepositDate, "yyyyMMddHHmmss", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");//Date
                dr[2] = DateTime.ParseExact(deposit.p.SaleDate, "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");//SaleDate
                dr[3] = deposit.p.CustomerID;//CustomerID
                dr[4] = deposit.q.TenantShortName;//Customer Name;
                dr[5] = deposit.p.BarcodeID;//BarcodeID
                dr[6] = deposit.p.DeclaredAmount;//Declared amount
                dr[7] = deposit.p.ActualAmount;//Actual amount
                dr[8] = deposit.p.CounterfeitAmount;//Counterfeit amount
                dr[9] = deposit.p.DiscrepancyAmount;//Discrepancy amount
                dr[10] = deposit.p.CountingPeople;//Counting People              
                dr[11] = deposit.p.LastEdit;//Quantity
                dr[12] = DateTimeString2String(deposit.p.TimeTag);//ID
                dr[13] = deposit.p.Checked == "T" ? true : false;
                

                // if (m_aeonMallDB.isExist(deposit.p.CustomerID))
                DataTableAM.Rows.Add(dr);

            }
        }
        private void loadTableAV(DateTime dt, string txtSearch)
        {
            if (txtSearch == null)
            {
                txtSearch = "";

            }
            var DateTime2String = dt.ToString("yyyyMMdd");
            var lstDepositVN = (from p in _context.DepositHistories
                               from q in _context.AeonVNCustomers
                               where ((q.IntermediateBagBarcode == p.BarcodeID) || (q.SaleBagBarcode == p.BarcodeID)) && p.SaleDate.Contains(DateTime2String)
                               select new { p, q }).ToList();
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
                    dr[1] = DateTime.ParseExact(depositVN.p.DepositDate, "yyyyMMddHHmmss", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");//Date
                    dr[2] = DateTime.ParseExact(depositVN.p.SaleDate, "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");//SaleDate
                    dr[3] = depositVN.q.PosName;//CustomerID                   
                    dr[4] = depositVN.p.BarcodeID;//BarcodeID
                    dr[5] = depositVN.p.DeclaredAmount;//Declared amount
                    dr[6] = depositVN.p.ActualAmount;//Actual amount
                    dr[7] = depositVN.p.CounterfeitAmount;//Counterfeit amount
                    dr[8] = depositVN.p.DiscrepancyAmount;//Discrepancy amount
                    dr[9] = depositVN.p.CountingPeople;//Counting People              
                    dr[10] = depositVN.p.LastEdit;//Quantity
                    dr[11] = DateTimeString2String(depositVN.p.TimeTag);//ID
                    dr[12] = depositVN.p.Checked == "T" ? true : false;

                    // if (m_aeonMallDB.isExist(deposit.p.CustomerID))
                    DataTableAV.Rows.Add(dr);                                      
                }

            }
        }
        string DateTimeString2String(string DatetimeYYYYMMDDHHMMSS)
        {
            return DateTime.ParseExact(DatetimeYYYYMMDDHHMMSS, "yyyyMMddHHmmss", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");//Date
        }


    }
}
