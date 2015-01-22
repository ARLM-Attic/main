using System;
using System.Runtime.InteropServices;

namespace SmartPrint.DriverSetupAction
{
    public class PrintDevice
    {

        #region P/Invoke

        [DllImport("winspool.drv", SetLastError = true, CharSet = CharSet.Auto)]
        static extern bool AddPrinter(string pName, uint Level, [In] ref PRINTER_INFO_2 pPrinter);

        [DllImport("winspool.drv", SetLastError = true, CharSet = CharSet.Auto)]
        static extern bool DeletePrinter(IntPtr hPrinter);

        [DllImport("winspool.drv", SetLastError = true, CharSet = CharSet.Auto)]
        static extern bool GetPrinter(
            IntPtr hPrinter,
            Int32 dwLevel,
            IntPtr pPrinter,
            Int32 dwBuf,
            out Int32 dwNeeded);

        [DllImport("winspool.Drv", SetLastError = true, CharSet = CharSet.Auto)]
        static extern bool SetPrinter(
            IntPtr hPrinter,
            Int32 Level,
            IntPtr pPrinter,
            Int32 Command
        );

        [DllImport("winspool.drv", SetLastError = true, CharSet = CharSet.Auto)]
        static extern bool EnumPrinters(
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
        protected string _serverName;
        protected string _shareName;
        protected string _portName;
        protected string _driverName;
        protected string _description;
        protected string _location = "";
        protected string _separatorFile = "";
        protected string _printProcessor;
        protected string _dataType;
        protected string _parameters;
        protected string _attributes;
        protected string _priority;
        protected string _defaultPriority;
        protected string _startTime;
        protected string _untilTime;
        protected string _status;
        protected string _jobs;
        protected string _averagePPM;

        static PRINTER_INFO_4[] enumDevicesInfos(PRINTER_ENUM_FLAGS flags)
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

        static PRINTER_INFO_2 GetDeviceInfo(string name)
        {
            IntPtr pHandle;
            PrinterDefaults defaults = new PrinterDefaults { DesiredAccess = PrinterAccess.PrinterUse };

            try
            {
                if (!OpenPrinter(name, out pHandle, ref defaults))
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
                finally { ClosePrinter(pHandle); }
            }
            catch { throw; }
        }

        static void SetDeviceInfo(string name, PRINTER_INFO_2 newInfo)
        {
            IntPtr printerHandle;
            PrinterDefaults defaults = new PrinterDefaults { DesiredAccess = PrinterAccess.PrinterAllAccess };

            if (!OpenPrinter(name, out printerHandle, ref defaults))
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

    }
}
