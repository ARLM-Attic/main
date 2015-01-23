using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using Microsoft.Win32;

namespace SmartPrint.DriverSetupAction
{
    public class SmartPrintDevice : PrintDevice
    {
        #region Constants

        public const string DEFAULT_NAME             = "SMARTPRINTER";
        public const string DEFAULT_APP_PATH         = @"C:\Program Files\SMARTdoc\Share\";
        public const string DEFAULT_DESCRIPTION      = "SMARTdoc printer";

        #endregion

        #region Constructors

        private SmartPrintDevice(string name, string description, string appPath, string portName)
        {
            Name = name;
            Description = description;
            Port = SmartPrintPort.AddPort(portName, appPath);
        }

        #endregion

        #region Static Methods

        public static SmartPrintDevice AddDevice(string name, string description, string appPath, string portName)
        {
            SmartPrintDevice device = new SmartPrintDevice(name, description, appPath, portName);
            PRINTER_INFO_2 pi = new PRINTER_INFO_2
            {
                pServerName = null,
                pPrinterName = device.Name,
                pShareName = "",
                pPortName = device.Port.Name,
                pDriverName = PrintDriver.NAME,
                pComment = device.Description,
                pLocation = "",
                pDevMode = IntPtr.Zero,
                pSepFile = "",
                pPrintProcessor = "WinPrint",
                pDatatype = "RAW",
                pParameters = "",
                pSecurityDescriptor = IntPtr.Zero
            };
            try
            {
                if (!PrintDevice.AddPrinter(null, 2, ref pi))
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                return device;
            }
            catch { throw; }
        }

        public static void DeleteDevice(string name)
        {
            if (!name.EndsWith("\0")) name += "\0";
            IntPtr handle = IntPtr.Zero;
            PrinterDefaults defaults = new PrinterDefaults { DesiredAccess = PRINTER_ACCESS.PrinterAllAccess };
            try
            {
                if (!PrintPort.OpenPrinter(name, out handle, ref defaults))
                {
                    int errorCode = Marshal.GetLastWin32Error();
                    if (errorCode != PrintDevice.ERROR_INVALID_PRINTER_NAME)
                        throw new Win32Exception(errorCode);
                }
                if (!DeletePrinter(handle))
                    throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            catch { throw; }
            finally
            {
                PrintPort.ClosePrinter(handle);
            }
        }

        #endregion
    }
}