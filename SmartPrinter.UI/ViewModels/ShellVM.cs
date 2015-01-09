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

            NewInfo(new ToastEventArgs { Message = "Printer is running...", Icon = BalloonIcon.Info });
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

            NewInfo(new ToastEventArgs { Message = "Document is prepared.", Icon = BalloonIcon.Info });
        }

        private void ShowForm(string filePath)
        {
            NewInfo(new ToastEventArgs { Message = String.Format("Preparing {0}", FileHelper.ExtractFilename(filePath)), Icon = BalloonIcon.Info });

            _vm.PostScriptFilePath = filePath;

            var printForm = new PrintForm();

            printForm.Show();

            printForm.DataContext = _vm;

            printForm.Topmost = true;
        }

        private void NewInfo(ToastEventArgs args)
        {
            ToastEventHandler handler = Toast;
            if (handler != null) handler(this, args);
        }

    }
}