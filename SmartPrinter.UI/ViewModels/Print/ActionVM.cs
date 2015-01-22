using SmartPrint.Model;

namespace SmartPrinter.Model.ViewModels
{
    public class ActionVM : BaseVM
    {
        private readonly PrinterAction _action;

        public ActionVM(PrinterAction action)
        {
            _action = action;
        }

        public string Name
        {
            get { return _action.Name; }
            set
            {
                if (_action.Name == value) return;
                _action.Name = value;
                OnPropertyChanged(() => Name);
            }
        }
    }
}