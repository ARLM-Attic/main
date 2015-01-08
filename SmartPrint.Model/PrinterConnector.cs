using SmartPrint.Model.Helpers;

namespace SmartPrint.Model
{
    public class PrinterConnector
    {
        private readonly PrinterOutputMonitor _monitor = new PrinterOutputMonitor();

        public event FileWatcherEvent FileCompleted
        {
            add { _monitor.FilePrintingFinished += value; }
            remove { _monitor.FilePrintingFinished -= value; }
        }

        public event FileWatcherEvent FileStarted
        {
            add { _monitor.FilePrintingStarted += value; }
            remove { _monitor.FilePrintingStarted -= value; }
        }

        public void Start()
        {
            _monitor.Start("c:\\SmartPrinter\\Temp\\");
        }

        public void Stop()
        {
        }
    }
}