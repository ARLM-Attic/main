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

        #region Protected Fields

        protected string _name;
        protected string _serverName = null;
        protected string _shareName = "";
        protected PrintPort _port;
        protected string _driverName;
        protected string _description = "";
        protected string _location = "";
        protected string _separatorFile = "";
        protected PrintProcessor _processor = PrintProcessor.Default;
        protected PRINTER_ATTRIBUTES _attributes = PRINTER_ATTRIBUTES.PRINTER_ATTRIBUTE_LOCAL;
        protected JOB_PRIORITY _priority = JOB_PRIORITY.DEF_PRIORITY;
        protected JOB_PRIORITY _defaultPriority = JOB_PRIORITY.DEF_PRIORITY;
        protected uint _startTime = 0;
        protected uint _untilTime = uint.MaxValue;
        protected PRINTER_STATUS _status = PRINTER_STATUS.PRINTER_STATUS_SERVER_UNKNOWN;
        protected uint _jobs = 0;
        protected uint _averagePPM = 0;

        #endregion

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

        public PrintProcessor Processor
        {
            get {return _processor; }
            set { _processor = value; }
        }

        public PRINTER_ATTRIBUTES Attributes
        {
            get { return _attributes; }
            set { _attributes = value; }
        }

        public JOB_PRIORITY Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        public JOB_PRIORITY DefaultPriority
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

        public PRINTER_STATUS Status
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

        #region Static Methods
        protected PRINTER_INFO_2 ToInfo2()
        {
            PRINTER_INFO_2 info = new PRINTER_INFO_2();
            info.pServerName = ServerName;
            info.pPrinterName = Name;
            info.pShareName = ShareName;
            info.pPortName = Port.Name;
            info.pDriverName = DriverName;
            info.pComment = Description;
            info.pLocation = Location;
            info.pSepFile = SeparatorFile;
            info.pPrintProcessor = Processor.Name;
            info.pDatatype = Processor.DataType;
            info.pParameters = Processor.Parameters;
            info.Attributes = Attributes;
            info.Priority = Priority;
            info.DefaultPriority = DefaultPriority;
            info.StartTime = StartTime;
            info.UntilTime = UntilTime;
            info.Status = Status;
            info.cJobs = Jobs;
            info.AveragePPM = AveragePPM;
            return info;
        }

        protected PRINTER_INFO_4 ToInfo4()
        {
            PRINTER_INFO_4 info = new PRINTER_INFO_4();
            info.pPrinterName = Name;
            info.pServerName = ServerName;
            info.Attributes = Attributes;
            return info;
        }

        protected static PrintDevice FromInfo2(PRINTER_INFO_2 info)
        {
            PrintDevice device = new PrintDevice();
            device.ServerName = info.pServerName;
            device.Name = info.pPrinterName;
            device.ShareName = info.pShareName;
            device.Port = new PrintPort(info.pPortName);
            device.DriverName = info.pDriverName;
            device.Description = info.pComment;
            device.Location = info.pLocation;
            device.SeparatorFile = info.pSepFile;
            device.Processor = new PrintProcessor(info.pPrintProcessor, info.pDatatype, info.pParameters);
            device.Attributes = info.Attributes;
            device.Priority = info.Priority;
            device.DefaultPriority = info.DefaultPriority;
            device.StartTime = info.StartTime;
            device.UntilTime = info.UntilTime;
            device.Status = info.Status;
            device.Jobs = info.cJobs;
            device.AveragePPM = info.AveragePPM;
            return device;
        }

        protected static PrintDevice FromInfo4(PRINTER_INFO_4 info)
        {
            PrintDevice device = new PrintDevice();
            device.Name = info.pPrinterName;
            device.ServerName = info.pServerName;
            device.Attributes = info.Attributes;
            return device;
        }

        protected static PRINTER_INFO_2[] enumDeviceInfos2(PRINTER_ENUM_FLAGS flags)
        {
            uint cbNeeded = 0;
            uint cReturned = 0;
            if (EnumPrinters(flags, null, 2, IntPtr.Zero, 0, ref cbNeeded, ref cReturned))
                return null;
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
                    return printerInfo2;
                }
                Marshal.FreeHGlobal(pAddr);
                lastWin32Error = Marshal.GetLastWin32Error();
            }
            throw new Win32Exception(lastWin32Error);
        }

        protected static PRINTER_INFO_4[] enumDeviceInfos4(PRINTER_ENUM_FLAGS flags)
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

        protected static PRINTER_INFO_2 GetDeviceInfo2(string name)
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

        protected static PRINTER_INFO_4 GetDeviceInfo4(string name)
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
                    if (!GetPrinter(pHandle, 4, IntPtr.Zero, 0, out cbNeeded) || 0 > cbNeeded)
                        throw new Win32Exception(Marshal.GetLastWin32Error());
                    pAddr = Marshal.AllocHGlobal((int)cbNeeded);
                    try
                    {
                        if (!GetPrinter(pHandle, 4, pAddr, cbNeeded, out cbNeeded))
                            throw new Win32Exception(Marshal.GetLastWin32Error());
                        return (PRINTER_INFO_4)Marshal.PtrToStructure(pAddr, typeof(PRINTER_INFO_4));
                    }
                    catch { throw; }
                    finally { Marshal.FreeHGlobal(pAddr); }
                }
                catch { throw; }
                finally { PrintPort.ClosePrinter(pHandle); }
            }
            catch { throw; }
        }

        protected static void SetDeviceInfo2(string name, PRINTER_INFO_2 newInfo)
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

        protected static void SetDeviceInfo4(string name, PRINTER_INFO_4 newInfo)
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
                    if (!SetPrinter(printerHandle, 4, infoHandle, 0))
                        throw new Win32Exception(Marshal.GetLastWin32Error());
                }
                catch { throw; }
                finally { Marshal.FreeHGlobal(infoHandle); }
            }
            catch { throw; }
            finally { PrintPort.ClosePrinter(printerHandle); }
        }

        protected static void RenameDevice2(string oldName, string newName)
        {
            try
            {
                PRINTER_INFO_2 pi = GetDeviceInfo2(oldName);
                pi.pPrinterName = newName;
                PrintDevice.SetDeviceInfo2(oldName, pi);
            }
            catch { throw; }
        }
        protected static void RenameDevice4(string oldName, string newName)
        {
            try
            {
                PRINTER_INFO_4 pi = GetDeviceInfo4(oldName);
                pi.pPrinterName = newName;
                PrintDevice.SetDeviceInfo4(oldName, pi);
            }
            catch { throw; }
        }

        public static PrintDevice Install(PRINTER_INFO_2 info)
        {
            try
            {
                if (!AddPrinter(null, 2, ref info))
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                return PrintDevice.FromInfo2(info);
            }
            catch { throw; }
        }

        public static void Uninstall(string name)
        {
            if (!name.EndsWith("\0")) name += "\0";
            IntPtr handle = IntPtr.Zero;
            PrinterDefaults defaults = new PrinterDefaults { DesiredAccess = PRINTER_ACCESS.PrinterAllAccess };
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

        public void Install()
        {
            Install(ToInfo2());
        }

        public void Uninstall()
        {
            Uninstall(Name);
        }

    }
}
