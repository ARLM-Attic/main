using System.Management;
using System;
using System.Drawing.Printing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Win32;
using System.Security.Principal;
using System.Security.AccessControl;

namespace SmartPrint.Tests
{
    [TestClass]
    public class PrinterApiTests
    {

        public const string PrintersRegKey = "System\\CurrentControlSet\\Control\\Print\\Printers";
        public const string MonitorsRegKey = "System\\CurrentControlSet\\Control\\Print\\Monitors\\SMARTPRINTER";

        [TestMethod]
        public void GetPrinters()
        {
            var printers = PrinterSettings.InstalledPrinters;
        }

        [TestMethod]
        public void SetRegistryPermissions()
        {
            // HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print\Monitors\SMARTPRINTER
            using (var rootKey = Registry.LocalMachine.OpenSubKey(MonitorsRegKey, RegistryKeyPermissionCheck.ReadWriteSubTree))
            {
                var sid = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
                
                var registrySecurity = rootKey.GetAccessControl();

                var rar = new RegistryAccessRule(
                    (sid.Translate(typeof(NTAccount)) as NTAccount).ToString(), 
                    RegistryRights.FullControl,
                    InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
                    PropagationFlags.None,
                    AccessControlType.Allow);

                registrySecurity.AddAccessRule(rar);
               
                rootKey.SetAccessControl(registrySecurity);
            }
        }

        [TestMethod]
        public void GetPrinterIdTest()
        {
            var id = GetPrinterIdFromRegistry("Microsoft XPS Document Writer");

            var getName = GetPrinterNameFromRegistry(id);
        }

        [TestMethod]
        public void GetPrintersWmi()
        {
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


        public static Guid GetPrinterIdFromRegistry(string name)
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

        public static string GetPrinterNameFromRegistry(Guid id)
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

        public static Guid GetPrinterId(RegistryKey key)
        {
            return
                new Guid(((string)key.GetValue("DeviceInterfaceId"))
                    .Replace("\\\\?\\SWD#PRINTENUM#", String.Empty)
                    .Substring(0, 38));
        }
    }
}