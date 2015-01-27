using System;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace SmartPrint.DriverSetupAction
{
    public static class PrintMonitor
    {
        #region Constants

        private const int ERROR_UNKNOWN_PRINT_MONITOR = 3000;
        private const int ERROR_PRINT_MONITOR_ALREADY_INSTALLED = 3006;

        public const string DEFAULT_MONITOR_NAME = "SMARTPRINTER";
        public const string DEFAULT_MONITOR_DLL = "mfilemon.dll";
        public const string DEFAULT_REGISTRY_KEY = @"SYSTEM\CurrentControlSet\Control\Print\Monitors\SMARTPRINTER";

        #endregion

        #region P/Invoke

        [DllImport("winspool.drv", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool AddMonitor(String pName, UInt32 Level, ref MONITOR_INFO_2 pMonitors);

        [DllImport("winspool.drv", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool DeleteMonitor(string pName, string pEnvironment, string pMonitorName);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        struct MONITOR_INFO_2
        {
            public string pName;
            public string pEnvironment;
            public string pDLLName;
        }

        #endregion

        #region Static Methods

        public static void Install()
        {
            MONITOR_INFO_2 mi2 = new MONITOR_INFO_2
            {
                pName = DEFAULT_MONITOR_NAME,
                pEnvironment = null,
                pDLLName = DEFAULT_MONITOR_DLL
            };
            try
            {
                if (!AddMonitor(null, 2, ref mi2))
                {
                    int code = Marshal.GetLastWin32Error();
                    if (code != ERROR_PRINT_MONITOR_ALREADY_INSTALLED) 
                    { throw new Win32Exception(code); }
                }
            }
            catch { throw; }
        }

        public static void Uninstall()
        {
            try
            {
                if (!DeleteMonitor(null, null, DEFAULT_MONITOR_NAME))
                {
                    int errorCode = Marshal.GetLastWin32Error();
                    if (errorCode != ERROR_UNKNOWN_PRINT_MONITOR)
                        throw new Win32Exception(errorCode);
                }
            }
            catch { throw; }
        }

        #endregion
    }
}
