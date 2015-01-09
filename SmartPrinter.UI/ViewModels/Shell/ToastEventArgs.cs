using Hardcodet.Wpf.TaskbarNotification;

namespace SmartPrint.Model.ViewModels
{
    public class ToastEventArgs
    {
        public string Message { get; set; }

        public BalloonIcon Icon { get; set; }
    }
}