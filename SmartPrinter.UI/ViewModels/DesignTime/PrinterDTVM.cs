using System.Linq;

using SmartPrint.Model.Repository;
using SmartPrint.Model.ViewModels;

namespace SmartPrinter.UI.ViewModels
{
    public class PrinterDTVM : PrinterVM
    {
        public PrinterDTVM()
            : base(new ShellDTVM().Printers.First().Printer)
        {
        }
    }
}