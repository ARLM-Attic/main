using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using Microsoft.Win32;

namespace SmartPrint.DriverSetupAction
{
    public class SmartPrintDevice : PrintDevice
    {
        #region Constants

        public const string DEFAULT_NAME = "SMARTPRINTER";
        public const string DEFAULT_APP_PATH = @"C:\Program Files\SMARTdoc\Share\";
        public const string DEFAULT_DESCRIPTION = "SMARTdoc printer";
        public const string DEFAULT_DRIVER_NAME = "SMARTPRINTER";

        #endregion

        private SmartPrintPort _port;

        public SmartPrintPort Port { get; private set; }

        public override string PortName
        {
            get { return _port.Name; }
        }

        #region Constructors

        private SmartPrintDevice(string name, string description)
            : this(name, description, name + "\0") { }

        private SmartPrintDevice(string name, string description, string portName)
        {
            Name = name;
            Description = description;
            DriverName = DEFAULT_DRIVER_NAME;
            Port = SmartPrintPort.Install(portName);
        }

        #endregion

        #region Static Methods

        protected override void FromInfo2(PRINTER_INFO_2 info)
        {
            ServerName = info.pServerName;
            ShareName = info.pShareName;
            Description = info.pComment;
            Location = info.pLocation;
            SeparatorFile = info.pSepFile;
            Processor = new PrintProcessor(
                info.pPrintProcessor,
                info.pDatatype,
                info.pParameters);
            Attributes = info.Attributes;
            Priority = info.Priority;
            DefaultPriority = info.DefaultPriority;
            StartTime = info.StartTime;
            UntilTime = info.UntilTime;
            Status = info.Status;
            Jobs = info.cJobs;
            AveragePPM = info.AveragePPM;
        }

        public static SmartPrintDevice Install(string name, string description)
        {
            return Install(name, description, name + "\0");
        }

        public static SmartPrintDevice Install(string name, string description, string portName)
        {
            PRINTER_INFO_2 info = new PRINTER_INFO_2()
            {
                pPrinterName = name,
                pComment = description,
                pPortName = portName,
                pDriverName = DEFAULT_DRIVER_NAME
            };
            SmartPrintDevice device = new SmartPrintDevice(name, description, portName);
            Install(info);
            return device;
        }

        #endregion

        #region Public Methods

        #endregion
    }
}