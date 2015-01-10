namespace SmartPrint.DriverSetupAction
{
    public class PrinterSettings
    {
        private string _portName;

        public PrinterSettings(string printerName, string appPath, string monitorName, string monitorDllName, string portName, PrinterDriverSettings driverFiles)
        {
           PrinterName = printerName;
           AppPath = appPath;
           MonitorName = monitorName;
           MonitorDllName = monitorDllName;
           PortName = portName;
           Drivers = driverFiles;
        }

        public string PrinterName { get; set; }
        
        public string AppPath { get; set; }
        
        public string MonitorName { get; set; }
        
        public string MonitorDllName { get; set; }
        
        public PrinterDriverSettings Drivers { get; set; }

        public string MyProperty { get; set; }

        public string PortName
        {
            get { return _portName; }
            set { _portName = value + (value.EndsWith("\0") ? "" : "\0"); }
        }
    }
}