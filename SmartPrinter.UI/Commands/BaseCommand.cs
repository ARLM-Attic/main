using System;
using System.Windows;
using System.Windows.Input;

namespace SmartPrinter.UI.Commands
{
    public abstract class BaseCommand : Freezable, ICommand
    {
        public abstract void Execute(object parameter);

        public virtual bool CanExecute(object parameter)
        {
            return true;
        }

#pragma warning disable 67
        public event EventHandler CanExecuteChanged;
#pragma warning restore 67

        protected override Freezable CreateInstanceCore()
        {
            return null;
        }
    }
}