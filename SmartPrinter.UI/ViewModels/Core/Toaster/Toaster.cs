using Hardcodet.Wpf.TaskbarNotification;
using SmartPrint.Model.ViewModels;

namespace SmartPrinter.UI.ViewModels.Core
{
    public class Toaster
    {
        public event ToastEventHandler ToastRaised;

        public void ToastInfo(string message)
        {
            if (ToastRaised != null)
                ToastRaised(this, new ToastEventArgs { Message = message, Icon = BalloonIcon.Info });
        }

        public void ToastWarning(string message)
        {
            if (ToastRaised != null)
                ToastRaised(this, new ToastEventArgs { Message = message, Icon = BalloonIcon.Warning });
        }

        public void ToastError(string message)
        {
            if (ToastRaised != null)
                ToastRaised(this, new ToastEventArgs { Message = message, Icon = BalloonIcon.Error });
        }
    }
}