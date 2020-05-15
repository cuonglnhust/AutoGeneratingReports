using AutoGeneratingReports.Common;
using AutoGenReport.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AutoGeneratingReports.ViewModel
{
    public class frmCustomerAmViewModel: ObservableObject
    {
        public Action CloseAction { get; set; }
        private string _CodeAM;
        public int CustomerAmID { get; set; }
        public string CodeAM
        {
            get { return _CodeAM; }
            set { _CodeAM = value; OnPropertyChanged("CodeAM");}
        }
        private string _FullNameAM;
        public string FullNameAM
        {
            get { return _FullNameAM; }
            set { _FullNameAM = value; OnPropertyChanged("FullNameAM");}
        }
        private string _ShortNameAM;
        public string ShortNameAM
        {
            get { return _ShortNameAM; }
            set { _ShortNameAM = value; OnPropertyChanged("ShortNameAM");}
        }
        private string _DateOpen;
        public string DateOpen
        {
            get { return _DateOpen; }
            set { _DateOpen = value; OnPropertyChanged("DateOpen");}
        }
        private string _NoteAM;
        public string NoteAM
        {
            get { return _NoteAM; }
            set { _NoteAM = value; OnPropertyChanged("NoteAM");}
        }
        public System.Windows.Input.ICommand btnSaveCustomerAM { get; set; }
        public frmCustomerAmViewModel()
        {
            btnSaveCustomerAM = new RelayCommand<object>((p) => { return true; }, (p) => { SaveCustomerAm(p); });
            
        }
        public frmCustomerAmViewModel(int IDCustomerAm)
        {
            CustomerAmID = IDCustomerAm;
            Load_Form();
        }
        public void Load_Form()
        {
            AutoGenReportDbContext m_safenetDataContext = new AutoGenReportDbContext();
            if(CustomerAmID != null || CustomerAmID > 0)
            {
                var amCustomer = (from p in m_safenetDataContext.AeonMallCustomers
                                  where p.AMCustomerID == CustomerAmID
                                  select p).SingleOrDefault();
                CodeAM = amCustomer.TenantCode;
                FullNameAM = amCustomer.TenantName;
                ShortNameAM = amCustomer.TenantShortName;
                DateOpen = amCustomer.OpenningDate;
                NoteAM = amCustomer.Note;
            }
        }
        public void SaveCustomerAm(object obj)
        {
            AutoGenReportDbContext m_safenetDataContext = new AutoGenReportDbContext();
            try
            {
                AeonMallCustomer amCustomer = null;

                if (CustomerAmID > 0)
                {
                    //UPDATE
                    amCustomer = (from p in m_safenetDataContext.AeonMallCustomers
                                  where p.AMCustomerID == CustomerAmID
                                  select p).SingleOrDefault();
                    amCustomer.TenantCode = CodeAM;
                    amCustomer.TenantName = FullNameAM;
                    amCustomer.TenantShortName = ShortNameAM;
                    amCustomer.OpenningDate = DateOpen;
                    amCustomer.Note = NoteAM;
                    amCustomer.Type = "UPDATED";

                }
                else
                {
                    //CREATE NEW ONE
                    amCustomer = new AeonMallCustomer();
                    amCustomer.TenantCode = CodeAM;
                    amCustomer.TenantName = FullNameAM;
                    amCustomer.TenantShortName = ShortNameAM;
                    amCustomer.OpenningDate = DateOpen;
                    amCustomer.Note = NoteAM;
                    m_safenetDataContext.AeonMallCustomers.Add(amCustomer);
                    amCustomer.Type = "NEW";
                }


                m_safenetDataContext.SaveChanges();
                QuanLyKhachHangAeonMall aeonMall = new QuanLyKhachHangAeonMall();
                aeonMall.ShowDialog();
                //this.DialogResult = DialogResult.OK;
                CloseWindow();
            }
            catch (Exception ex)
            {
                HelperClass.writeExceptionToDebugger(ex);
            }
        }
        private void CloseWindow()
        {
            CloseAction();
        }
    }
}
