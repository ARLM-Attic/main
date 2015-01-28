using SmartPrint.Model.ViewModels;

namespace SmartPrint.Model
{
    public static class Toaster
    {
        public static event ToastEventHandler ToastRaised;

        public static void ToastInfo(string message)
        {
            if (ToastRaised != null)
                ToastRaised(new ToastEventArgs { Message = message, Icon = "Info" });
        }

        public static void ToastInfo(string title, string message)
        {
            if (ToastRaised != null)
                ToastRaised(new ToastEventArgs { Title = title, Message = message, Icon = "Info" });
        }

        public static void ToastWarning(string message)
        {
            if (ToastRaised != null)
                ToastRaised(new ToastEventArgs { Message = message, Icon = "Warning" });
        }

        public static void ToastError(string message)
        {
            if (ToastRaised != null)
                ToastRaised(new ToastEventArgs { Message = message, Icon = "Error" });
        }
    }
}