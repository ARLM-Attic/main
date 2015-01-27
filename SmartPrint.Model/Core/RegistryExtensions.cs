using System;
using Microsoft.Win32;

namespace SmartPrint
{
    public static class RegistryExtensions
    {
        private const string PrintersRegKey = "System\\CurrentControlSet\\Control\\Print\\Printers";
        private const string MonitorsRegKey = "System\\CurrentControlSet\\Control\\Print\\Monitors\\SMARTPRINTER";

        public static Guid GetPrinterId(string name)
        {
            RegistryKey rootKey = Registry.LocalMachine.OpenSubKey(PrintersRegKey, false);

            foreach (var subKeyName in rootKey.GetSubKeyNames())
            {
                var subKey = rootKey.OpenSubKey(subKeyName);

                if (subKey != null)
                    if ((string)subKey.GetValue("Name") == name)
                        return GetPrinterId(subKey);
            }

            return Guid.Empty;
        }

        public static string GetPrinterName(Guid id)
        {
            RegistryKey rootKey = Registry.LocalMachine.OpenSubKey(PrintersRegKey, false);

            foreach (var subKeyName in rootKey.GetSubKeyNames())
            {
                var subKey = rootKey.OpenSubKey(subKeyName);

                if (subKey != null && GetPrinterId(subKey) == id)
                    return (string)subKey.GetValue("Name");
            }


            return String.Empty;
        }

        public static string GetPrinterOutputPath(Guid id)
        {
            var rootKey = Registry.LocalMachine.OpenSubKey(PrintersRegKey, false);

            foreach (var subKeyName in rootKey.GetSubKeyNames())
            {
                var subKey = rootKey.OpenSubKey(subKeyName);

                if (subKey != null && GetPrinterId(subKey) == id)
                {
                    var openSubKey = Registry.LocalMachine.OpenSubKey(String.Format("{0}\\{1}", MonitorsRegKey, subKey.GetValue("Port")), false);

                    if (openSubKey != null)
                    {
                        var val = (string) openSubKey.GetValue("OutputPath");
                        val = val.Replace("\0\\", String.Empty);
                        return val;
                    }
                }
            }

            return String.Empty;
        }

        private static Guid GetPrinterId(RegistryKey key)
        {
            return
                new Guid(((string)key.GetValue("DeviceInterfaceId"))
                    .Replace("\\\\?\\SWD#PRINTENUM#", String.Empty)
                    .Substring(0, 38));
        }
    }
}