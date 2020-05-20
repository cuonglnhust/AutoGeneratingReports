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

namespace AutoGeneratingReports.ViewModel
{
    public class frmCustomerAvViewModel: ObservableObject
    {
        AutoGenReportDbContext m_safenetLocalContext = new AutoGenReportDbContext();
        public string m_strPosName { get; set; }
        public Action CloseAction { get; set; }
        private DataTable _DataTableBarcodeAv; 
        public DataTable DataTableBarcodeAv
        {
            get { return _DataTableBarcodeAv; }
            set { _DataTableBarcodeAv = value; OnPropertyChanged("DataTableBarcodeAv");}
        }
        private string _NameSupAV;
        public string NameSupAV
        {
            get { return _NameSupAV; }
            set { _NameSupAV = value; OnPropertyChanged("NameSupAV"); }
        }
        private string _CodeSupAV;
        public string CodeSupAV
        {
            get { return _CodeSupAV; }
            set { _CodeSupAV = value; OnPropertyChanged("CodeSupAV"); }
        }
        private string _LocationAV;
        public string LocationAV
        {
            get { return _LocationAV; }
            set { _LocationAV = value; OnPropertyChanged("LocationAV"); }
        }
        private string _floorAV;
        public string floorAV
        {
            get { return _floorAV; }
            set { _floorAV = value; OnPropertyChanged("floorAV"); }
        }
        private string _BarcodeNum = "10";
        public string BarcodeNum
        {
            get { return _BarcodeNum; }
            set { _BarcodeNum = value; OnPropertyChanged("BarcodeNum");}
        }
        public ICommand btnSaveCustomerAV { get; set; }
        public frmCustomerAvViewModel()
        {
            InitCustomerAV();
            btnSaveCustomerAV = new RelayCommand<object>((p) => { return true; }, (p) => { SaveAv(p); });
        }
        public frmCustomerAvViewModel(string m_strPosNameInput)
        {
            m_strPosName = m_strPosNameInput;
            InitCustomerAV();
            btnSaveCustomerAV = new RelayCommand<object>((p) => { return true; }, (p) => { SaveAv(p); });
        }
        private void InitCustomerAV()
        {
            try
            {
                DataTableBarcodeAv = new DataTable();
                DataTableBarcodeAv.Columns.Add("ID", typeof(int));
                DataTableBarcodeAv.Columns.Add("Mã vạch túi đỏ");
                DataTableBarcodeAv.Columns.Add("Mã vạch túi xanh");

                initTableRows();
            }
            catch (Exception ex)
            {
                HelperClass.writeExceptionToDebugger(ex);
            }

        }
        private void initTableRows()
        {
            if (m_strPosName != null)
            {
                var result = from p in m_safenetLocalContext.AeonVNCustomers
                             where p.PosName == m_strPosName
                             select p;

                foreach (var avCustomer in result)
                {
                    NameSupAV = avCustomer.PosName;
                    CodeSupAV = avCustomer.PosNumber;
                    floorAV = avCustomer.Floor;
                    LocationAV = avCustomer.PosLocation;
                    DataRow row = DataTableBarcodeAv.NewRow();
                    row["Mã vạch túi đỏ"] = avCustomer.SaleBagBarcode;
                    row["Mã vạch túi xanh"] = avCustomer.IntermediateBagBarcode;
                    row["ID"] = avCustomer.AVCustomerID;
                    DataTableBarcodeAv.Rows.Add(row);
                }
            }
            else
            {
                int nBarcodeNum = 10;
                //gridControl1.DataSource = null;
                DataTableBarcodeAv.Clear();
                if (int.TryParse(BarcodeNum, out nBarcodeNum) == false)
                {
                    nBarcodeNum = 10;
                }
                for (int i = 0; i < nBarcodeNum -1; i++)
                {
                    DataRow row = DataTableBarcodeAv.NewRow();
                    row["ID"] = 0;
                    row["Mã vạch túi đỏ"] = " ";
                    row["Mã vạch túi xanh"] = " ";
                    DataTableBarcodeAv.Rows.Add(row);
                }

            }
            //gridControl1.DataSource = m_dataTableBarcode;
            //DataTableCustomerAv.Columns["ID"].Visible = false;
        }
        private void SaveAv(object obj)
        {
            try
            {
                var query = 
                       (from p in DataTableBarcodeAv.AsEnumerable() select p);
                foreach (var barcode in query)
                {

                    if ((int)barcode["ID"] != 0)
                    {
                        var avCustomer = (from p in m_safenetLocalContext.AeonVNCustomers
                                          where  p.AVCustomerID == (int)barcode["ID"]
                                          select p).SingleOrDefault();
                        avCustomer.PosName = NameSupAV;
                        avCustomer.PosNumber = CodeSupAV;
                        avCustomer.PosLocation = LocationAV;
                        avCustomer.Floor = floorAV;
                        avCustomer.SaleBagBarcode = "" + barcode["Mã vạch túi đỏ"];
                        avCustomer.IntermediateBagBarcode = "" + barcode["Mã vạch túi xanh"];

                    }
                    else
                    {
                        AeonVNCustomer avCustomer = new AeonVNCustomer();
                        avCustomer.PosName = NameSupAV;
                        avCustomer.PosNumber = CodeSupAV;
                        avCustomer.PosLocation = LocationAV;
                        avCustomer.Floor = floorAV;
                        avCustomer.SaleBagBarcode = "" + barcode["Mã vạch túi đỏ"];
                        avCustomer.IntermediateBagBarcode = "" + barcode["Mã vạch túi xanh"];
                        m_safenetLocalContext.AeonVNCustomers.Add(avCustomer);
                    }
                }
                m_safenetLocalContext.SaveChanges();
                MessageBox.Show("Thêm mới thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                DSQuayAvWindow dSQuayAv = new DSQuayAvWindow();
                CloseAction();
                dSQuayAv.ShowDialog();
            }
            catch (Exception ex)
            {
                HelperClass.writeExceptionToDebugger(ex);
            }

        }
    }
}
