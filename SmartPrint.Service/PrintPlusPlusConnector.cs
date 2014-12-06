namespace SmartPrint.Service
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
            var printForm =  new SmartPrint.Wpf.PrintForm();
          
        }

        public void Stop()
        {
        }
    }
}