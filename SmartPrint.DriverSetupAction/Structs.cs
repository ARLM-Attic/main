﻿using System;
using System.Runtime.InteropServices;

namespace SmartPrint.DriverSetupAction
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct PRINTER_INFO_2
    {
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pServerName;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pPrinterName;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pShareName;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pPortName;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pDriverName;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pComment;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pLocation;
        public IntPtr pDevMode;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pSepFile;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pPrintProcessor;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pDatatype;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pParameters;
        public IntPtr pSecurityDescriptor;
        public PRINTER_ATTRIBUTES Attributes;
        public JOB_PRIORITY Priority;
        public JOB_PRIORITY DefaultPriority;
        public uint StartTime;
        public uint UntilTime;
        public PRINTER_STATUS Status;
        public uint cJobs;
        public uint AveragePPM;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct PRINTER_INFO_4
    {
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pPrinterName;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pServerName;
        public PRINTER_ATTRIBUTES Attributes;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PrinterDefaults
    {
        public IntPtr pDataType;
        public IntPtr pDevMode;
        public PRINTER_ACCESS DesiredAccess;
    }

}

