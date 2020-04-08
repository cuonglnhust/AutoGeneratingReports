using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AutoGeneratingReports.Common
{
    public class RelayCommand<T>: ICommand
    {
        //readonly Action<object> _execute;
        //readonly Predicate<object> _canExecute;

        //public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        //{
        //    if (execute == null)
        //        throw new NullReferenceException("execute");

        //    _execute = execute;
        //    _canExecute = canExecute;
        //}

        //public RelayCommand(Action<object> execute) : this(execute, null) { }
        //public event EventHandler CanExecuteChanged
        //{
        //    add { CommandManager.RequerySuggested += value; }
        //    remove { CommandManager.RequerySuggested -= value; }
        //}

        //public bool CanExecute(object parameter)
        //{
        //    return _canExecute == null ? true : _canExecute(parameter);
        //}

        //public void Execute(object parameter)
        //{
        //    _execute(parameter);
        //}
        private readonly Predicate<T> _canExecute;
        private readonly Action<T> _execute;

        public RelayCommand(Predicate<T> canExecute, Action<T> execute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            _canExecute = canExecute;
            _execute = execute;
        }

        public bool CanExecute(object parameter)
        {
            try
            {
                return _canExecute == null ? true : _canExecute((T)parameter);
            }
            catch
            {
                return true;
            }
        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
