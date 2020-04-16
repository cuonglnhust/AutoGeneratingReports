using AutoGeneratingReports.Common;
using AutoGeneratingReports.Custom;
using AutoGeneratingReports.ViewModel;
using AutoGenReport.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AutoGeneratingReports
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly AutoGenReportDbContext _DbContext;
        public LoginWindow()
        {
            
            LoginViewModel VM  = new LoginViewModel(_DbContext);
            this.DataContext = VM;
            if (VM.CloseAction == null)
            {
                VM.CloseAction = new Action(() => this.Close());
            }
            InitializeComponent();
        }             

    }
}
