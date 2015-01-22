using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using Microsoft.Win32;

namespace SmartPrint.DriverSetupAction
{
    public class SmartPrintDevice : PrintDevice
    {
        #region Constants

        private static const int ERROR_INSUFFICIENT_BUFFER  = 122;
        private static const int ERROR_INVALID_PRINTER_NAME = 1801;

        public static const string DEFAULT_NAME             = "SMARTPRINTER";
        public static const string DEFAULT_APP_PATH         = @"C:\Program Files\SMARTdoc\Share\";
        public static const string DEFAULT_DESCRIPTION      = "SMARTdoc printer";

        #endregion

        #region Private Fields

        private string _name = DEFAULT_NAME;
        private string _description = DEFAULT_DESCRIPTION;
        private SmartPrintPort _port;

        #endregion

        #region Constructors

        private SmartPrintDevice(string name, string description, string appPath, string portName)
        {
            Name = name;
            Description = description;
            Port = SmartPrintPort.AddPort(portName, appPath);
        }

        #endregion

        #region Properties
        public string Name
        {
            get { return _name; }
            set { if (!string.IsNullOrEmpty(value)) _name = value; }
        }

        public string Description
        {
            get { return _description; }
            set { if (!string.IsNullOrEmpty(value)) _description = value; }
        }

        public SmartPrintPort Port { get; set; }

        #endregion

        #region Static Methods

        private static void RenameDevice(string oldName, string newName)
        {
            try
            {
                PRINTER_INFO_2 pi = PrintDevice.GetDeviceInfo(oldName);
                pi.pPrinterName = newName;
                PrintDevice.SetDeviceInfo(oldName, pi);
            }
            catch { throw; }
        }

        private static void AddDevice()
        {
            PRINTER_INFO_2 pi = new PRINTER_INFO_2
            {
                pServerName = null,
                pPrinterName = Name,
                pShareName = "",
                pPortName = Port.Name,
                pDriverName = PrintDriver.Name,
                pComment = Description,
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
                if (PrintDevice.AddPrinter(null, 2, ref pi) == 0)
                    throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            catch { throw; }
        }

        private static void DeleteDevice(string name)
        {
            if (!name.EndsWith("\0")) name += "\0";
            IntPtr handle = IntPtr.Zero;
            PrinterDefaults defaults = new PrinterDefaults { DesiredAccess = PrinterAccess.PrinterAllAccess };
            try
            {
                if (!PrintPort.OpenPrinter(name, out handle, ref defaults))
                {
                    int errorCode = Marshal.GetLastWin32Error();
                    if (errorCode != ERROR_INVALID_PRINTER_NAME)
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