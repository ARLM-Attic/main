using System;
using System.Linq;
using SmartPrint.Model;
using SmartPrinter.Model.ViewModels;

namespace SmartPrinter.UI.Commands
{
    public class AddPrinterCommand : BaseCommand
    {
        public override void Execute(object parameter)
        {
            var vm = new PrinterVM(new Printer { Name = GetPrinterName(), Description = "SMARTdoc printer" });
            Shell.Printers.Add(vm);
            Shell.SelectedPrinter = vm;

            // DriverInstaller.AddVSmartPrinter(vm.Name, vm.Description);
        }

        public string GetPrinterName()
        {
            var name = "new printer";
            var i = 1;

            while (Shell.Printers.Any(a => a.Name == name))
            {
                name = String.Format("new printer {0}", i);
                i++;
            }

            return name;
        }
    }
}