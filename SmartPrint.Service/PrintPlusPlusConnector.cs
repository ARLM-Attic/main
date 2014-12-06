using SmartPrint.Model;
using SmartPrint.Wpf;

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
            var printForm = new PrintForm();
            printForm.DataContext = new PrintFormVM();
            printForm.Show();
        }

        public void Stop()
        {
        }
    }
}