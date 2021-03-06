﻿using AutoGeneratingReports.ViewModel;
using AutoGenReport.Model;
using System;
using System.Collections.Generic;
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

namespace AutoGeneratingReports
{
    /// <summary>
    /// Interaction logic for ChangePasswordWindow.xaml
    /// </summary>
    public partial class ChangePasswordWindow : Window
    {
        private readonly AutoGenReportDbContext _DbContext = new AutoGenReportDbContext();
        public ChangePasswordWindow()
        {
            ChangePasswordViewModel VM = new ChangePasswordViewModel(_DbContext);
            this.DataContext = VM;

            if (VM.CloseAction == null)
            {
                VM.CloseAction = new Action(() => this.Close());
            }
            InitializeComponent();
        }
        public ChangePasswordWindow(string UserName)
        {
            ChangePasswordViewModel VM = new ChangePasswordViewModel(_DbContext);
            this.DataContext = VM;
            VM.TenDangNhap = UserName;
            if (VM.CloseAction == null)
            {
                VM.CloseAction = new Action(() => this.Close());
            }
            InitializeComponent();
        }
    }
}
