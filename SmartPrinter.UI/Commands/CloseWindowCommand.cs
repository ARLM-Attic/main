using System;
using System.Windows;

namespace SmartPrinter.UI.Commands
{
    public class CloseWindowCommand : BaseCommand
    {
        public override void Execute(object parameter)
        {
            var window = parameter as Window;

            if (window == null)
                throw new ArgumentException("CommandParameter must be System.Windows.Window");
            
            window.Close();
        }
    }
}