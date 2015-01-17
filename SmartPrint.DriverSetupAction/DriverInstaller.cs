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
        private static extern int DeleteMonitor(string pName, string pEnvironment, string pMonitorName);
        
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
        private static extern bool GetPrinterDriverDirectory(StringBuilder pName, StringBuilder pEnv, int Level, [Out] StringBuilder outPath, int bufferSize, ref int Bytes);
        #endregion
        
        #region Printer
     
        [DllImport("winspool.drv", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern Int32 AddPrinter(string pName, uint Level, [In] ref PRINTER_INFO_2 pPrinter);
     
        [DllImport("winspool.drv", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool DeletePrinter(IntPtr hPrinter);

        [DllImport("winspool.drv", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool GetPrinter(
            IntPtr hPrinter,
            Int32 dwLevel,
            IntPtr pPrinter,
            Int32 dwBuf,
            out Int32 dwNeeded);

        [DllImport("winspool.Drv", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool SetPrinter(
            IntPtr hPrinter,
            Int32 Level,
            IntPtr pPrinter,
            Int32 Command
        );

        [DllImport("winspool.drv", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool EnumPrinters(
            PRINTER_ENUM_FLAGS Flags,
            string Name,
            uint Level,
            IntPtr pPrinterEnum,
            uint cbBuf,
            ref uint pcbNeeded,
            ref uint pcReturned
            );
        enum PrinterCommand
        {
            PRINTER_CONTROL_NOT_SUPPORTED   = 0,
            PRINTER_CONTROL_PAUSE           = 1,
            PRINTER_CONTROL_RESUME          = 2,
            PRINTER_CONTROL_PURGE           = 3,
            PRINTER_CONTROL_SET_STATUS      = 4
        }

        [Flags]
        enum PRINTER_ENUM_FLAGS
        {
            /// <summary>
            /// Return information about the default printer.
            /// </summary>
            PRINTER_ENUM_DEFAULT = 0x00000001,
            /// <summary>
            /// Enumerate local printer objects.
            /// </summary>
            PRINTER_ENUM_LOCAL = 0x00000002,
            /// <summary>
            /// Enumerate printer connections previously added through RpcAddPerMachineConnection.
            /// </summary>
            PRINTER_ENUM_CONNECTIONS = 0x00000004,
            /// <summary>
            /// 
            /// </summary>
            PRINTER_ENUM_FAVORITE = 0x00000004,
            /// <summary>
            /// Enumerate printers on the print server, network domain, or a specific print provider.
            /// </summary>
            PRINTER_ENUM_NAME = 0x00000008,
            /// <summary>
            /// Enumerate network printers and other print servers that are in the same domain as the print server.
            /// </summary>
            PRINTER_ENUM_REMOTE = 0x00000010,
            /// <summary>
            /// Only enumerate printers with the shared attribute set. 
            /// This flag MUST be combined with one or more of the other flags.
            /// </summary>
            PRINTER_ENUM_SHARED = 0x00000020,
            /// <summary>
            /// Enumerate network printers that are in the same domain as the print server.
            /// </summary>
            PRINTER_ENUM_NETWORK = 0x00000040,
            /// <summary>
            /// Indicates that the printer object contains further enumerable child objects. 
            /// When a server enumerates print servers (see RpcEnumPrinters (section 3.1.4.2.1) 
            /// the server can set this bit for each enumerated server whose name matches the server's domain name.
            /// </summary>
            PRINTER_ENUM_EXPAND = 0x00004000,
            /// <summary>
            /// Indicates that the printer object is capable of containing enumerable objects.
            /// One such object is a print provider, which is a print server that contains printers.
            /// </summary>
            PRINTER_ENUM_CONTAINER = 0x00008000,
            /// <summary>
            /// 
            /// </summary>
            PRINTER_ENUM_ICONMASK = 0x00ff0000,
            /// <summary>
            /// Indicates that, where appropriate, an application treats the printer object 
            /// as a top-level network name, such as Windows network. 
            /// A GUI application can choose to display an icon of choice for this type of object.
            /// </summary>
            PRINTER_ENUM_ICON1 = 0x00010000,
            /// <summary>
            /// Indicates that, where appropriate, an application treats an object as a network domain name. 
            /// A GUI application can<199> choose to display an icon of choice for this type of object.
            /// </summary>
            PRINTER_ENUM_ICON2 = 0x00020000,
            /// <summary>
            /// Indicates that, where appropriate, an application treats an object as a print server. 
            /// A GUI application can choose to display an icon of choice for this type of object.
            /// </summary>
            PRINTER_ENUM_ICON3 = 0x00040000,
            PRINTER_ENUM_ICON4 = 0x00080000,
            PRINTER_ENUM_ICON5 = 0x00100000,
            PRINTER_ENUM_ICON6 = 0x00200000,
            PRINTER_ENUM_ICON7 = 0x00400000,
            /// <summary>
            /// Indicates that, where appropriate, an application treats an object as a print server. 
            /// A GUI application can choose to display an icon of choice for this type of object.
            /// </summary>
            PRINTER_ENUM_ICON8 = 0x00800000,
            /// <summary>
            /// Indicates that an application cannot display the printer object.
            /// </summary>
            PRINTER_ENUM_HIDE = 0x01000000,
            PRINTER_ENUM_CATEGORY_ALL = 0x02000000,
            PRINTER_ENUM_CATEGORY_3D = 0x04000000
        }
        
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct PRINTER_INFO_2
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

        private static int ERROR_INSUFFICIENT_BUFFER = 122;

        #region Private Methods
        private static bool AddPrinterMonitor(string monitorName, string monitorDllName)
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

        private static bool DeletePrinterMonitor(string monitorName)
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

        private static bool AddPrinterPort(string portName, string portType)
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

        private static bool DeletePrinterPort(string portName, string portType)
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

        private static bool AddPrinterDriver(PrinterDriverSettings driver)
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

        private static bool DeletePrinterDriver(string driverName)
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

        private static bool AddPrinter(string printerName, string monitorName, string portName, string driverName, string description)
        {
            if (!portName.EndsWith("\0")) portName += "\0";
            PRINTER_INFO_2 pi = new PRINTER_INFO_2
            {
                pServerName = null,
                pPrinterName = printerName,
                pShareName = "",
                pPortName = portName,
                pDriverName = driverName,
                pComment = description,
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

        private static bool AddPrinter(string printerName, string monitorName, string portName, string driverName)
        {
            return AddPrinter(printerName, monitorName, portName, driverName, monitorName);
        }

        private static bool DeletePrinter(string printerName)
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

        private static bool ConfigureVirtualPort(string appPath, string monitorName, string portName)
        {
            try
            {
                if (!portName.EndsWith("\0")) portName += "\0";
                string outputPath = string.Format(@"{0}Temp", appPath);
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

        private static bool DeleteVirtualPortConfiguration(string monitorName, string portName)
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

        private static bool AddVPrinter(PrinterSettings printer)
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

        public static bool DeleteVPrinter(PrinterSettings printer)
        {
            try
            {
                bool success = true;
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
            catch (Exception ex)
            {
                throw new Exception("DeleteVprinter exception:\n" + ex.Message);
            }
        }

        private static PRINTER_INFO_2[] enumPrinters(PRINTER_ENUM_FLAGS flags)
        {
            uint cbNeeded = 0;
            uint cReturned = 0;
            if (EnumPrinters(flags, null, 2, IntPtr.Zero, 0, ref cbNeeded, ref cReturned))
            {
                return null;
            }
            int lastWin32Error = Marshal.GetLastWin32Error();
            if (lastWin32Error == ERROR_INSUFFICIENT_BUFFER)
            {
                IntPtr pAddr = Marshal.AllocHGlobal((int)cbNeeded);
                if (EnumPrinters(flags, null, 2, pAddr, cbNeeded, ref cbNeeded, ref cReturned))
                {
                    PRINTER_INFO_2[] printerInfo2 = new PRINTER_INFO_2[cReturned];
                    int offset = pAddr.ToInt32();
                    Type type = typeof(PRINTER_INFO_2);
                    int increment = Marshal.SizeOf(type);
                    for (int i = 0; i < cReturned; i++)
                    {
                        printerInfo2[i] = (PRINTER_INFO_2)Marshal.PtrToStructure(new IntPtr(offset), type);
                        offset += increment;
                    }
                    Marshal.FreeHGlobal(pAddr);
                    return printerInfo2;
                }
                lastWin32Error = Marshal.GetLastWin32Error();
            }
            throw new Win32Exception(lastWin32Error);
        }

        public static PRINTER_INFO_2 GetPrinterInfo(String printerName)
        {
            IntPtr pHandle;
            PrinterDefaults defaults = new PrinterDefaults 
                { DesiredAccess = PrinterAccess.PrinterUse };

            if (!OpenPrinter(printerName, out pHandle, ref defaults))
                throw new Win32Exception(Marshal.GetLastWin32Error());
            try
            {
                Int32 cbNeeded = 0;
                IntPtr pAddr = IntPtr.Zero;
                if (!GetPrinter(pHandle, 2, IntPtr.Zero, 0, out cbNeeded) && 0 >= cbNeeded)
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                pAddr = Marshal.AllocHGlobal((int)cbNeeded);
                try
                {
                    if (!GetPrinter(pHandle, 2, pAddr, cbNeeded, out cbNeeded))
                        throw new Win32Exception(Marshal.GetLastWin32Error());
                    return (PRINTER_INFO_2)Marshal.PtrToStructure(pAddr, typeof(PRINTER_INFO_2));
                }
                catch { throw; }
                finally { Marshal.FreeHGlobal(pAddr); }
            }
            catch { throw; }
            finally { ClosePrinter(pHandle); }
        }

        public static void SetPrinterInfo(string printerName, PRINTER_INFO_2 newInfo)
        {
            IntPtr printerHandle;
            PrinterDefaults defaults = new PrinterDefaults
                { DesiredAccess = PrinterAccess.PrinterAllAccess };

            if (!OpenPrinter(printerName, out printerHandle, ref defaults))
                throw new Win32Exception(Marshal.GetLastWin32Error());
            try
            {
                IntPtr infoHandle = Marshal.AllocHGlobal(Marshal.SizeOf(newInfo));
                Marshal.StructureToPtr(newInfo, infoHandle, false);
                try
                {
                    if (!SetPrinter(printerHandle, 2, infoHandle, 0))
                        throw new Win32Exception(Marshal.GetLastWin32Error());
                }
                catch { throw; }
                finally { Marshal.FreeHGlobal(infoHandle); }
            }
            catch { throw; }
            finally { ClosePrinter(printerHandle); }
        }

        public static void RenamePrinter(string oldName, string newName)
        {
            try
            {
                PRINTER_INFO_2 pi = GetPrinterInfo(oldName);
                pi.pPrinterName = newName;
                SetPrinterInfo(oldName, pi);
            }
            catch { throw; }
        }

        #endregion

        #region Public Interface

        public static string GetPrinterDriverDirectory()
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

        public static bool AddSmartPrinterPort(string name)
        {
            return AddPrinterPort(name, "SMARTPRINTER");
        }

        public static bool DeleteSmartPrinterPort(string name)
        {
            return DeletePrinterPort(name, "SMARTPRINTER");
        }

        public static bool AddSmartPrinterDriver()
        {
            return AddPrinterDriver(new PrinterDriverSettings());
        }

        public static bool DeleteSmartPrinterDriver()
        {
            return DeletePrinterDriver("SMARTPRINTER");
        }

        public static bool AddSmartPrinter(string name, string description)
        {
            var settings = new PrinterSettings(name);
            return AddPrinter(name, settings.MonitorName, settings.PortName, settings.Drivers.Name);
        }

        public static bool DeleteSmartPrinter(string name)
        {
            return DeletePrinter(name);
        }

        public static bool ConfigureSmarPrinterPort(string printerName, string appPath)
        {
            var settings = new PrinterSettings(printerName);
            return ConfigureVirtualPort(appPath, settings.MonitorName, settings.PortName);
        }

        public static bool DeleteSmartPrinterConfiguration(string printerName)
        {
            var settings = new PrinterSettings(printerName);
            return DeleteVirtualPortConfiguration(settings.MonitorName, settings.PortName);
        }

        public static bool RestartSpoolService()
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

        public static bool AddVSmartPrinter(string name, string description)
        {
            var settings = new PrinterSettings(name, description);
            try
            {
                //1 - Add Printer Port
                if (!AddPrinterPort(settings.PortName, settings.MonitorName))
                    return false;
                //4 - Add Printer
                if (!AddPrinter(
                    settings.PrinterName,
                    settings.MonitorName,
                    settings.PortName,
                    settings.Drivers.Name,
                    settings.Description
                    ))
                    return false;
                //5 - Configure Virtual Port
                if (!ConfigureVirtualPort(
                    settings.AppPath,
                    settings.MonitorName,
                    settings.PortName
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

        #endregion
    }
}