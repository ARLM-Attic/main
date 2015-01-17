using System.Windows;
using Hardcodet.Wpf.TaskbarNotification;
using SmartPrint.Model;
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

            Toaster.ToastRaised += a => ShowBalloon(a.Message, a.Icon);

            _shellVM = new ShellVM();

            _notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");

            if (_notifyIcon != null)
                _notifyIcon.DataContext = _shellVM;

            _shellVM.Initialize();

            
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _notifyIcon.Dispose();

            base.OnExit(e);
        }

        private void ShowBalloon(string message, string iconName)
        {
            BalloonIcon icon = BalloonIcon.None;
            
            switch (iconName)
            {
                case "Error" :
                    icon = BalloonIcon.Error;
                    break;
                case "Warning" :
                    icon = BalloonIcon.Warning;
                    break;
                case "Info":
                    icon = BalloonIcon.Info;
                    break;
            }

            if (_notifyIcon != null)
                _notifyIcon.ShowBalloonTip("SMARTdoc Share", message, icon);
        }
    }
}