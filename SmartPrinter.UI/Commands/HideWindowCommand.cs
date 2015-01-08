using System.Windows;

namespace SmartPrinter.UI.Commands
{
    public class HideWindowCommand : BaseCommand
    {
        public override void Execute(object parameter)
        {
            Application.Current.MainWindow.Hide();
        }
    }
}