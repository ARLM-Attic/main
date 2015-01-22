using System;
using System.Collections.ObjectModel;
using System.Linq;
using SmartPrint.Model;

namespace SmartPrinter.UI.ViewModels
{
    public class PrinterVM : BaseVM
    {
        private readonly Printer _printer;

        private ReadOnlyObservableCollection<ActionVM> _actions;
        private readonly ObservableCollection<ActionVM> _actionsInternal;

        public PrinterVM(Printer printer)
        {
            _printer = printer;
            _actionsInternal = new ObservableCollection<ActionVM>(_printer.Actions.Select(a => new ActionVM(a)));
            _actions = new ReadOnlyObservableCollection<ActionVM>(_actionsInternal);
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


        public Printer Printer { get { return _printer; } }

        public ReadOnlyObservableCollection<ActionVM> Actions
        {
            get { return _actions; }
        }

        public void NewAction()
        {
            var action = new PhysicalPrinterAction();
            action.Name = GetActionName();
            
            _printer.Actions.Add(action);

            _actionsInternal.Add(new ActionVM(action));
        }

        private string GetActionName()
        {
            var name = "new action";
            var i = 1;

            while (Printer.Actions.Any(a => a.Name == name))
            {
                name = String.Format("new action {0}", i);
                i++;
            }

            return name;
        }
    }
}