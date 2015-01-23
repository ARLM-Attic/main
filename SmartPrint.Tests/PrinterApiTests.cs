using System.Management;
using System;
using System.Drawing.Printing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Win32;

namespace SmartPrint.Tests
{
    [TestClass]
    public class PrinterApiTests
    {
        [TestMethod]
        public void GetPrinters()
        {
            var printers = PrinterSettings.InstalledPrinters;
        }

        [TestMethod]
        public void GetPrintersWmi()
        {
            string printerName = "YourPrinterName";
            string query = string.Format("SELECT * from Win32_Printer");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            ManagementObjectCollection coll = searcher.Get();

            foreach (ManagementObject printer in coll)
            {
                foreach (PropertyData property in printer.Properties)
                {
                    Console.WriteLine(string.Format("{0}: {1}", property.Name, property.Value));
                }
            }
        }

        [TestMethod]
        public void GetPrinterIdTest()
        {
            var id = GetPrinterIdFromRegistry("Microsoft XPS Document Writer");

            var getName = GetPrinterNameFromRegistry(id);
        }

        public const string PrintersRegKey = "System\\CurrentControlSet\\Control\\Print\\Printers";

        public static Guid GetPrinterIdFromRegistry(string name)
        {
            RegistryKey rootKey = Registry.LocalMachine.OpenSubKey(PrintersRegKey, false);

            foreach (var subKeyName in rootKey.GetSubKeyNames())
            {
                var subKey = rootKey.OpenSubKey(subKeyName);

                if (subKey != null)
                    if ((string)subKey.GetValue("Name") == name)
                        return new Guid(((string)subKey.GetValue("DeviceInterfaceId")).Replace("\\\\?\\SWD#PRINTENUM#", String.Empty).Substring(0, 38));
            }

            return Guid.Empty;
        }

        public static string GetPrinterNameFromRegistry(Guid id)
        {
            RegistryKey rootKey = Registry.LocalMachine.OpenSubKey(PrintersRegKey, false);

            foreach (var subKeyName in rootKey.GetSubKeyNames())
            {
                var subKey = rootKey.OpenSubKey(subKeyName);

                if (subKey != null)
                    if (new Guid(((string)subKey.GetValue("DeviceInterfaceId")).Replace("\\\\?\\SWD#PRINTENUM#", String.Empty).Substring(0, 38)) == id)
                        return (string)subKey.GetValue("Name");
            }


            return String.Empty;
        }

    }
}
