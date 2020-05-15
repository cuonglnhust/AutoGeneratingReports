using AutoGeneratingReports.Common;
using AutoGeneratingReports.Custom;
using AutoGenReport.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static AutoGeneratingReports.ViewModel.LoginViewModel;

namespace AutoGeneratingReports.ViewModel
{
   public class ChangePasswordViewModel : ObservableObject
    {
        private readonly AutoGenReportDbContext _context;
        private string _MatKhauHienTai;
        private string _MatKhauMoi;
        public string _UserName;
        public Action CloseAction { get; set; }
        public string TenDangNhap
        {
            get { return _UserName; }
            set
            {
                _UserName = value;
                OnPropertyChanged("TenDangNhap");
            }
        }
        public ICommand PasswordChangedCommandMatKhauCu { get; set; }
        public ICommand PasswordChangedCommandMatKhauMoi { get; set; }
        public ICommand btnLuu { get; set; }
        public string PasswordOld { get => _MatKhauHienTai; set { _MatKhauHienTai = value; OnPropertyChanged("PasswordChangedCommandMatKhauCu");}}
        public string PasswordNew { get => _MatKhauMoi; set { _MatKhauMoi = value; OnPropertyChanged("PasswordChangedCommandMatKhauMoi");}}


        public ChangePasswordViewModel(AutoGenReportDbContext context)
        {
            _context = context;
            //CurrentUser = new Users();
            PasswordChangedCommandMatKhauCu = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { 
                    PasswordOld = p.Password;
            });
            PasswordChangedCommandMatKhauMoi = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { PasswordNew = p.Password; });
            btnLuu = new RelayCommand<object>((p) => { return true; }, (p) => { SaveChangePasswod(p); });
           // ResetCheckAV = new RelayCommand<object>((p) => { return true; }, (p) => { ResetCheckBoxAM(p); });
            //btn_Thoat = new RelayCommand<Window>((p) => { return true; }, (p) => { CloseWindow(p); });

        }
        private void CloseWindow(Window window)
        {
            CloseAction();
        }
        private void SaveChangePasswod(object obj)
        {
            AutoGenReportDbContext safenetLocalDataContext = new AutoGenReportDbContext();
            
            if(PasswordOld == null || PasswordNew == null)
            {
                Dialog dialog = new Dialog();
                dialog.Message = "Mật khẩu không được bỏ trống";
                dialog.ShowDialog();
            }
            else
            {
                var result = (from p in safenetLocalDataContext.Users where p.Username == TenDangNhap select p);
                if (result != null && result.Count() > 0)
                {
                    var user = result.SingleOrDefault();
                    if (CredentialsSource.GetHash(PasswordOld) == user.Password)
                    {
                        Dialog dialog = new Dialog();
                        user.Password = (CredentialsSource.GetHash(PasswordNew));
                        safenetLocalDataContext.SaveChanges();
                        CloseAction();
                        dialog.Message = "Đã đổi mật khẩu thành công";                        
                        dialog.ShowDialog();
                        
                        //this.Close();
                    }
                    else
                    {
                        Dialog dialog = new Dialog();
                        dialog.Message = "Mật khẩu cũ không đúng! Vui lòng nhập lại";
                        dialog.ShowDialog();
                    }
                }
                else
                {
                    Dialog dialog = new Dialog();
                    dialog.Message = "Không tìm thấy tài khoản tương ứng";
                    dialog.ShowDialog();
                }

            }

        }



    }
}
