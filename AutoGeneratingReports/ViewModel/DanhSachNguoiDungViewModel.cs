using AutoGeneratingReports.Common;
using AutoGenReport.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AutoGeneratingReports.ViewModel
{
    public class DanhSachNguoiDungViewModel: ObservableObject
    {

        AutoGenReportDbContext dbContext = new AutoGenReportDbContext();
        private System.Data.DataTable _DataTableUser;
        public ICommand btnXoa { get; set; }
        public ICommand btnRefresh { get; set; }
        public ICommand btnResetAll { get; set; }
        public ICommand btnCapNhatQuyen { get; set; }
        public ICommand btnThemMoi { get; set; }
        public Action CloseAction { get; set; }
        public System.Data.DataTable DataTableUser
        {
            get { return _DataTableUser; }
            set
            {
                _DataTableUser = value;
                OnPropertyChanged("DataTableUser");
            }
        }
        //public Prototype SelectedItem SelectedItem {  }
        public DanhSachNguoiDungViewModel(AutoGenReportDbContext context)
        {
            dbContext = context;
            btnXoa = new RelayCommand<object>((p) => { return true; }, (p) => { AcionDelete(p);});
            btnThemMoi = new RelayCommand<Window>((p) => { return true; }, (p) => { ShowAddForm(p);});
            //btnXoa = 
            InitGridListUser();
            updateUserTable();

        }

        public string isBtnXoaVisible { get; set; }
        public bool BtnXoaCanVisible(object obj)
        {
            isBtnXoaVisible = "Hidden";
            return false;
        }

        public void AcionDelete(object obj)
        {
            updateUserTable();

        }
        public void ShowAddForm(Window wd)
        {
            NguoiDungWindow NguoiDung = new NguoiDungWindow();
            CloseAction();
            NguoiDung.ShowDialog();
            CloseAction();

        }
        public void InitGridListUser()
        {
            try
            {
                DataTableUser = new System.Data.DataTable();
                DataTableUser.Columns.Add("Tên đăng nhập");
                DataTableUser.Columns.Add("Thời gian tạo check List");
                DataTableUser.Columns.Add("Thời gian cho phép bắt đầu chỉnh sửa");
                DataTableUser.Columns.Add("Thời gian cho phép kết thúc chỉnh sửa");
                DataTableUser.Columns.Add("Mô tả");
                  
            }
            catch (Exception ex)
            {
                HelperClass.writeExceptionToDebugger(ex);
            }

        }
        public void updateUserTable()
        {
            AutoGenReportDbContext m_safenetLocalContext = new AutoGenReportDbContext();
            try
            {
                //DataTableUser.DataSource = null;
                DataTableUser.Clear();
                var results = (from p in m_safenetLocalContext.Users select p).ToList();
                

                foreach (var result in results)
                {
                    DataRow row = DataTableUser.NewRow();
                    row[0] = result.Username;
                    row[1] = result.Time2GenCheckList;
                    row[2] = result.TimeStart2Edit;
                    row[3] = result.TimeEnd2Edit;                    
                    row[4] = result.Description;

                    DataTableUser.Rows.Add(row);
                    
                }
                //gridControlUser.DataSource = DataTableUser;
            }
            catch (Exception ex)
            {
                HelperClass.writeExceptionToDebugger(ex);
            }

        }
       
        

    }
}
