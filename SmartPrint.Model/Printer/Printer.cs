using System;
using System.Linq;
using System.Collections.Generic;

namespace SmartPrint.Model
{
    public class Printer
    {
        private readonly FolderMonitor _monitor = new FolderMonitor();

        private List<PrinterAction> _actions = new List<PrinterAction>();

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<PrinterAction> Actions
        {
            get { return _actions; }
            set { _actions = value; }
        }

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
            Toaster.ToastInfo(String.Format("New file printing is started on {0}", Name));

            // load content from filePath
            byte[] content = new byte[0];

            foreach (var a in Actions)
            {
                if (a is PdfAction)
                    ((PdfAction)a).PdfBytes = content;

                a.Execute();
            }
        }

        private void OnMonitorOnFilePrintingFinished(string filePath)
        {
            Toaster.ToastInfo(String.Format("New file printing is finished on {0}", Name));
        }

        #endregion
    }
}