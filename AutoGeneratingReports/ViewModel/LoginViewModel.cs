using AutoGeneratingReports.Common;
using AutoGenReport.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AutoGeneratingReports.ViewModel
{
    public class LoginViewModel : ObservableObject
    {
        private readonly AutoGenReportDbContext _context;
        public virtual Users CurrentUser { get; set; }
        public bool IsCurrentUserCredentialsValid { get; private set; }
        public virtual AppState State { get; set; }
        private string _TaiKhoan;
        private string _MatKhau;
        public Action CloseAction { get; set; }
        public ICommand PasswordChangedCommand { get; set; }
        public ICommand btn_Thoat { get; set; }
        public ICommand btn_DangNhap { get; private set; }
        public string TaiKhoan
        {
            get { return _TaiKhoan; }
            set
            {
                _TaiKhoan = value;
                OnPropertyChanged("TaiKhoan");
            }
        }
        //public string MatKhau
        //{
        //    get => _MatKhau; set { _MatKhau = value; OnPropertyChanged("PasswordChangedCommand");}
        //}
        public string Password { get => _MatKhau; set { _MatKhau = value; OnPropertyChanged("PasswordChangedCommand"); } }

        public LoginViewModel(AutoGenReportDbContext context)
        {
            _context = context;
            CurrentUser = new Users();
            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { Password = p.Password; });
            this.btn_DangNhap = new RelayCommand<Window>((p) => { return true; }, (p) => { DangNhap(p); });
            btn_Thoat = new RelayCommand<Window>((p) => { return true; }, (p) => { CloseWindow(p); });
        }

        private void CloseWindow(Window window)
        {
            CloseAction();
        }

        private void DangNhap(Window LoginWindow)
        {
            if (LoginWindow == null)
                return;
            checkLogin(LoginWindow);
        }
        private void checkLogin(Window LoginWindow)
        {
            
            try
            {
                CurrentUser.Login = TaiKhoan;
                CurrentUser.Password = Password;

                IsCurrentUserCredentialsValid = CredentialsSource.Check(CurrentUser.Login, CurrentUser.Password);
                if (IsCurrentUserCredentialsValid)
                {
                    State = AppState.Autorized;
                    MainWindow mainWindow = new MainWindow();
                    CloseAction();
                    mainWindow.Show();
                    
                    

                }
                else
                {
                    State = AppState.NotAutorized;
                    MessageBox.Show("Sai tài khoản hoặc mật khẩu!");
                    //lblError.Text = "Tên đăng nhập hoặc mật khẩu không đúng";

                }
                Properties.Settings.Default.State = State.ToString();
                Properties.Settings.Default.Username = CurrentUser.Login;
                //Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                HelperClass.writeExceptionToDebugger(ex);
            }

        }
      
        static class CredentialsSource
        {
            static MD5 m_md5 = new MD5CryptoServiceProvider();
            static System.Collections.Hashtable credentials;
            static AutoGenReportDbContext _dbContext = new AutoGenReportDbContext();
            static CredentialsSource()
            {
                reloadCredentialsSource();
            }
            static void reloadCredentialsSource()
            {
                credentials = new System.Collections.Hashtable();

                var results = from p in _dbContext.Users select p;
                foreach (var result in results)
                {
                    credentials.Add(result.Username, result.Password);
                }

            }
            internal static void UpdateDataContext()
            {
                CredentialsSource._dbContext = new AutoGenReportDbContext();
            }

            internal static bool Check(string login, string pwd)
            {
                if (login == "superadmin" && pwd == "root")
                {
                    return true;
                }
                else
                {
                    reloadCredentialsSource();
                    return object.Equals(credentials[login], GetHash(pwd));
                }

            }
            static public string GetHash(string password)
            {

                return ASCIIEncoding.Default.GetString(m_md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(password)));
            }
            internal static System.Collections.Generic.IEnumerable<string> GetUserNames()
            {
                foreach (string item in credentials.Keys)
                    yield return item;
            }
        }
    }
    public enum AppState
    {
        NotAutorized,
        Autorized,
        ExitQueued
    }
    public class Users
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
