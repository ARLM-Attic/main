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
            Port = SmartPrintPort.Install(portName, appPath);
        }

        #endregion

        #region Static Methods

        public static SmartPrintDevice Install(string name, string description, string appPath, string portName)
        {
            PRINTER_INFO_2 info = new PRINTER_INFO_2();
            info.pPrinterName = name;
            info.pComment = description;
            info.pPortName = portName;
            SmartPrintDevice device = (SmartPrintDevice)FromInfo2(info);
            device.Port = SmartPrintPort.Install(name, appPath);
            device.Install();
            return device;
        }

        #endregion
    }
}