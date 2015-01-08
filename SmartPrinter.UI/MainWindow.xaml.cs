using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using SmartPrint.Model;
using SmartPrint.UI;

namespace SmartPrinter.UI
{
    public partial class MainWindow
    {
        private readonly FolderMonitor _monitor = new FolderMonitor();
        private readonly PrintFormVM _vm = new PrintFormVM();
        private readonly Dispatcher _dispatcher = Application.Current.Dispatcher;

        public MainWindow()
        {
            InitializeComponent();
            _monitor.FilePrintingStarted += OnMonitorOnFilePrintingStarted;
            _monitor.FilePrintingFinished += OnMonitorOnFilePrintingFinished;
            _monitor.Start("c:\\SmartPrinter\\Temp\\");

#if DEBUG
            Visibility = Visibility.Visible;
#endif
        }

        private void OnMonitorOnFilePrintingStarted(string filePath)
        {
            if (_dispatcher.CheckAccess())
                ShowForm(filePath);
            else
                _dispatcher.BeginInvoke((Action)(() => ShowForm(filePath)));
        }

        private void OnMonitorOnFilePrintingFinished(string filePath)
        {
            if (_dispatcher.CheckAccess())
                _vm.PostScriptCreated = true;
            else
                _dispatcher.BeginInvoke((Action)(() => _vm.PostScriptCreated = true));
        }

        private void ShowForm(string filePath)
        {
            var printForm = new PrintForm();
         
            printForm.DataContext = _vm;
            _vm.PostScriptFilePath = filePath;
            printForm.Show();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
