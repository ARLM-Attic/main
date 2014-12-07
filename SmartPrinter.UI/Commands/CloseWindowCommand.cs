using System;
using System.Windows;
using System.Windows.Input;

namespace SmartPrinter.UI.Commands
{
    public class CloseWindowCommand : ICommand
    {
        public void Execute(object parameter)
        {
            ((Window)parameter).Close();
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }
}
