using System;
using System.Windows;
using System.Windows.Input;
using SmartPrint.Model;
using SmartPrint.UI;

namespace SmartPrinter.UI
{
    public partial class MainWindow
    {
        private readonly PrinterOutputMonitor _monitor = new PrinterOutputMonitor();

        public MainWindow()
        {
            InitializeComponent();
            _monitor.FilePrinted += OnMonitorOnFilePrinted;
            _monitor.Start("c:\\SmartPrinter\\Temp\\");

#if DEBUG
            Visibility = Visibility.Visible;
#endif
        }

        private void OnMonitorOnFilePrinted(string filePath)
        {
            var d = Application.Current.Dispatcher;

            if (d.CheckAccess())
                ShowForm(filePath);
            else
                d.BeginInvoke((Action)(() => ShowForm(filePath)));
        }

        private void ShowForm(string filePath)
        {
            var printForm = new PrintForm();
            var vm = new PrintFormVM();

            printForm.DataContext = vm;

            vm.PostScriptFilePath = filePath;
            
            printForm.Show();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
