using System;

namespace SmartPrint.DriverSetupAction
{
    [Flags]
    public enum PRINTER_ATTRIBUTES
    {
        /// <summary>
        /// No attribute set.
        /// </summary>
        PRINTER_ATTRIBUTE_NOT_SUPPORTED = 0,

        /// <summary>
        /// If set, the printer spools and starts printing after the last page is spooled.
        /// If not set and PRINTER_ATTRIBUTE_DIRECT is not set, the printer spools and
        /// prints while spooling.
        /// </summary>
        PRINTER_ATTRIBUTE_QUEUED = 0x1,

        /// <summary>
        /// Job is sent directly to the printer (it is not spooled).
        /// </summary>
        PRINTER_ATTRIBUTE_DIRECT = 0x2,

        /// <summary>
        /// Printer is default printer.
        /// </summary>
        PRINTER_ATTRIBUTE_DEFAULT = 0x4,

        /// <summary>
        /// Printer is shared.
        /// </summary>
        PRINTER_ATTRIBUTE_SHARED = 0x8,

        /// <summary>
        /// Printer is a network printer connection.
        /// </summary>
        PRINTER_ATTRIBUTE_NETWORK = 0x10,

        /// <summary>
        /// Reserved for future use.
        /// </summary>
        PRINTER_ATTRIBUTE_HIDDEN = 0x20,

        /// <summary>
        /// Printer is a local printer.
        /// </summary>
        PRINTER_ATTRIBUTE_LOCAL = 0x40,

        /// <summary>
        /// If set, DevQueryPrint is called. DevQueryPrint may fail if the document
        /// and printer setups do not match. Setting this flag causes mismatched
        /// documents to be held in the queue.
        /// </summary>
        PRINTER_ATTRIBUTE_ENABLE_DEVQ = 0x80,

        /// <summary>
        /// If set, jobs are kept after they are printed. If unset, jobs are deleted.
        /// </summary>
        PRINTER_ATTRIBUTE_KEEPPRINTEDJOBS = 0x100,

        /// <summary>
        /// If set and printer is set for print-while-spooling, any jobs that have
        /// completed spooling are scheduled to print before jobs that have not
        /// completed spooling.
        /// </summary>
        PRINTER_ATTRIBUTE_DO_COMPLETE_FIRST = 0x200,

        /// <summary>
        /// No description.
        /// </summary>
        PRINTER_ATTRIBUTE_WORK_OFFLINE = 0x400,

        /// <summary>
        /// No description.
        /// </summary>
        PRINTER_ATTRIBUTE_ENABLE_BIDI = 0x800,

        /// <summary>
        /// Indicates that only raw data type print jobs can be spooled.
        /// </summary>
        PRINTER_ATTRIBUTE_RAW_ONLY = 0x1000,

        /// <summary>
        /// Indicates whether the printer is published in the directory service.
        /// </summary>
        PRINTER_ATTRIBUTE_PUBLISHED = 0x2000,

        /// <summary>
        /// <note>In Windows XP and later versions of Windows:</note>
        /// If set, printer is a fax printer. This can only be set by AddPrinter,
        /// but it can be retrieved by EnumPrinters and GetPrinter.
        /// </summary>
        PRINTER_ATTRIBUTE_FAX = 0x4000,

        /// <summary>
        /// <note>In Windows Server 2003:</note>
        /// Indicates the printer is currently connected through a terminal server.
        /// </summary>
        PRINTER_ATTRIBUTE_TS = 0x8000,

        /// <summary>
        /// <note>In Windows Vista and later versions of Windows:</note>
        /// The printer was installed by using the Push Printer Connections
        /// user policy. See Print Management Step-by-Step Guide.
        /// </summary>
        PRINTER_ATTRIBUTE_PUSHED_USER = 0x20000,

        /// <summary>
        /// <note>In Windows Vista and later versions of Windows:</note>
        /// The printer was installed by using the Push Printer Connections
        /// computer policy. See Print Management Step-by-Step Guide.
        /// </summary>
        PRINTER_ATTRIBUTE_PUSHED_MACHINE = 0x40000,

