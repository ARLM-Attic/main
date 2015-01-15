using System;
using SmartPrint.Model;

namespace SmartPrinter.Model.ViewModels
{
    public class PrinterVM : BaseVM
    {
        private readonly Printer _printer;

        public PrinterVM(Printer printer)
        {
            _printer = printer;
        }

        public Guid Id { get { return _printer.Id; } }

        public string Name
        {
            get { return _printer.Name; }
            set
            {
                if (_printer.Name == value) return;
                _printer.Name = value;
                OnPropertyChanged(() => Name);
            }
        }

        public string Description
        {
            get { return _printer.Description; }
            set
            {
                if (_printer.Description == value) return;
                _printer.Description = value;
                OnPropertyChanged(() => Description);
            }
        }
    }
}