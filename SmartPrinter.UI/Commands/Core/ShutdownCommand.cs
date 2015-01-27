using System.Windows;

namespace SmartPrinter.UI.Commands
{
    public class ShutdownCommand : BaseCommand
    {
        public override void Execute(object parameter)
        {
            Application.Current.Shutdown();
        }
    }
}