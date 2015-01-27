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

        private const string PrintersRegKey = "System\\CurrentControlSet\\Control\\Print\\Printers";
        private const string MonitorsRegKey = "System\\CurrentControlSet\\Control\\Print\\Monitors\\SMARTPRINTER";

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
            var id = RegistryExtensions.GetPrinterId("new printer");
            Assert.AreNotEqual(id, Guid.Empty);
        }

        [TestMethod]
        public void GetPrinterNameTest()
        {
            const string name = "new printer";
            
            var id = RegistryExtensions.GetPrinterId(name);
            
            var getName = RegistryExtensions.GetPrinterName(id);
            
            Assert.AreEqual(name, getName);
        }

        [TestMethod]
        public void GetPrinterPathTest()
        {
            var id = RegistryExtensions.GetPrinterId("new printer");
            
            var path = RegistryExtensions.GetPrinterOutputPath(id);

            Assert.IsNotNull(path);

            Assert.AreNotEqual(String.Empty, path);
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


    }
}