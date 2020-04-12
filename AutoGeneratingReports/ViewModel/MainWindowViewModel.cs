using AutoGeneratingReports.Common;
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
        public MainWindowViewModel()
        {
           
        }
       
        
    }
}
