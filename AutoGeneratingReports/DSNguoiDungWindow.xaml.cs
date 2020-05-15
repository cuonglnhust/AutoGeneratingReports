using AutoGeneratingReports.Common;
using AutoGeneratingReports.ViewModel;
using AutoGenReport.Model;
using System;
using System.Collections;
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
using static AutoGeneratingReports.ViewModel.LoginViewModel;

namespace AutoGeneratingReports
{
    /// <summary>
    /// Interaction logic for DSNguoiDungWindow.xaml
    /// </summary>
    public partial class DSNguoiDungWindow : Window
    {
        private readonly AutoGenReportDbContext _DbContext = new AutoGenReportDbContext();
        public DSNguoiDungWindow()
        {
            DanhSachNguoiDungViewModel VM = new DanhSachNguoiDungViewModel(_DbContext);
            this.DataContext = VM;

            if (VM.CloseAction == null)
            {
                VM.CloseAction = new Action(() => this.Close());
            }
            InitializeComponent();
        }
     

      
        public IEnumerable<DataGridRow> GetDataGridRows(DataGrid grid)
        {
            var itemsSource = grid.ItemsSource as IEnumerable;
            if (null == itemsSource) yield return null;
            foreach (var item in itemsSource)
            {
                var row = grid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (null != row) yield return row;
            }
        }

