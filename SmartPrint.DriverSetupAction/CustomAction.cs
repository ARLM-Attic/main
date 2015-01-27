using System;
using Microsoft.Deployment.WindowsInstaller;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;
using System.Security.Principal;
using System.Security.AccessControl;

namespace SmartPrint.DriverSetupAction
{
    public class CustomActions
    {
        [CustomAction]
        public static ActionResult GetPrinterDriverDir(Session session)
        {
            session.Log("Custom action GetDriverDirectoryAction - Start");
            try
            {
                session["DRIVER_PATH"] = PrintDriver.GetPrinterDriverDirectory();
            }
            catch (Exception ex)
            {
                session.Log("Custom action GetDriverDirectory - Exception: " + ex.Message);
                session.Log("Custom action GetDriverDirectory - Exit (Failure)");
                return ActionResult.Failure;
            }
            session.Log("Custom action GetDriverDirectory - Exit (Success)");
            return ActionResult.Success;
        }

        [CustomAction]
        public static ActionResult InstallSmartPrint(Session session)
        {
            session.Log("Custom action InstallPrinter - Start");

            try
            {
                PrintDriver.Install();
                PrintMonitor.Install();
                Spool.Restart();
            }
            catch (Exception ex)
            {
                session.Log("Custom action InstallPrinter - Exception: " + ex.Message);
                session.Log("Custom action InstallPrinter - Exit (Failure)");
                return ActionResult.Failure;
            }
            session.Log("Custom action InstallPrinter - Exit (Success)");
            return ActionResult.Success;
        }

        [CustomAction]
        public static ActionResult UninstallSmartPrint(Session session)
        {
            session.Log("Custom action UninstallPrinter - Start");

            try
            {
                //  TODO: Find all SmartPrintDevices installed locally and delete them
                // Deleting PrintMonitor will remove all ports of that PrintMonitor type
                PrintMonitor.Uninstall();
                PrintDriver.Uninstall();
                Spool.Restart();
            }
            catch (Exception ex)
            {
                session.Log("Custom action UninstallPrinter - Exception: " + ex.Message);
                session.Log("Custom action UninstallPrinter - Exit (Failure)");
            }
            session.Log("Custom action UninstallPrinter - Exit (Success)");
            return ActionResult.Success;
        }

        [CustomAction]
        public static ActionResult RestartSpoolService(Session session)
        {
            session.Log("Custom action RestartSpoolService - Start");
            try
            {
                Spool.Restart();
            }
            catch (Exception ex)
            {
                session.Log("Custom action RestartSpoolService - Exception: " + ex.Message);
                session.Log("Custom action RestartSpoolService - Exit (Failure)");
            }
            session.Log("Custom action RestartSpoolService - Exit (Success)");
            return ActionResult.Success;
        }

        [CustomAction]
        public static ActionResult KillTrayApp(Session session)
        {
            session.Log("Custom action KillTrayApp - Start");
            try
            {
                Process[] processes = Process.GetProcessesByName("SmartPrinter.UI.exe");
                foreach (Process proc in processes) proc.Kill();
            }
            catch (Exception ex)
            {
                session.Log("Custom action KillTrayApp - Exception: " + ex.Message);
                session.Log("Custom action KillTrayApp - Exit (Failure)");
                return ActionResult.Failure;
            }
            session.Log("Custom action KillTrayApp - Exit (Success)");
            return ActionResult.Success;
        }

        [CustomAction]
        public static ActionResult SetRegistryPermissions(Session session)
        {
            session.Log("Custom action SetRegistryPermissions - Start");
            try
            {
                // HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print\Monitors\SMARTPRINTER
                using (var rootKey = Registry.LocalMachine.OpenSubKey(PrintMonitor.DEFAULT_REGISTRY_KEY, RegistryKeyPermissionCheck.ReadWriteSubTree))
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
            catch
            {
                session.Log("Custom action SetRegistryPermissions - Exit (Failure)");
                return ActionResult.Failure;
            }
            session.Log("Custom action SetRegistryPermissions - Exit (Success)");
            return ActionResult.Success;
        }

    }
}