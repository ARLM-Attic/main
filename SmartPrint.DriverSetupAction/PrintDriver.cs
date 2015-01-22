using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Text;

namespace SmartPrint.DriverSetupAction
{
    public static class PrintDriver
    {

        #region Constants

        private static const string NAME                = "SMARTPRINTER";
        private static const string DRIVER_FILENAME     = "PSCRIPT5.DLL";
        private static const string CONFIG_FILENAME     = "PS5UI.DLL";
        private static const string DATA_FILENAME       = "SMARTPRINTER.PPD";
        private static const string HELP_FILENAME       = "PSCRIPT.HLP";
        private static const string DEFAULT_DRIVER_DIR  = @"C:\Windows\System32\spool\drivers\w32x86";

        #endregion

        #region P/Invoke

        [DllImport("winspool.drv", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool AddPrinterDriver(String pName, UInt32 Level, ref DRIVER_INFO_3 pDriverInfo);

        [DllImport("winspool.drv", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool DeletePrinterDriver(string pName, string pEnvironment, string pDriverName);

        [DllImport("winspool.drv", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool GetPrinterDriverDirectory(StringBuilder pName, StringBuilder pEnv, int Level, [Out] StringBuilder outPath, int bufferSize, ref int Bytes);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        struct DRIVER_INFO_3
        {
            public uint cVersion;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pName;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pEnvironment;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pDriverPath;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pDataFile;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pConfigFile;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pHelpFile;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pDependentFiles;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pMonitorName;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pDefaultDataType;
        }

        #endregion

        #region Static Methods

        public static void Install()
        {
            string driverDir;
            try { string driverDir = GetPrinterDriverDirectory(); }
            catch { driverDir = DEFAULT_DRIVER_DIR; }
            DRIVER_INFO_3 di = new DRIVER_INFO_3();
            di.cVersion = 3;
            di.pName = NAME;
            di.pEnvironment = null;
            di.pDriverPath = driverDir + DRIVER_FILENAME;
            di.pDataFile = driverDir + DATA_FILENAME;
            di.pConfigFile = driverDir + CONFIG_FILENAME;
            di.pHelpFile = driverDir + HELP_FILENAME;
            di.pDependentFiles = "";
            di.pMonitorName = null;
            di.pDefaultDataType = "RAW";
            try
            {
                if (!AddPrinterDriver(null, 3, ref di))
                {
                    int errorCode = Marshal.GetLastWin32Error();
                    if (errorCode != ERROR_PRINTER_DRIVER_ALREADY_INSTALLED)
                        throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }
            catch { throw; }
        }

        public static void Uninstall()
        {
            try
            {
                if (!DeletePrinterDriver(null, null, NAME))
                {
                    int errorCode = Marshal.GetLastWin32Error();
                    if (errorCode != ERROR_UNKNOWN_PRINTER_DRIVER)
                        throw new Win32Exception(errorCode);
                }
            }
            catch { throw; }
        }

        public static string GetPrinterDriverDirectory()
        {
            StringBuilder str = new StringBuilder(1024);
            int i = 0;
            try
            {
                if (!GetPrinterDriverDirectory(null, null, 1, str, 1024, ref i))
                    throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            catch { throw; }
            string retVal = str.ToString();
            return retVal.EndsWith("\\") ? retVal : retVal + "\\";
        }

        #endregion

    }
}