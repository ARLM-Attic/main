using System.Windows;

namespace SmartPrinter.UI.Commands
{
    public class ShowWindowCommand : BaseCommand
    {
        public override void Execute(object parameter)
        {
            Application.Current.MainWindow.Show();
        }
    }
}