using AutoGeneratingReports.ViewModel;
using System;
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

namespace AutoGeneratingReports
{
    /// <summary>
    /// Interaction logic for DSNguoiDamSatAV.xaml
    /// </summary>
    public partial class DSNguoiDamSatAV : Window
    {
        public DSNguoiDamSatAV()
        {
            SuperVisorManagementViewModel VM = new SuperVisorManagementViewModel();
            this.DataContext = VM;
            if (VM.CloseAction == null)
            {
                VM.CloseAction = new Action(() => this.Close());
            }
            InitializeComponent();
        }

        private void SuperVisor_Loading(object sender, DataGridRowEventArgs e)
        {
            
            SuperVisorTable.Columns[7].Visibility = Visibility.Hidden;
            SuperVisorTable.Columns[1].Visibility = Visibility.Hidden;
            //SuperVisorTable.Columns[5].Visibility = Visibility.Hidden;
            //SuperVisorTable.Columns[9].Visibility = Visibility.Hidden;


            SuperVisorTable.Columns[1].IsReadOnly = true;
            SuperVisorTable.Columns[2].IsReadOnly = true;
            SuperVisorTable.Columns[3].IsReadOnly = true;
            SuperVisorTable.Columns[4].IsReadOnly = true;
            SuperVisorTable.Columns[5].IsReadOnly = true;
            SuperVisorTable.Columns[6].IsReadOnly = true;
            SuperVisorTable.Columns[7].IsReadOnly = true;
            
            
        }

        private void Click_Double(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                var curentCell = SuperVisorTable.CurrentCell;
                var row = (DataRowView)curentCell.Item;

                if (row != null)
                {
                    var IDSup = (int)row.Row["ID"];
                    //var row2 = row.Item as DataRow;
                    //var idCus = int.Parse(IDSup);
                    SuperVisorAV SupVisorForm = new SuperVisorAV(IDSup);
                    this.Close();
                    SupVisorForm.ShowDialog();
                }
            }
        }
    }
}
