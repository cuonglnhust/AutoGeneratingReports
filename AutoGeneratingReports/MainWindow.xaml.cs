using AutoGeneratingReports.ViewModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AutoGeneratingReports
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = new MainWindowViewModel();
            InitializeComponent();
        }

        private void RibbonWin_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var indexer = RibbonWin.SelectedIndex;
            if(indexer == 0)
            {
                GridAeonMall.Visibility = Visibility.Visible;
                GridAeonVN.Visibility = Visibility.Hidden;
            }

            if(indexer == 1)
            {
                GridAeonMall.Visibility = Visibility.Hidden;
                GridAeonVN.Visibility = Visibility.Visible;
            }
            if(indexer == 2)
            {
                GridAeonMall.Visibility = Visibility.Hidden;
                GridAeonVN.Visibility = Visibility.Hidden;
            }
            if (indexer == 3)
            {
                GridAeonMall.Visibility = Visibility.Hidden;
                GridAeonVN.Visibility = Visibility.Hidden;
            }
            if (indexer == 4)
            {
                GridAeonMall.Visibility = Visibility.Hidden;
                GridAeonVN.Visibility = Visibility.Hidden;
            }
        }
       
    }
}
