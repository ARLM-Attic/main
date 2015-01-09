using System;
using System.Windows;
using System.Windows.Threading;
using Hardcodet.Wpf.TaskbarNotification;
using SmartPrint.Model.Helpers;
using SmartPrint.UI;

namespace SmartPrint.Model.ViewModels
{
    public class ShellVM
    {
        private readonly PrintFormVM _vm = new PrintFormVM();
        private readonly Dispatcher _dispatcher = Application.Current.Dispatcher;
        private readonly FolderMonitor _monitor = new FolderMonitor();

        public event ToastEventHandler Toast;

        public ShellVM()
        {
        }

        public void StartMonitoring()
        {
            _monitor.FilePrintingStarted += OnMonitorOnFilePrintingStarted;
            _monitor.FilePrintingFinished += OnMonitorOnFilePrintingFinished;
            _monitor.Start("c:\\SmartPrinter\\Temp\\");

            ToastInfo("Printer is running...");
        }

        private void OnMonitorOnFilePrintingStarted(string filePath)
        {
            if (_dispatcher.CheckAccess())
                ShowForm(filePath);
            else
                _dispatcher.BeginInvoke((Action)(() => ShowForm(filePath)));
        }

        private void OnMonitorOnFilePrintingFinished(string filePath)
        {
            if (_dispatcher.CheckAccess())
                PrintingFinished();
            else
                _dispatcher.BeginInvoke((Action)PrintingFinished);
        }

        private void PrintingFinished()
        {
            _vm.PostScriptCreated = true;

            ToastInfo("Document is prepared.");
        }

        private void ShowForm(string filePath)
        {
            ToastInfo(String.Format("Preparing {0}", FileHelper.ExtractFilename(filePath)));

            _vm.PostScriptFilePath = filePath;

            var printForm = new PrintForm();

            printForm.Show();

            printForm.DataContext = _vm;

            printForm.Topmost = true;
        }

        private void ToastInfo(string message)
        {
            if (Toast != null) 
                Toast(this, new ToastEventArgs { Message = message, Icon = BalloonIcon.Info });
        }

        private void ToastWarning(string message)
        {
            if (Toast != null)
                Toast(this, new ToastEventArgs { Message = message, Icon = BalloonIcon.Warning });
        }

        private void ToastError(string message)
        {
            if (Toast != null)
                Toast(this, new ToastEventArgs { Message = message, Icon = BalloonIcon.Error });
        }
    }
}