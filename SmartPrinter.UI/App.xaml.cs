using System.Windows;
using Hardcodet.Wpf.TaskbarNotification;
using SmartPrint.Model.ViewModels;

namespace SmartPrinter.UI
{
    public partial class App
    {
        private TaskbarIcon _notifyIcon;

        private ShellVM _shellVM;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _shellVM = new ShellVM();

            _shellVM.Toast += (t, a) => ShowBalloon(a.Message, a.Icon);

            _notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");

            if (_notifyIcon != null)
                _notifyIcon.DataContext = _shellVM;

            // TODO: location should be in Program Files\SMARTdoc\PrinterConnector\Temp
            _shellVM.StartMonitoring("c:\\SmartPrinter\\Temp\\");
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _notifyIcon.Dispose();

            base.OnExit(e);
        }

        private void ShowBalloon(string message, BalloonIcon icon)
        {
            if (_notifyIcon != null)
                _notifyIcon.ShowBalloonTip("SMARTdoc PrintConnector", message, icon);
        }
    }
}