        /// <summary>
        /// <note>In Windows Vista and later versions of Windows:</note>
        /// Printer is a per-machine connection.
        /// </summary>
        PRINTER_ATTRIBUTE_MACHINE = 0x0000,

        /// <summary>
        /// <note>In Windows Vista and later versions of Windows:</note>
        /// A computer has connected to this printer and given it a friendly name.
        /// </summary>
        PRINTER_ATTRIBUTE_FRIENDLY_NAME = 0x100000,

        /// <summary>
        /// <note>In Windows Vista and later versions of Windows:</note>
        /// No description.
        /// </summary>
        PRINTER_ATTRIBUTE_TS_GENERIC_DRIVER = 0x200000,
    }

    [Flags]
    public enum PRINTER_ENUM_FLAGS
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

    public enum PRINTER_COMMAND
    {
        PRINTER_CONTROL_NOT_SUPPORTED = 0,
        PRINTER_CONTROL_PAUSE = 1,
        PRINTER_CONTROL_RESUME = 2,
        PRINTER_CONTROL_PURGE = 3,
        PRINTER_CONTROL_SET_STATUS = 4
    }

    [Flags]
    public enum PRINTER_ACCESS
    {
        ServerAdmin             = 0x00000001,
        ServerEnum              = 0x00000002,
        PrinterAdmin            = 0x00000004,
        PrinterUse              = 0x00000008,
        JobAdmin                = 0x00000010,
        JobRead                 = 0x00000020,
        StandardRightsRequired  = 0x000f0000,
        PrinterAllAccess        = (StandardRightsRequired | PrinterAdmin | PrinterUse)
    }

    [Flags]
    public enum PRINTER_STATUS
    {
        PRINTER_STATUS_PAUSED = 0x00000001,
        PRINTER_STATUS_ERROR = 0x00000002,
        PRINTER_STATUS_PENDING_DELETION = 0x00000004,
        PRINTER_STATUS_PAPER_JAM = 0x00000008,
        PRINTER_STATUS_PAPER_OUT = 0x00000010,
        PRINTER_STATUS_MANUAL_FEED = 0x00000020,
        PRINTER_STATUS_PAPER_PROBLEM = 0x00000040,
        PRINTER_STATUS_OFFLINE = 0x00000080,
        PRINTER_STATUS_IO_ACTIVE = 0x00000100,
        PRINTER_STATUS_BUSY = 0x00000200,
        PRINTER_STATUS_PRINTING = 0x00000400,
        PRINTER_STATUS_OUTPUT_BIN_FULL = 0x00000800,
        PRINTER_STATUS_NOT_AVAILABLE = 0x00001000,
        PRINTER_STATUS_WAITING = 0x00002000,
        PRINTER_STATUS_PROCESSING = 0x00004000,
        PRINTER_STATUS_INITIALIZING = 0x00008000,
        PRINTER_STATUS_WARMING_UP = 0x00010000,
        PRINTER_STATUS_TONER_LOW = 0x00020000,
        PRINTER_STATUS_NO_TONER = 0x00040000,
        PRINTER_STATUS_PAGE_PUNT = 0x00080000,
        PRINTER_STATUS_USER_INTERVENTION = 0x00100000,
        PRINTER_STATUS_OUT_OF_MEMORY = 0x00200000,
        PRINTER_STATUS_DOOR_OPEN = 0x00400000,
        PRINTER_STATUS_SERVER_UNKNOWN = 0x00800000,
        PRINTER_STATUS_POWER_SAVE = 0x01000000,
        PRINTER_STATUS_SERVER_OFFLINE = 0x02000000,
        PRINTER_STATUS_DRIVER_UPDATE_NEEDED = 0x04000000
    }

    public enum JOB_PRIORITY
    {
        NO_PRIORITY = 0,
        MIN_PRIORITY = 1,
        DEF_PRIORITY = 1,
        MAX_PRIORITY = 99
    }
}
