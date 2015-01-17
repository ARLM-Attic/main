using System.Windows;
using SmartPrinter.Model.ViewModels;

namespace SmartPrinter.UI.Commands
{
    public class AddActionCommand : BaseCommand
    {
        public override void Execute(object parameter)
        {
            PrinterVM.NewAction();
        }

        public static readonly DependencyProperty PrinterVMProperty =
            DependencyProperty.Register("PrinterVM", typeof (PrinterVM), typeof (AddActionCommand), new PropertyMetadata(default(PrinterVM)));

        public PrinterVM PrinterVM
        {
            get { return (PrinterVM) GetValue(PrinterVMProperty); }
            set { SetValue(PrinterVMProperty, value); }
        }
    }
}