using System;
using System.Runtime.InteropServices;

namespace SmartPrint.DriverSetupAction
{
    public class PrintPort
    {

        #region P/Invoke

        [DllImport("winspool.drv", SetLastError = true)]
        public static extern bool OpenPrinter(string pPrinterName, out IntPtr phPrinter, ref PrinterDefaults pDefault);

        [DllImport("winspool.drv", SetLastError = true)]
        public static extern bool ClosePrinter(IntPtr phPrinter);

        [DllImport("winspool.drv", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool XcvDataW(IntPtr hXcv, string pszDataName, IntPtr pInputData, UInt32 cbInputData, IntPtr pOutputData, UInt32 cbOutputData, out UInt32 pcbOutputNeeded, out UInt32 pdwStatus);

        #endregion

        protected string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrEmpty(value)) throw new ArgumentException();
                _name = value;
            }
        }

        public PrintPort() { }
        public PrintPort(string name)
        {
            Name = name;
        }
    }
}
