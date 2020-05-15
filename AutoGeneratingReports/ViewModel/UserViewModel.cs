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
using static AutoGeneratingReports.ViewModel.LoginViewModel;

namespace AutoGeneratingReports.ViewModel
{
    public class UserViewModel : ObservableObject
    {
        AutoGenReportDbContext m_safenetLocalContext;
        public void loadDataContext(AutoGenReportDbContext safenetDC)
        {
            m_safenetLocalContext = safenetDC;
        }
        public string TenDangNhap { get; set; }
        public ICommand btnSaveUser { get; set; }
        public ICommand btnExitAdd { get; set; }
        public System.Action CloseAction { get; set; }
        private DateTime _TimeCreateCheckList;
        private DataTable _DataTableCheckBox;
        public DataTable DataTableCheckBox
        {
            get { return _DataTableCheckBox; }
            set { _DataTableCheckBox = value; OnPropertyChanged("DataTableCheckBox"); }
        }
        public DateTime TimeCreateCheckList
        {
            get
            {
                return _TimeCreateCheckList;
            }
            set
            {
                _TimeCreateCheckList = value;
                OnPropertyChanged(nameof(TimeCreateCheckList));
            }
        }
        private DateTime _TimeEditFrom;
        public DateTime TimeEditFrom
        {
            get
            {
                return _TimeEditFrom;
            }
            set
            {
                _TimeEditFrom = value;
                OnPropertyChanged(nameof(TimeEditFrom));
            }
        }
        private DateTime _TimeEditTo;
        public DateTime TimeEditTo
        {
            get
            {
                return _TimeEditTo;
            }
            set
            {
                _TimeEditTo = value;
                OnPropertyChanged(nameof(TimeEditTo));
            }
        }
        private string _UserPassword = "123456";
        public string UserPassword
        {
            get { return _UserPassword; }
            set { _UserPassword = value; OnPropertyChanged("UserPassword");}
        }
        private string _Description;
        public string Description
        {
            get { return _Description; }
            set { _Description = value; OnPropertyChanged("Description");}
        }
        private string _UserName;
        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; OnPropertyChanged("UserName"); }
        }

        private string _EnableName;
        public string EnableName
        {
            get { return _EnableName; }
            set { _EnableName = value; OnPropertyChanged("EnableName"); }
        }
        private string _BackgroundColor;
        public string BackgroundColor
        {
            get { return _BackgroundColor; }
            set { _BackgroundColor = value; OnPropertyChanged("BackgroundColor"); }
        }

        public UserViewModel(string Username)
        {
            TenDangNhap = Username;
            btnSaveUser = new RelayCommand<object>((p) => { return true; }, (p) => { SaveUser(p); });
            InitGridUser();
            LoadCheckBox();
        }
        public UserViewModel()
        {            
            btnSaveUser = new RelayCommand<object>((p) => { return true; }, (p) => { SaveUser(p); });
            InitGridUser();
            LoadCheckBox();
        }
        public void SaveUser(object obj)
        {
            AutoGenReportDbContext m_safenetLocalContext = new AutoGenReportDbContext();
            try
            {
                var users = (from p in m_safenetLocalContext.Users where p.Username == UserName select p);
                if (users.Count() > 0)
                {
                    var user = users.SingleOrDefault();
                    //user.Password = txtPassword.Text;
                    user.Time2GenCheckList = "" + ConvertTimeToString.ConvertToString(TimeCreateCheckList);
                    user.TimeStart2Edit = "" + ConvertTimeToString.ConvertToString(TimeEditFrom);
                    user.TimeEnd2Edit = "" + ConvertTimeToString.ConvertToString(TimeEditTo);
                    user.Description = Description;
                }
                else
                {
                    //Create new user
                    User user = new User();
                    user.Username = UserName;
                    user.Password = CredentialsSource.GetHash(UserPassword);
                    user.Time2GenCheckList = "" + ConvertTimeToString.ConvertToString(TimeCreateCheckList);
                    user.TimeStart2Edit = "" + ConvertTimeToString.ConvertToString(TimeEditFrom);
                    user.TimeEnd2Edit = "" + ConvertTimeToString.ConvertToString(TimeEditTo);
                    user.Description = "" + Description;
                    m_safenetLocalContext.Users.Add(user);
                    //m_safenetLocalContext.SaveChanges();
                }
                var listRole = m_safenetLocalContext.Users_Roles.ToList();
                //Update/Add User_Roles Table
                //var user_roles = (from p in m_safenetLocalContext.Users_Roles where p.Username == txtUsername.Text select p);
                foreach (DataRow row in DataTableCheckBox.Rows)
                {

                    //Update
                    if ((bool)row["Cho phép (Checked)"] == true)
                    {
                        var test = (string)row["ID"];
                        var user_roles = (from p in listRole where p.Username == UserName && p.RoleID == (string)row["ID"] select p).ToList();
                        if (user_roles.Count() == 0)
                        {
                            Users_Roles user_role = new Users_Roles();
                            user_role.Username = UserName;
                            user_role.RoleID = (string)row["ID"];
                            m_safenetLocalContext.Users_Roles.Add(user_role);
                        }
                    }
                    else
                    {
                        var test2 = (string)row["ID"];
                        var user_roles = (from p in listRole where p.Username == UserName && p.RoleID == (string)row["ID"] select p);
                        if (user_roles.Count() > 0)
                        {
                            foreach (var user_role in user_roles)
                                m_safenetLocalContext.Users_Roles.Remove(user_role);
                        }

                    }

                }
                m_safenetLocalContext.SaveChanges();
                if(TenDangNhap == null)
                {
                    CloseAction();
                    //AutoGenReportDbContext m_safenetLocalContextX = new AutoGenReportDbContext();
                    DSNguoiDungWindow dSNguoiDung = new DSNguoiDungWindow();
                    dSNguoiDung.ShowDialog();
                }
                else
                {
                    MessageBox.Show("cập nhật thành công", "Thông báo", MessageBoxButton.OK);
                }
                
               
                //danhSach.updateUserTable();

                //this.DialogResult = DialogResult.OK;
                //this.Close();
            }
            catch (Exception ex)
            {
                HelperClass.writeExceptionToDebugger(ex);
                //this.DialogResult = DialogResult.Cancel;
            }
        

        }
        private void CloseWindow(System.Windows.Window window)
        {
            CloseAction();
        }
        public void InitGridUser()
        {
            try
            {
                DataTableCheckBox = new DataTable();
                DataTableCheckBox.Columns.Add("ID");
                DataTableCheckBox.Columns.Add("Quyền");
                DataTableCheckBox.Columns.Add("Cho phép (Checked)", typeof(bool));

            }
            catch (Exception ex)
            {
                HelperClass.writeExceptionToDebugger(ex);
            }

        }
       
        private void LoadCheckBox()
        {
            AutoGenReportDbContext m_safenetLocalContext = new AutoGenReportDbContext();
            try
            {
                if (TenDangNhap != null)
                {
                    var users = (from p in m_safenetLocalContext.Users where p.Username == TenDangNhap select p).SingleOrDefault();
                    UserName = users.Username;
                    EnableName = "True";
                    BackgroundColor = "LightGray";
                    UserPassword = users.Password;
                    Description = users.Description;
                    TimeCreateCheckList = ConvertTimeToString.ConvertToTime(users.Time2GenCheckList);
                    TimeEditFrom = ConvertTimeToString.ConvertToTime(users.TimeStart2Edit);
                    TimeEditTo = ConvertTimeToString.ConvertToTime(users.TimeEnd2Edit);

                }
                var roles = from p in m_safenetLocalContext.Roles select p;
                foreach (var role in roles)
                {
                    DataRow row = DataTableCheckBox.NewRow();
                    row[0] = role.RoleID;
                    row[1] = role.Description;
                    row[2] = false;
                    if (TenDangNhap != null)
                    {
                        var user_roles = from p in m_safenetLocalContext.Users_Roles where p.Username == TenDangNhap && p.RoleID == role.RoleID select p;
                        if (user_roles.Count() > 0)
                        {
                            row["Cho phép (Checked)"] = true;
                        }
                    }
                    DataTableCheckBox.Rows.Add(row);
                    DataTableCheckBox.Columns[0].ColumnMapping = MappingType.Hidden;
                    DataTableCheckBox.Columns[1].ReadOnly = true;
                }
            }
            catch (Exception ex)
            {
                HelperClass.writeExceptionToDebugger(ex);
            }

        }
      


    }
}
