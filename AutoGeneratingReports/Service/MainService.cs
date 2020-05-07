using AutoGenReport.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoGeneratingReports.Service
{
    public class MainService
    {
        private readonly AutoGenReportDbContext _dbcontext;
        public MainService(AutoGenReportDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public void UpdateCheckbox(string DepositHistoryID, string Checked)
        {
            if(Checked == "False")
            {
                Checked = "True";
            }
            else
            {
                Checked = "False";
            }
            var DepositChecked = _dbcontext.DepositHistories.Where(x => x.DepositHistoryID.Contains(DepositHistoryID)).FirstOrDefault();
            var FindCheck = DepositChecked.Checked == "T" ? true : false;
            if(FindCheck.ToString() != Checked)
            {
                DepositChecked.Checked = Checked == "True" ? "T" : "F";
                _dbcontext.SaveChanges();
            }
        }
    }
}
