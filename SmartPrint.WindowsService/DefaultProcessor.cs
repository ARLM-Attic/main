using PrinterPlusPlusSDK;
using SmartPrint.Wpf;

namespace SmartPrint.WindowsService
{
    public class DefaultProcessor : IProcessor
    {
        public ProcessResult Process(string key, string psFilename)
        {
            new PrintForm().Show();
            return new ProcessResult();
        }
    }
}