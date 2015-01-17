namespace SmartPrint.DriverSetupAction
{
    public class PrinterSettings
    {
        private string _portName;

        public PrinterSettings(string printerName)
        {
           PrinterName = printerName;
           AppPath = @"C:\Program Files\SMARTdoc\PrintConnector";
           MonitorName = "SMARTPRINTER";
           MonitorDllName = "mfilemon.dll";
           PortName = printerName + ":";
           Description = MonitorName;
           Drivers = new PrinterDriverSettings();
        }

        public PrinterSettings(string printerName, string description)
        {
            PrinterName = printerName;
            AppPath = @"C:\Program Files\SMARTdoc\PrintConnector";
            MonitorName = "SMARTPRINTER";
            MonitorDllName = "mfilemon.dll";
            PortName = printerName + ":";
            Description = description;
            Drivers = new PrinterDriverSettings();

        }
        public PrinterSettings(string printerName, string appPath, string monitorName, string monitorDllName, string portName, string description, PrinterDriverSettings driverFiles)
        {
           PrinterName = printerName;
           AppPath = appPath;
           MonitorName = monitorName;
           MonitorDllName = monitorDllName;
           PortName = portName;
           Description = description;
           Drivers = driverFiles;
        }

        public string PrinterName { get; set; }
        
        public string AppPath { get; set; }
        
        public string MonitorName { get; set; }
        
        public string MonitorDllName { get; set; }
        
        public PrinterDriverSettings Drivers { get; set; }

        public string Description { get; set; }

        public string PortName
        {
            get { return _portName; }
            set { _portName = value + (value.EndsWith("\0") ? "" : "\0"); }
        }
    }
}