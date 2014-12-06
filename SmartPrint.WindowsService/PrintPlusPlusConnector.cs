namespace SmartPrint.WindowsService
{
    public class PrintPlusPlusConnector
    {
        private PrinterOutputMonitor _monitor;

        public void Start()
        {
            _monitor = new PrinterOutputMonitor();
            _monitor.FilePrinted += OnFilePrinted;
        }

        private void OnFilePrinted(string path)
        {
            
        }

        public void Stop()
        {
        }
    }
}