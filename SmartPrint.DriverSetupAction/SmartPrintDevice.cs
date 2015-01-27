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

        public override PrintPort Port
        {
            get { return (SmartPrintPort)_port; }
            set { _port = value; }
        }

        #region Constructors

        private SmartPrintDevice(string name, string description)
            : this(name, description, name + ":\0") { }

        private SmartPrintDevice(string name, string description, string portName)
        {
            Name = name;
            Description = description;
            Port = SmartPrintPort.Install(portName);
        }

        #endregion

        #region Static Methods

        public static SmartPrintDevice Install(string name, string description)
        {
            return Install(name, description, name + ":\0");
        }

        public static SmartPrintDevice Install(string name, string description, string portName)
        {
            PRINTER_INFO_2 info = new PRINTER_INFO_2()
            {
                pPrinterName = name,
                pComment = description,
                pPortName = portName
            };
            SmartPrintDevice device = (SmartPrintDevice)FromInfo2(info);
            device.Port = SmartPrintPort.Install(portName);
            device.Install();
            return device;
        }

        #endregion

        #region Public Methods

        #endregion
    }
}