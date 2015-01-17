using System;
using System.Collections.Generic;

namespace SmartPrint.Model
{
    public class Printer
    {
        private readonly FolderMonitor _monitor = new FolderMonitor();

        public Guid Id { get; set; }
        
        public string Name { get; set; }

        public string Description { get; set; }

        public List<PrinterAction> Actions { get; set; }

        public void StartMonitoring(string path)
        {
            _monitor.FilePrintingStarted += OnMonitorOnFilePrintingStarted;
            _monitor.FilePrintingFinished += OnMonitorOnFilePrintingFinished;
            _monitor.Start(path);

            Toaster.ToastInfo(String.Format("Monitoring {0} path", path));
        }

        #region Private methods

        private void OnMonitorOnFilePrintingStarted(string filePath)
        {
            //if (_dispatcher.CheckAccess())
            //    ShowForm(filePath);
            //else
            //    _dispatcher.BeginInvoke((Action)(() => ShowForm(filePath)));
        }

        private void OnMonitorOnFilePrintingFinished(string filePath)
        {
            //if (_dispatcher.CheckAccess())
            //    PrintingFinished();
            //else
            //    _dispatcher.BeginInvoke((Action)PrintingFinished);
        }

        private void PrintingFinished()
        {
            //_vm.PostScriptCreated = true;

            //_toaster.ToastInfo("Document is prepared.");
        }

        private void ShowForm(string filePath)
        {
            //_vm.PostScriptFilePath = filePath;

            //var printForm = new PrintForm();

            //printForm.Show();

            //printForm.DataContext = _vm;

            //printForm.Topmost = true;

            //_toaster.ToastInfo(String.Format("Preparing {0}", FileHelper.ExtractFilename(filePath)));
        }

        #endregion


    }
}
