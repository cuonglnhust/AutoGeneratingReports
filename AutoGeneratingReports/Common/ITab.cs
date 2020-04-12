using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AutoGeneratingReports.Common
{
    public interface ITab
    {
        string Name { get; set; }
        ICommand CloseCommand { get; }
        event EventHandler CloseRequested;
    }
    //public abstract class Tab : ITab
    //{
    //    public Tab()
    //    {
    //        CloseCommand = new ActionCommand()
    //    }
    //    public string Name { get; set; }
    //    public ICommand CloseCommand { get; }
    //    public event EventHandler CloseRequested;
    //}
}