        private void btnXoaClick(object sender, RoutedEventArgs e)
        {
            var dialogRst = MessageBox.Show("Bạn chắc chắn muốn xóa?", "Xác nhận", MessageBoxButton.OKCancel);
            var temp = 0;
            if(dialogRst.ToString() == "OK")
            {
                AutoGenReportDbContext m_safenetLocalContext = new AutoGenReportDbContext();
                object item = DataGridNguoiDung.SelectedItem;

                //string ID = (GridDataAM.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
                //MessageBox.Show(ID);
                var row_list = GetDataGridRows(DataGridNguoiDung);
                foreach (DataGridRow single_row in row_list)
                {
                    if (single_row.IsSelected == true)
                    {
                        temp += 1;
                        var userRow = (DataGridNguoiDung.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
                        var users = from p in m_safenetLocalContext.Users where p.Username == userRow select p;
                        if (users.Count() > 0)
                        {
                            var user = users.SingleOrDefault();
                            m_safenetLocalContext.Users.Remove(user);
                            m_safenetLocalContext.SaveChanges();
                        }
                        //Delete User_Roles
                        var user_roles = from p in m_safenetLocalContext.Users_Roles where p.Username == userRow select p;
                        if (user_roles.Count() > 0)
                        {
                            foreach (var user_role in user_roles)
                            {
                                m_safenetLocalContext.Users_Roles.Remove(user_role);
                            }

                            // m_safenetLocalContext.SubmitChanges();
                        }
                        m_safenetLocalContext.SaveChanges();
                    }
                    
                }
                if(temp == 0)
                {
                    MessageBox.Show("Chọn người dùng cần xóa", "Xác nhận", MessageBoxButton.OKCancel);
                }
            }
            
        }

        private void btnCapNhatQuyen(object sender, RoutedEventArgs e)
        {
            UpdateRoleTable();
        }
        #region UpdateRole
        private void UpdateRoleTable()
        {
            AutoGenReportDbContext m_safenetLocalContext = new AutoGenReportDbContext();
            var dialogRst = MessageBox.Show("Bạn có muốn cập nhật lại bảng Roles?", "Xác nhận", MessageBoxButton.OKCancel);
            if (dialogRst.ToString() == "OK")
            {

                m_safenetLocalContext.Database.ExecuteSqlCommand("TRUNCATE TABLE ROLES");
                m_safenetLocalContext.SaveChanges();
                dialogRst = MessageBox.Show("Đã cập nhật thành công", "Thông báo", MessageBoxButton.OKCancel);
            }
            // Add 2 more roles 
            //1	Hiển thị AM
            Role role1 = new Role();
            role1.RoleID = "1";
            role1.Description = "Hiển thị AM";
            m_safenetLocalContext.Roles.Add(role1);
            m_safenetLocalContext.SaveChanges();
            //1.1	Xuất báo cáo AM
            Role role11 = new Role();
            role11.RoleID = "1.1";
            role11.Description = "Xuất báo cáo AM";
            m_safenetLocalContext.Roles.Add(role11);
            m_safenetLocalContext.SaveChanges();
            //1.2	Nhập liệu AM
            Role role12 = new Role();
            role12.RoleID = "1.2";
            role12.Description = "Nhập liệu AM";
            m_safenetLocalContext.Roles.Add(role12);
            m_safenetLocalContext.SaveChanges();
            //1.3	Nhập liệu AM
            Role role13 = new Role();
            role13.RoleID = "1.3";
            role13.Description = "Cho phép xuất bảng kê AM";
            m_safenetLocalContext.Roles.Add(role13);
            m_safenetLocalContext.SaveChanges();
            //2	Hiển thị AV
            Role role2 = new Role();
            role2.RoleID = "2";
            role2.Description = "Hiển thị AV";
            m_safenetLocalContext.Roles.Add(role2);
            m_safenetLocalContext.SaveChanges();
            //2.1	Xuất báo cáo AV
            Role role21 = new Role();
            role21.RoleID = "2.1";
            role21.Description = "Xuất báo cáo AV";
            m_safenetLocalContext.Roles.Add(role21);
            m_safenetLocalContext.SaveChanges();
            //2.2	Nhập liệu AV
            Role role22 = new Role();
            role22.RoleID = "2.2";
            role22.Description = "Nhập liệu AV";
            m_safenetLocalContext.Roles.Add(role22);
            m_safenetLocalContext.SaveChanges();
            //2.3 Cho phép xuất bảng kê
            Role role23 = new Role();
            role23.RoleID = "2.3";
            role23.Description = "Cho phép xuất bảng kê Aeon VN";
            m_safenetLocalContext.Roles.Add(role23);
            m_safenetLocalContext.SaveChanges();
            //3	Chỉnh sửa cài đặt
            Role role3 = new Role();
            role3.RoleID = "3";
            role3.Description = "Chỉnh sửa cài đặt";
            m_safenetLocalContext.Roles.Add(role3);
            m_safenetLocalContext.SaveChanges();
            //3.1	Thay đổi đầu vào đầu ra
            Role role31 = new Role();
            role31.RoleID = "3.1";
            role31.Description = "Thay đổi đầu vào đầu ra";
            m_safenetLocalContext.Roles.Add(role31);
            m_safenetLocalContext.SaveChanges();
            //3.2	Thay đổi thời điểm xuất báo cáo
            Role role32 = new Role();
            role32.RoleID = "3.2";
            role32.Description = "Thay đổi thời điểm xuất báo cáo";
            m_safenetLocalContext.Roles.Add(role32);
            m_safenetLocalContext.SaveChanges();
            //3.3	Cho phép thay đổi khoảng thời gian được phép xử lý dữ liệu
            Role role33 = new Role();
            role33.RoleID = "3.3";
            role33.Description = "Cho phép thay đổi khoảng thời gian được phép xử lý dữ liệu";
            m_safenetLocalContext.Roles.Add(role33);
            m_safenetLocalContext.SaveChanges();
            //4	Quản lý người dùng
            Role role4 = new Role();
            role4.RoleID = "4";
            role4.Description = "Quản lý người dùng";
            m_safenetLocalContext.Roles.Add(role4);
            m_safenetLocalContext.SaveChanges();
            //5	Quản lý CSDL Khách hàng
            Role role5 = new Role();
            role5.RoleID = "5";
            role5.Description = "Quản lý CSDL Khách hàng";
            m_safenetLocalContext.Roles.Add(role5);
            m_safenetLocalContext.SaveChanges();
            //6	Tạo báo cáo CHECKLIST
            Role role6 = new Role();
            role6.RoleID = "6";
            role6.Description = "Tạo báo cáo CHECKLIST";
            m_safenetLocalContext.Roles.Add(role6);
            m_safenetLocalContext.SaveChanges();
            //7  Thêm cột kiểm soát
            Role role7 = new Role();
            role7.RoleID = "7";
            role7.Description = "Hiện cột kiểm soát";
            m_safenetLocalContext.Roles.Add(role7);
            m_safenetLocalContext.SaveChanges();
        }
        #endregion UpdateRole
        private void btnResetAll(object sender, RoutedEventArgs e)
        {
            AutoGenReportDbContext m_safenetLocalContext = new AutoGenReportDbContext();
            object item = DataGridNguoiDung.SelectedItem;
            var dialogRst = MessageBox.Show("Bạn có muốn cập nhật lại toàn bộ dl trong csdl?", "Xác nhận", MessageBoxButton.OKCancel);
            if (dialogRst.ToString() == "OK")
            {


                m_safenetLocalContext.Database.ExecuteSqlCommand("TRUNCATE TABLE DEPOSITHISTORY");
                m_safenetLocalContext.Database.ExecuteSqlCommand("TRUNCATE TABLE PROCESSEDFILES");
                m_safenetLocalContext.Database.ExecuteSqlCommand("TRUNCATE TABLE ROLES");
                m_safenetLocalContext.SaveChanges();
                dialogRst = MessageBox.Show("Đã xóa thành công", "Thông báo", MessageBoxButton.OKCancel);
            }
            UpdateRoleTable();




            dialogRst = MessageBox.Show("Mật khẩu mới sẽ trùng với tên đăng nhập?", "Xác nhận", MessageBoxButton.OKCancel);
            if (dialogRst.ToString() == "OK")
            {
                var row_list = GetDataGridRows(DataGridNguoiDung);
                try
                {
                    for (int i = 0; i < row_list.Count(); i++)
                    {
                        //DataRow row = row_list.GetDataRow(i);
                        var userRow = (DataGridNguoiDung.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
                        //Delete User
                        var users = from p in m_safenetLocalContext.Users where p.Username == userRow select p;
                        if (users.Count() > 0)
                        {
                            var user = users.SingleOrDefault();
                            user.Password = CredentialsSource.GetHash(user.Username);
                        }
                        m_safenetLocalContext.SaveChanges();
                        //MessageBox.Show(row[0].ToString());

                    }
                    UpdateRoleTable();
                }
                catch (Exception ex)
                {
                    HelperClass.writeExceptionToDebugger(ex);
                }
            }
        }

        private void border1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {          
            if (e.ClickCount == 2)
            {

                AutoGenReportDbContext m_safenetLocalContext = new AutoGenReportDbContext();
                object item = DataGridNguoiDung.SelectedItem;
                var row_list = GetDataGridRows(DataGridNguoiDung);
                foreach (DataGridRow single_row in row_list)
                {
                    if (single_row.IsSelected == true)
                    {
                        var userRow = (DataGridNguoiDung.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
                        
                            //UserViewModel viewModel = new UserViewModel();
                            NguoiDungWindow userForm = new NguoiDungWindow(userRow);
                            this.Close();                       
                            userForm.ShowDialog();
                        
                    }
                }

                //NguoiDungWindow userForm = new NguoiDungWindow();
                //this.Close();
                //userForm.ShowDialog();
            }
                   
            
        }

       
    }
}
