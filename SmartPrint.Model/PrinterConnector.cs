using SmartPrint.Model.Helpers;

namespace SmartPrint.Model
{
    public class PrinterConnector
    {
        private PrinterOutputMonitor _monitor = new PrinterOutputMonitor();

        public event FilePrinted FileCompleted
        {
            add { _monitor.FilePrinted += value; }
            remove { _monitor.FilePrinted -= value; }
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