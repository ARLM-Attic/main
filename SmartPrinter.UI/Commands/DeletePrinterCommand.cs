using System.Linq;
using System.Windows;
using SmartPrinter.Model.ViewModels;

namespace SmartPrinter.UI.Commands
{
    public class DeletePrinterCommand : BaseCommand
    {
        public override void Execute(object parameter)
        {
            Shell.Printers.Remove(Printer);

            Shell.Repository.DeletePrinter(Printer.Id);

            Shell.SelectedPrinter = Shell.Printers.FirstOrDefault();
        }

        public static readonly DependencyProperty PrinterProperty =
           DependencyProperty.Register("Printer", typeof(PrinterVM), typeof(DeletePrinterCommand), new PropertyMetadata(default(PrinterVM)));

        public PrinterVM Printer
        {
            get { return (PrinterVM)GetValue(PrinterProperty); }
            set { SetValue(PrinterProperty, value); }
        }
    }
}