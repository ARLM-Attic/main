using System;
using SmartPrint.DriverSetupAction;

namespace SmartPrint.DriverSetupAction
{
    public class PrintDriverSettings
    {
        public PrintDriverSettings()
        {
            Name = "SMARTPRINTER";
            DriverFileName = "PSCRIPT5.DLL";
            ConfigFilename = "PS5UI.DLL";
            DataFilename = "SMARTPRINTER.PPD";
            HelpFilename = "PSCRIPT.HLP";
        }
        public PrintDriverSettings(string driverName, string driverFilename, string configFilename, string dataFilename, string helpFilename)
        {
            Name = driverName;
            DriverFileName = driverFilename;
            ConfigFilename = configFilename;
            DataFilename = dataFilename;
            HelpFilename = helpFilename;
        }

        public string Name { get; set; }
        
        public string DriverFileName { get; set; }
        
        public string ConfigFilename { get; set; }
        
        public string DataFilename { get; set; }
        
        public string HelpFilename { get; set; }

        private string printerDriverDir;
        public string PrinterDriverDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(printerDriverDir))
                    try { printerDriverDir = DriverInstaller.GetPrinterDriverDirectory(); }
                    catch { }
                return printerDriverDir;
            }

        }
        public string DriverFilePath
        {
            get { return PrinterDriverDirectory + DriverFileName; }
        }
        
        public string DataFilePath
        {
            get { return PrinterDriverDirectory + DataFilename; }
        }
        
        public string ConfigFilePath
        {
            get { return PrinterDriverDirectory + ConfigFilename; }
        }
        
        public string HelpFilePath
        {
            get { return PrinterDriverDirectory + HelpFilename; }
        }

    }
}