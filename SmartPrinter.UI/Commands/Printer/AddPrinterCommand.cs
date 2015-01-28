using System;
using System.Linq;
using SmartPrint;
using SmartPrint.Model;
using SmartPrinter.UI.ViewModels;
using SmartPrint.DriverSetupAction;

namespace SmartPrinter.UI.Commands
{
    public class AddPrinterCommand : BaseCommand
    {
        const string DefaultPrinterDescription = "SMARTdoc printer";

        public override void Execute(object parameter)
        {
            var name = GetPrinterName();

            var device = SmartPrintDevice.Install(name, DefaultPrinterDescription);

            var printerId = RegistryExtensions.GetPrinterId(name);

            var printer = new Printer
            {
                Id = printerId,
                Name = name,
                Description = DefaultPrinterDescription
            };

            printer.OutputPath = RegistryExtensions.GetPrinterOutputPath(printerId);

            printer.StartMonitoring();

            var vm = new PrinterVM(printer);

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