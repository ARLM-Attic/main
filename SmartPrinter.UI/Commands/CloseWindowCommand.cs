using System.Windows;

namespace SmartPrinter.UI.Commands
{
    public class CloseWindowCommand : BaseCommand
    {
        public override void Execute(object parameter)
        {
            ((Window)parameter).Close();
        }
    }
}