using System.Collections.ObjectModel;
using System.Linq;
using SmartPrint.Model.Repository;
using SmartPrinter.Model.ViewModels;

namespace SmartPrint.Model.ViewModels
{
    public class ShellVM : BaseVM
    {
        #region Private fields

        private readonly IRepository _repository;

        private ObservableCollection<PrinterVM> _printers = new ObservableCollection<PrinterVM>();
        private PrinterVM _selectedPrinter;

        #endregion
        
        public ShellVM(IRepository repository)
        {
            _repository = repository;
        }

        #region Properties

        public IRepository Repository { get { return _repository; } }

        public ObservableCollection<PrinterVM> Printers { get { return _printers; } }

        public PrinterVM SelectedPrinter
        {
            get { return _selectedPrinter; }
            set
            {
                if (_selectedPrinter == value) return;
                _selectedPrinter = value;
                OnPropertyChanged(() => SelectedPrinter);
            }
        }

        #endregion

        #region Public Methods

        public void Initialize()
        {
            LoadPrinters();
            
            Toaster.ToastInfo("SMARTdoc Share Monitor is running...");
        }

        #endregion

        private void LoadPrinters()
        {
            var printers = _repository.LoadPrinters();

            // Start VPrinters
            // TODO: location should be in Program Files\SMARTdoc\PrinterConnector\Temp
            // _shellVM.StartMonitoring("c:\\SmartPrinter\\Temp\\");
            printers.ForEach(a => a.StartMonitoring("c:\\SmartPrinter\\Temp\\"));

            _printers = new ObservableCollection<PrinterVM>(printers.Select(p => new PrinterVM(p)));
            OnPropertyChanged(() => Printers);

        }
    }
}