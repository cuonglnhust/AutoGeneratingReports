using AutoGeneratingReports.Common;
using AutoGenReport.Model;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly AutoGenReportDbContext _context;
        private int selectIndex;
        private Ribbon _Ribbon;
        public int Index
        {
            get { return selectIndex; }
            set
            {
                selectIndex = value;
                OnPropertyChanged("Index");
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
        public ICommand btn_Thoat { get; set; } 
        private Ribbon RibbonAM
        {
            get { return _Ribbon; }
            set
            {
                _Ribbon = value;
                if(Index == 0)
                {
                    OnPropertyChanged("TabAeonMall");
                }
               
            }
        }
        public MainWindowViewModel(AutoGenReportDbContext context)
        {
            
            //var x = Properties.Settings.Default.Username;
            //_context = context;
            //if(x == "admin")
            //{
            //    MessageBox.Show(x);
            //}

        }
       
        
    }
}
