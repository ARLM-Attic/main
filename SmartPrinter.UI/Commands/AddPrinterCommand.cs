using SmartPrint.Model;
using SmartPrinter.Model.ViewModels;

namespace SmartPrinter.UI.Commands
{
    public class AddPrinterCommand : BaseCommand
    {
        public override void Execute(object parameter)
        {
            var vm = new PrinterVM(new Printer() {Name = "new printer"});
            Shell.Printers.Add(vm);
            Shell.SelectedPrinter = vm;
        }
    }
}