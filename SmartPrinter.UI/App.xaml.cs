using System.Windows;
using System.Windows.Navigation;
using Hardcodet.Wpf.TaskbarNotification;
using SmartPrint.Model;
using SmartPrint.Model.Repository;
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

            _shellVM = new ShellVM(new XmlRepository(System.AppDomain.CurrentDomain.BaseDirectory));

            _notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");

            if (_notifyIcon != null)
                _notifyIcon.DataContext = _shellVM;

            _shellVM.Initialize();
        }

        public ShellVM Shell { get { return _shellVM; } }

        protected override void OnLoadCompleted(NavigationEventArgs e)
        {
            base.OnLoadCompleted(e);

            Current.MainWindow.DataContext = _shellVM;
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