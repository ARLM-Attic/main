using System;
using System.Runtime.InteropServices;
using System.Text;
using System.ComponentModel;
using System.ServiceProcess;
using Microsoft.Win32;
using System.Windows.Forms;

namespace SmartPrint.DriverSetupAction
{
    public class DriverInstaller
    {
        #region PInvoke Codes

        #region Printer Monitor

        [DllImport("winspool.drv", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern Int32 AddMonitor(String pName, UInt32 Level, ref MONITOR_INFO_2 pMonitors);

        [DllImport("winspool.drv", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int DeleteMonitor(string pName, string pEnvironment, string pMonitorName);
        
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct MONITOR_INFO_2
        {
            public string pName;
            public string pEnvironment;
            public string pDLLName;
        }

        #endregion
        
        #region Printer Port
        private const int MAX_PORTNAME_LEN = 64;
        private const int MAX_NETWORKNAME_LEN = 49;
        private const int MAX_SNMP_COMMUNITY_STR_LEN = 33;
        private const int MAX_QUEUENAME_LEN = 33;
        private const int MAX_IPADDR_STR_LEN = 16;
        private const int RESERVED_BYTE_ARRAY_SIZE = 540;

        private enum PrinterAccess
        {
            ServerAdmin = 0x01,
            ServerEnum = 0x02,
            PrinterAdmin = 0x04,
            PrinterUse = 0x08,
            JobAdmin = 0x10,
            JobRead = 0x20,
            StandardRightsRequired = 0x000f0000,
            PrinterAllAccess = (StandardRightsRequired | PrinterAdmin | PrinterUse)
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct PrinterDefaults
        {
            public IntPtr pDataType;
            public IntPtr pDevMode;
            public PrinterAccess DesiredAccess;
        }
      
        [DllImport("winspool.drv", SetLastError = true)]
        private static extern bool OpenPrinter(string pPrinterName, out IntPtr phPrinter, ref PrinterDefaults pDefault);
        
        [DllImport("winspool.drv", SetLastError = true)]
        private static extern bool ClosePrinter(IntPtr phPrinter);
        
        [DllImport("winspool.drv", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool XcvDataW(IntPtr hXcv, string pszDataName, IntPtr pInputData, UInt32 cbInputData, IntPtr pOutputData, UInt32 cbOutputData, out UInt32 pcbOutputNeeded, out UInt32 pdwStatus);

        #endregion
        
        #region Printer Driver
      
        [DllImport("winspool.drv", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern Int32 AddPrinterDriver(String pName, UInt32 Level, ref DRIVER_INFO_3 pDriverInfo);
      
        [DllImport("winspool.drv", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int DeletePrinterDriver(string pName, string pEnvironment, string pDriverName);
        
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct DRIVER_INFO_3
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
       
        [DllImport("winspool.drv", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern bool GetPrinterDriverDirectory(StringBuilder pName, StringBuilder pEnv, int Level, [Out] StringBuilder outPath, int bufferSize, ref int Bytes);
        #endregion
        
        #region Printer
     
        [DllImport("winspool.drv", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern Int32 AddPrinter(string pName, uint Level, [In] ref PRINTER_INFO_2 pPrinter);
     
        [DllImport("winspool.drv", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool DeletePrinter(IntPtr hPrinter);
        
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct PRINTER_INFO_2
        {
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pServerName;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pPrinterName;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pShareName;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pPortName;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pDriverName;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pComment;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pLocation;
            public IntPtr pDevMode;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pSepFile;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pPrintProcessor;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pDatatype;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pParameters;
            public IntPtr pSecurityDescriptor;
            public uint Attributes;
            public uint Priority;
            public uint DefaultPriority;
            public uint StartTime;
            public uint UntilTime;
            public uint Status;
            public uint cJobs;
            public uint AveragePPM;
        }
        #endregion
        
        #endregion

        public bool AddPrinterMonitor(string monitorName, string monitorDllName)
        {
            MONITOR_INFO_2 mi2 = new MONITOR_INFO_2
            {
                pName = monitorName,
                pEnvironment = null,
                pDLLName = monitorDllName
            };
            try
            {
                if (AddMonitor(null, 2, ref mi2) == 0)
                {
                    int code = Marshal.GetLastWin32Error();
                    if (code != 3006) { throw new Win32Exception(code); }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("AddPrinterMonitor exception:\n" + ex.Message);
            }

        }

        public bool DeletePrinterMonitor(string monitorName)
        {
            try
            {
                if (DeleteMonitor(null, null, monitorName) == 0)
                {
                    int errorCode = Marshal.GetLastWin32Error();
                    Console.WriteLine(errorCode);
                    if (errorCode == 3000) return false; // printer monitor is unknown
                    throw new Win32Exception(errorCode);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("DeletePrinterMonitor exception:\n" + ex.Message);
            }
        }

        public bool AddPrinterPort(string portName, string portType)
        {
            IntPtr printerHandle;
            PrinterDefaults defaults = new PrinterDefaults { DesiredAccess = PrinterAccess.ServerAdmin };
            bool success = false;
            try
            {
                if (!OpenPrinter(",XcvMonitor " + portType, out printerHandle, ref defaults))
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                try
                {
                    if (!portName.EndsWith("\0")) portName += "\0";
                    uint size = (uint)(portName.Length * 2);
                    IntPtr pointer = Marshal.AllocHGlobal((int)size);
                    Marshal.Copy(portName.ToCharArray(), 0, pointer, portName.Length);
                    IntPtr pOutputData = IntPtr.Zero;
                    UInt32 outputNeeded, status;
                    try
                    {
                        if (!XcvDataW(printerHandle, "AddPort", pointer, size, pOutputData, 0, out outputNeeded, out status))
                            throw new Win32Exception((int)status);
                        Console.WriteLine((new Win32Exception((int)status)).Message);
                        success = true;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("AddPrinterPort(XcvData) exception:\n" + ex.Message);
                    }
                    finally
                    {
                        Marshal.FreeHGlobal(pointer);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("AddPrinterPort(OpenPrinter) exception:\n" + ex.Message);
                }
                finally
                {
                    ClosePrinter(printerHandle);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AddPrinterPort exception:\n" + ex.Message);
            }
            return success;
        }

        public bool DeletePrinterPort(string portName, string portType)
        {
            IntPtr printerHandle;
            PrinterDefaults defaults = new PrinterDefaults { DesiredAccess = PrinterAccess.ServerAdmin };
            bool success = false;
            try
            {
                if (!OpenPrinter(",XcvMonitor " + portType, out printerHandle, ref defaults))
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                try
                {
                    if (!portName.EndsWith("\0")) portName += "\0";
                    uint size = (uint)(portName.Length * 2);
                    IntPtr pointer = Marshal.AllocHGlobal((int)size);
                    Marshal.Copy(portName.ToCharArray(), 0, pointer, portName.Length);
                    IntPtr pOutputData = IntPtr.Zero;
                    UInt32 outputNeeded, status;
                    try
                    {
                        if (!XcvDataW(printerHandle, "DeletePort", pointer, size, pOutputData, 0, out outputNeeded, out status))
                            throw new Win32Exception((int)status);
                        Console.WriteLine((new Win32Exception((int)status)).Message);
                        success = true;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("DeletePrinterPort(XcvData) exception:\n" + ex.Message);
                    }
                    finally
                    {
                        Marshal.FreeHGlobal(pointer);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("DeletePrinterPort(OpenPrinter) exception:\n" + ex.Message);
                }
                finally
                {
                    ClosePrinter(printerHandle);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("DeletePrinterPort exception:\n" + ex.Message);
            }
            return success;

        }

        public string GetPrinterDirectory()
        {
            StringBuilder str = new StringBuilder(1024);
            int i = 0;
            try
            {
                if (!GetPrinterDriverDirectory(null, null, 1, str, 1024, ref i))
                    throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            catch (Exception ex)
            {
                throw new Exception("GetPrinterDirectory exception:\n" + ex.Message);
            }
            return str.ToString();
        }

        public bool AddPrinterDriver(PrinterDriverSettings driver)
        {
            DRIVER_INFO_3 di = new DRIVER_INFO_3();
            di.cVersion = 3;
            di.pName = driver.Name;
            di.pEnvironment = null;
            di.pDriverPath = driver.DriverFilePath;
            di.pDataFile = driver.DataFilePath;
            di.pConfigFile = driver.ConfigFilePath;
            di.pHelpFile = driver.HelpFilePath;
            di.pDependentFiles = "";
            di.pMonitorName = null;
            di.pDefaultDataType = "RAW";

            try
            {
                if (AddPrinterDriver(null, 3, ref di) == 0)
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("AddPrinterDriver exception:\n" + ex.Message);
            }
        }

        public bool DeletePrinterDriver(string driverName)
        {
            try
            {
                if (DeletePrinterDriver(null, null, driverName) == 0)
                {
                    int errorCode = Marshal.GetLastWin32Error();
                    if (errorCode == 1797) return false; // driver name is unknown
                    throw new Win32Exception(errorCode);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("DeletePrinterDriver exception:\n" + ex.Message);
            }
        }

        public bool AddPrinter(string printerName, string monitorName, string portName, string driverName)
        {
            if (!portName.EndsWith("\0")) portName += "\0";
            PRINTER_INFO_2 pi = new PRINTER_INFO_2
            {
                pServerName = null,
                pPrinterName = printerName,
                pShareName = "",
                pPortName = portName,
                pDriverName = driverName,
                pComment = printerName,
                pLocation = "",
                pDevMode = new IntPtr(0),
                pSepFile = "",
                pPrintProcessor = "WinPrint",
                pDatatype = "RAW",
                pParameters = "",
                pSecurityDescriptor = new IntPtr(0)
            };
            try
            {
                if (AddPrinter(null, 2, ref pi) == 0)
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("AddPrinter exception:\n" + ex.Message);
            }
        }

        public bool DeletePrinter(string printerName)
        {
            if (!printerName.EndsWith("\0")) printerName += "\0";
            IntPtr printerHandle = IntPtr.Zero;
            PrinterDefaults defaults = new PrinterDefaults { DesiredAccess = PrinterAccess.PrinterAllAccess };
            try
            {
                if (!OpenPrinter(printerName, out printerHandle, ref defaults))
                {
                    int errorCode = Marshal.GetLastWin32Error();
                    if (errorCode == 1801) return false; // There is no printer by that name
                    throw new Win32Exception(errorCode);
                }
                if (!DeletePrinter(printerHandle))
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("DeletePrinter exception: " + ex.Message);
            }
            finally
            {
                ClosePrinter(printerHandle);
            }
        }

        public bool ConfigureVirtualPort(string appPath, string monitorName, string portName)
        {
            try
            {
                if (!portName.EndsWith("\0")) portName += "\0";
                string outputPath = string.Format(@"{0}\Temp", appPath);
                string filePattern = "%r_%c_%u_%Y%m%d_%H%n%s_%j.ps";
                string userCommand = string.Empty;
                var execPath = string.Empty;

                string keyName = string.Format(@"SYSTEM\CurrentControlSet\Control\Print\Monitors\{0}\{1}", monitorName, portName);
                Registry.LocalMachine.CreateSubKey(keyName);
                RegistryKey regKey = Registry.LocalMachine.OpenSubKey(keyName, true);
                regKey.SetValue("OutputPath", outputPath, RegistryValueKind.String);
                regKey.SetValue("FilePattern", filePattern, RegistryValueKind.String);
                regKey.SetValue("Overwrite", 0, RegistryValueKind.DWord);
                regKey.SetValue("UserCommand", userCommand, RegistryValueKind.String);
                regKey.SetValue("ExecPath", execPath, RegistryValueKind.String);
                regKey.SetValue("WaitTermination", 0, RegistryValueKind.DWord);
                regKey.SetValue("PipeData", 0, RegistryValueKind.DWord);
                regKey.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("ConfigureVrtualPort exception:\n" + ex.Message);
            }
        }

        public bool DeleteVirtualPortConfiguration(string monitorName, string portName)
        {
            if (!portName.EndsWith("\0")) portName += "\0";
            try
            {
                string keyName = string.Format(@"SYSTEM\CurrentControlSet\Control\Print\Monitors\{0}\{1}", monitorName, portName);
                if (null == Registry.LocalMachine.OpenSubKey(keyName)) return false;
                Registry.LocalMachine.DeleteSubKeyTree(keyName);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("DeleteVirtualPortConfiguration exception:\n" + ex.Message);
            }
        }

        public bool RestartSpoolService()
        {
            try
            {
                ServiceController sc = new ServiceController("Spooler");
                if (sc.Status != ServiceControllerStatus.Stopped || sc.Status != ServiceControllerStatus.StopPending)
                    sc.Stop();
                sc.WaitForStatus(ServiceControllerStatus.Stopped);
                sc.Start();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("RestartSpoolService exception:\n" + ex.Message);
            }
        }

        public bool StopSpoolService()
        {
            try
            {
                ServiceController sc = new ServiceController("Spooler");
                if (sc.Status != ServiceControllerStatus.Stopped || sc.Status != ServiceControllerStatus.StopPending)
                    sc.Stop();
                sc.WaitForStatus(ServiceControllerStatus.Stopped);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("RestartSpoolService exception:\n" + ex.Message);
            }
        }

        public bool AddVPrinter(PrinterSettings printer)
        {
            try
            {

                //1 - Add Printer Monitor
                if (!AddPrinterMonitor(printer.MonitorName, printer.MonitorDllName))
                    return false;
                //2 - Add Printer Port
                if (!AddPrinterPort(printer.PortName, printer.MonitorName))
                    return false;
                //3 - Add Printer Driver
                if (!AddPrinterDriver(printer.Drivers))
                    return false;
                //4 - Add Printer
                if (!AddPrinter(
                    printer.PrinterName,
                    printer.MonitorName,
                    printer.PortName,
                    printer.Drivers.Name
                    ))
                    return false;
                //5 - Configure Virtual Port
                if (!ConfigureVirtualPort(
                    printer.AppPath,
                    printer.MonitorName,
                    printer.PortName
                    ))
                    return false;
                //6 - Restart Spool Service
                RestartSpoolService();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("AddVprinter exception:\n" + ex.Message);
            }
        }

        public bool DeleteVPrinter(PrinterSettings printer)
        {
            bool success = true;
            // 0 - Restart Spooler
            if (!RestartSpoolService())
                return false;
            //1 - Delete Printer
            if (!DeletePrinter(printer.PrinterName))
                return false;
            //2 - Delete Printer Driver
            if (!DeletePrinterDriver(printer.Drivers.Name))
                success = false;
            //3 - Delete Configuration entries
            if (!DeleteVirtualPortConfiguration(printer.MonitorName, printer.PortName))
                success = false;
            //4 - Delete Monitor
            if (!DeletePrinterMonitor(printer.MonitorName))
                success = false;
            //5 - Restart Spool Service
            RestartSpoolService();
            return success;
        }
    }
}