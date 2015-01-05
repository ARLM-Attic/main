namespace SmartPrint.DriverSetupAction
{
    public class PrinterDriverSettings
    {
        public PrinterDriverSettings(string driverName, string driverFilename, string configFilename, string dataFilename, string helpFilename, string driverDir)
        {
            Name = driverName;
            DriverFileName = driverFilename;
            ConfigFilename = configFilename;
            DataFilename = dataFilename;
            HelpFilename = helpFilename;
            PrinterDriverDirectory = driverDir;
        }

        public string Name { get; set; }
        
        public string DriverFileName { get; set; }
        
        public string ConfigFilename { get; set; }
        
        public string DataFilename { get; set; }
        
        public string HelpFilename { get; set; }
        
        public string PrinterDriverDirectory { get; set; }
        
        public string DriverFilePath
        {
            get { return GetPath(DriverFileName); }
        }
        
        public string DataFilePath
        {
            get { return GetPath(DataFilename); }
        }
        
        public string ConfigFilePath
        {
            get { return GetPath(ConfigFilename); }
        }
        
        public string HelpFilePath
        {
            get { return GetPath(HelpFilename); }
        }
        private string GetPath(string filename)
        {
            return (string.IsNullOrEmpty(PrinterDriverDirectory) ? "" : (PrinterDriverDirectory + "\\")) + filename;
        }
    }
}