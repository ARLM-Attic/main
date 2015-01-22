using Microsoft.Win32;

namespace SmartPrint.DriverSetupAction
{
    public class PrintDeviceSettings
    {
        private string _name;
        private string _appPath = @"C:\Program Files\SMARTdoc\";
        private string _monitorName = "SMARTdoc";
        private string _monitorDllName = "mfilemon.dll";
        private string _portName = "SMARTdoc:\0";
        private string _description = "SMARTdoc printer";
        private PrintDriverSettings _driver = new PrintDriverSettings();

        public PrintDeviceSettings(
            string name, 
            string description = "",
            string appPath = "",
            string portName = "",
            string monitorName = "",
            string monitorDllName = "",
            PrintDriverSettings driver = null)
        {
            Name = name;
            Description = description;
            AppPath = appPath;
            PortName = portName;
            MonitorName = monitorName;
            MonitorDllName = monitorDllName;
            Driver = driver;
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string AppPath
        {
            get { return _appPath; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    try
                    {
                        string appPath = (string)Registry.LocalMachine.GetValue(@"SOFTWARE\SMARTdoc\SMARTdoc Share\appPath");
                        if (!string.IsNullOrEmpty(appPath)) _appPath = appPath;
                    }
                    catch { }
                    _appPath = value.EndsWith("\\") ? value : value + "\\";
                }
            }
        }

        public string MonitorName
        { 
            get { return _monitorName; }
            set { if (!string.IsNullOrEmpty(value)) _monitorName = value; } 
        }

        public string MonitorDllName
        {
            get { return _monitorDllName; }
            set { if (!string.IsNullOrEmpty(value)) _monitorDllName = value; }
        }

        public PrintDriverSettings Driver
        {
            get { return _driver; }
            set
            {
                if (value != null) _driver = value;
            }
        }

        public string PortName
        {
            get { return _portName; }
            set
            {
                if (string.IsNullOrEmpty(value)) _portName = _name + ":\0";
                _portName = value.EndsWith("\0") ? value : value + "\0";
            }
        }

        public string Description
        {
            get { return _description; }
            set { if (!string.IsNullOrEmpty(value)) _description = value; }
        }
    }
}