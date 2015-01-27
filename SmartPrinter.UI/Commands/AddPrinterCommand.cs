using System;
using System.Linq;
using SmartPrint.Model;
using SmartPrinter.UI.ViewModels;
using SmartPrint.DriverSetupAction;

namespace SmartPrinter.UI.Commands
{
    public class AddPrinterCommand : BaseCommand
    {
        public override void Execute(object parameter)
        {
            var printer = new Printer
                          {
                              Id = Guid.NewGuid(),
                              Name = GetPrinterName(), 
                              Description = "SMARTdoc printer"
                          };

            var vm = new PrinterVM(printer);

            var device = SmartPrintDevice.Install(vm.Name, vm.Description);
         
            Shell.Printers.Add(vm);
           
            Shell.SelectedPrinter = vm;

            Shell.Repository.SavePrinter(printer);

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