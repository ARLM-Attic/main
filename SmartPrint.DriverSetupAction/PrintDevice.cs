using System;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace SmartPrint.DriverSetupAction
{
    public class PrintDevice
    {
        #region Constants

        public const int ERROR_INSUFFICIENT_BUFFER = 122;
        public const int ERROR_INVALID_PRINTER_NAME = 1801;

        #endregion

        #region P/Invoke

        [DllImport("winspool.drv", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool AddPrinter(string pName, uint Level, [In] ref PRINTER_INFO_2 pPrinter);

        [DllImport("winspool.drv", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool DeletePrinter(IntPtr hPrinter);

        [DllImport("winspool.drv", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool GetPrinter(
            IntPtr hPrinter,
            Int32 dwLevel,
            IntPtr pPrinter,
            Int32 dwBuf,
            out Int32 dwNeeded);

        [DllImport("winspool.Drv", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool SetPrinter(
            IntPtr hPrinter,
            Int32 Level,
            IntPtr pPrinter,
            Int32 Command
        );

        [DllImport("winspool.drv", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool EnumPrinters(
            PRINTER_ENUM_FLAGS Flags,
            string Name,
            uint Level,
            IntPtr pPrinterEnum,
            uint cbBuf,
            ref uint pcbNeeded,
            ref uint pcReturned
            );

        #endregion

        protected string _name;
        protected string _serverName = null;
        protected string _shareName = "";
        protected PrintPort _port;
        protected string _driverName;
        protected string _description = "";
        protected string _location = "";
        protected string _separatorFile = "";
        protected string _printProcessor;
        protected string _dataType;
        protected string _parameters;
        protected PRINTER_ATTRIBUTES _attributes;
        protected uint _priority = 0;
        protected uint _defaultPriority = 0;
        protected uint _startTime = 0;
        protected uint _untilTime = 0;
        protected uint _status = 0;
        protected uint _jobs = 0;
        protected uint _averagePPM = 0;

        #region Properties

        public string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrEmpty(value)) throw new ArgumentException();
                _name = value;
            }
        }

        public string ServerName
        {
            get { return _serverName; }
            set { _serverName = value; }
        }

        public string ShareName
        {
            get { return _shareName; }
            set { _shareName = value; }
        }

        public PrintPort Port
        {
            get { return _port; }
            set { _port = value; }
        }

        public string DriverName
        {
            get { return _driverName; }
            set
            {
                if (string.IsNullOrEmpty(value)) throw new ArgumentException();
                _driverName = value;
            }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public string Location
        {
            get { return _location; }
            set { _location = value; }
        }

        public string SeparatorFile
        {
            get { return _separatorFile; }
            set { _separatorFile = value; }
        }

        public string PrintProcessor
        {
            get { return _printProcessor; }
            set { _printProcessor = value; }
        }

        public string DataType
        {
            get { return _dataType; }
            set { _dataType = value; }
        }

        public string Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }

        public PRINTER_ATTRIBUTES Attributes
        {
            get { return _attributes; }
            set { _attributes = value; }
        }

        public uint Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        public uint DefaultPriority
        {
            get { return _defaultPriority; }
            set { _defaultPriority = value; }
        }

        public uint StartTime
        {
            get { return _startTime; }
            set { _startTime = value; }
        }

        public uint UntilTime
        {
            get { return _untilTime; }
            set { _untilTime = value; }
        }

        public uint Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public uint Jobs
        {
            get { return _jobs; }
            set { _jobs = value; }
        }

        public uint AveragePPM
        {
            get { return _averagePPM; }
            set { _averagePPM = value; }
        }

        #endregion

        public static PRINTER_INFO_4[] enumDevicesInfos(PRINTER_ENUM_FLAGS flags)
        {
            uint cbNeeded = 0;
            uint cReturned = 0;
            if (EnumPrinters(flags, null, 4, IntPtr.Zero, 0, ref cbNeeded, ref cReturned))
                return null;
            int lastWin32Error = Marshal.GetLastWin32Error();
            if (lastWin32Error == ERROR_INSUFFICIENT_BUFFER)
            {
                IntPtr pAddr = Marshal.AllocHGlobal((int)cbNeeded);
                if (EnumPrinters(flags, null, 4, pAddr, cbNeeded, ref cbNeeded, ref cReturned))
                {
                    PRINTER_INFO_4[] printerInfo4 = new PRINTER_INFO_4[cReturned];
                    int offset = pAddr.ToInt32();
                    Type type = typeof(PRINTER_INFO_4);
                    int increment = Marshal.SizeOf(type);
                    for (int i = 0; i < cReturned; i++)
                    {
                        printerInfo4[i] = (PRINTER_INFO_4)Marshal.PtrToStructure(new IntPtr(offset), type);
                        offset += increment;
                    }
                    return printerInfo4;
                }
                Marshal.FreeHGlobal(pAddr);
                lastWin32Error = Marshal.GetLastWin32Error();
            }
            throw new Win32Exception(lastWin32Error);
        }

        public static PRINTER_INFO_2 GetDeviceInfo(string name)
        {
            IntPtr pHandle;
            PrinterDefaults defaults = new PrinterDefaults { DesiredAccess = PRINTER_ACCESS.PrinterUse };

            try
            {
                if (!PrintPort.OpenPrinter(name, out pHandle, ref defaults))
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                try
                {
                    Int32 cbNeeded = 0;
                    IntPtr pAddr = IntPtr.Zero;
                    if (!GetPrinter(pHandle, 2, IntPtr.Zero, 0, out cbNeeded) || 0 > cbNeeded)
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
                finally { PrintPort.ClosePrinter(pHandle); }
            }
            catch { throw; }
        }

        public static void SetDeviceInfo(string name, PRINTER_INFO_2 newInfo)
        {
            IntPtr printerHandle;
            PrinterDefaults defaults = new PrinterDefaults { DesiredAccess = PRINTER_ACCESS.PrinterAllAccess };

            if (!PrintPort.OpenPrinter(name, out printerHandle, ref defaults))
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
            finally { PrintPort.ClosePrinter(printerHandle); }
        }

        public static void RenameDevice(string oldName, string newName)
        {
            try
            {
                PRINTER_INFO_2 pi = GetDeviceInfo(oldName);
                pi.pPrinterName = newName;
                PrintDevice.SetDeviceInfo(oldName, pi);
            }
            catch { throw; }
        }


    }
}
