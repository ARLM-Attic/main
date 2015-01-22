using System.Windows;
using SmartPrinter.UI.ViewModels;

namespace SmartPrinter.UI.Commands
{
    public class SelectPrinterCommand : BaseCommand
    {
        public override void Execute(object parameter)
        {
            Shell.SelectedPrinter = Printer;
        }

        public static readonly DependencyProperty PrinterProperty =
                   DependencyProperty.Register("Printer", typeof(PrinterVM), typeof(SelectPrinterCommand), new PropertyMetadata(default(PrinterVM)));

        public PrinterVM Printer
        {
            get { return (PrinterVM)GetValue(PrinterProperty); }
            set { SetValue(PrinterProperty, value); }
        }
    }
}