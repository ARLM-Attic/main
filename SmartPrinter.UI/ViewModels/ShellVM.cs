using System;
using System.Windows;
using System.Windows.Threading;
using SmartPrint.UI;

namespace SmartPrint.Model.ViewModels
{
    public class ShellVM
    {
        private readonly PrintFormVM _vm = new PrintFormVM();
        private readonly Dispatcher _dispatcher = Application.Current.Dispatcher;
        private readonly FolderMonitor _monitor = new FolderMonitor();

        public ShellVM()
        {
            _monitor.FilePrintingStarted += OnMonitorOnFilePrintingStarted;
            _monitor.FilePrintingFinished += OnMonitorOnFilePrintingFinished;
            _monitor.Start("c:\\SmartPrinter\\Temp\\");
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
                _vm.PostScriptCreated = true;
            else
                _dispatcher.BeginInvoke((Action)(() => _vm.PostScriptCreated = true));
        }

        private void ShowForm(string filePath)
        {
            _vm.PostScriptFilePath = filePath;

            var printForm = new PrintForm();

            printForm.Show();

            printForm.DataContext = _vm;

            printForm.Topmost = true;
        }

    }
}