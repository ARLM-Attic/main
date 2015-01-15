using System;
using System.Windows;
using System.Windows.Input;
using SmartPrint.Model.ViewModels;

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

        public static readonly DependencyProperty ShellProperty =
            DependencyProperty.Register("Shell", typeof(ShellVM), typeof(BaseCommand), new PropertyMetadata(default(ShellVM)));

        public ShellVM Shell
        {
            get { return (ShellVM)GetValue(ShellProperty); }
            set { SetValue(ShellProperty, value); }
        }
    }
}