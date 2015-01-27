using System.Windows;

namespace SmartPrinter.UI.Commands
{
    public class ResizeWindowCommand : BaseCommand
    {
        public override void Execute(object parameter)
        {
            var window = Application.Current.MainWindow;

            if (window.WindowState == WindowState.Maximized)
                window.WindowState = WindowState.Normal;
            else
                window.WindowState = WindowState.Maximized;
        }
    }
}