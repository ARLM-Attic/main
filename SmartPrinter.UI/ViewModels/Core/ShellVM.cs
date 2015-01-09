using System;
using System.Windows;
using System.Windows.Threading;
using SmartPrint.Model.Helpers;
using SmartPrint.UI;
using SmartPrinter.UI.ViewModels.Core;

namespace SmartPrint.Model.ViewModels
{
    public class ShellVM
    {
        #region Private fields

        private readonly Dispatcher _dispatcher = Application.Current.Dispatcher;
        private readonly FolderMonitor _monitor = new FolderMonitor();
        private readonly Toaster _toaster = new Toaster();
        private readonly PrintFormVM _vm = new PrintFormVM();

        #endregion

        #region Properties

        public Toaster Toaster { get { return _toaster; } }

        #endregion

        #region Public Methods

        public void StartMonitoring(string path)
        {
            _monitor.FilePrintingStarted += OnMonitorOnFilePrintingStarted;
            _monitor.FilePrintingFinished += OnMonitorOnFilePrintingFinished;
            _monitor.Start(path);

            _toaster.ToastInfo("Printer is running...");
        }

        #endregion

        #region Private methods

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

            _toaster.ToastInfo("Document is prepared.");
        }

        private void ShowForm(string filePath)
        {
            _vm.PostScriptFilePath = filePath;

            var printForm = new PrintForm();

            printForm.Show();

            printForm.DataContext = _vm;

            printForm.Topmost = true;

            _toaster.ToastInfo(String.Format("Preparing {0}", FileHelper.ExtractFilename(filePath)));
        }

        #endregion

    }
}