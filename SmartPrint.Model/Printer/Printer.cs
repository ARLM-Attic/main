using System;
using System.Linq;
using System.Collections.Generic;

namespace SmartPrint.Model
{
    public class Printer
    {
        private readonly FolderMonitor _monitor = new FolderMonitor();

        private List<PrinterAction> _actions = new List<PrinterAction>();

        public Printer()
        {
            _monitor.FilePrintingStarted += OnMonitorOnFilePrintingStarted;
            _monitor.FilePrintingFinished += OnMonitorOnFilePrintingFinished;
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string OutputPath { get; set; }

        public List<PrinterAction> Actions
        {
            get { return _actions; }
            set { _actions = value; }
        }

        public void StartMonitoring()
        {
            if (OutputPath == null)
                throw new InvalidOperationException("OutputPath is not set.");

            _monitor.Start(OutputPath);

            Toaster.ToastInfo(Name, String.Format("Monitoring {0} path", OutputPath));
        }

        #region Private methods

        private void OnMonitorOnFilePrintingStarted(string filePath)
        {
            Toaster.ToastInfo(Name, String.Format("PRINTING STARTED\n{0}", filePath));
        }

        private void OnMonitorOnFilePrintingFinished(string filePath)
        {
            Toaster.ToastInfo(Name, String.Format("PRINTING FINISHED\n{0}", filePath));

            // load content from filePath
            byte[] content = new byte[1];

            foreach (var a in Actions)
            {
                if (a is PdfAction)
                    ((PdfAction)a).PdfBytes = content;

                a.Execute();
            }
        }

        #endregion
    }
}