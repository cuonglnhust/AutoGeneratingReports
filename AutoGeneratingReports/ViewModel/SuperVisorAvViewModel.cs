using AutoGeneratingReports.Common;
using AutoGenReport.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AutoGeneratingReports.ViewModel
{
    public class SuperVisorAvViewModel : ObservableObject
    {
        public int SuperVisorID { get; set; }
        public Action CloseAction { get; set; }
        public ICommand btnSaveSupAV { get; set; }
        private string _SupCode;
        
        public string SupCode
        {
            get { return _SupCode; }
            set { _SupCode = value; OnPropertyChanged("SupCode");}
        }
        private string _SupName;
        public string SupName
        {
            get { return _SupName; }
            set { _SupName = value; OnPropertyChanged("SupName");}
        }
        private string _RegisterDay;
        public string RegisterDay
        {
            get { return _RegisterDay; }
            set { _RegisterDay = value; OnPropertyChanged("ShortNameAM");}
        }
        private string _Note;
        public string Note
        {
            get { return _Note; }
            set { _Note = value; OnPropertyChanged("Note");}
        }
        public SuperVisorAvViewModel()
        {
            btnSaveSupAV = new RelayCommand<object>((p) => { return true; }, (p) => { SaveSupAv(p); });
        }
        public SuperVisorAvViewModel(int idSup)
        {
            btnSaveSupAV = new RelayCommand<object>((p) => { return true; }, (p) => { SaveSupAv(p); });
            SuperVisorID = idSup;
            Load_Form();
        }
        public void Load_Form()
        {
            AutoGenReportDbContext m_safenetDataContext = new AutoGenReportDbContext();
            if (SuperVisorID != null || SuperVisorID > 0)
            {
                var AvSup = (from p in m_safenetDataContext.AeonVNSups
                                  where p.AVSupID == SuperVisorID
                                  select p).SingleOrDefault();
                SupCode = AvSup.SupCode;
                SupName = AvSup.SupName;
                RegisterDay = AvSup.RegisterDay;
                Note = AvSup.Note;                
            }
        }
        public void SaveSupAv(object obj)
        {
            AutoGenReportDbContext m_safenetDataContext = new AutoGenReportDbContext();
            try
            {
                AeonVNSup AvSupvisor = null;
                if (SuperVisorID > 0)
                {
                    //UPDATE
                    AvSupvisor = (from p in m_safenetDataContext.AeonVNSups
                                  where p.AVSupID == SuperVisorID
                                  select p).SingleOrDefault();
                    AvSupvisor.SupCode = SupCode;
                    AvSupvisor.SupName = SupName;
                    AvSupvisor.RegisterDay = RegisterDay;
                    AvSupvisor.Note = Note;                    
                    AvSupvisor.Type = "UPDATED";

                }
                else
                {
                    //CREATE NEW ONE
                    AvSupvisor = new AeonVNSup();
                    AvSupvisor.SupCode = SupCode;
                    AvSupvisor.SupName = SupName;
                    AvSupvisor.RegisterDay = RegisterDay;
                    AvSupvisor.Note = Note;
                    m_safenetDataContext.AeonVNSups.Add(AvSupvisor);
                    AvSupvisor.Type = "NEW";
                }
                m_safenetDataContext.SaveChanges();
                DSNguoiDamSatAV NguoiGiamSat = new DSNguoiDamSatAV();
                CloseAction();
                NguoiGiamSat.ShowDialog();
                //this.DialogResult = DialogResult.OK;
                
            }
            catch (Exception ex)
            {
                HelperClass.writeExceptionToDebugger(ex);
            }
        }
    }
}
