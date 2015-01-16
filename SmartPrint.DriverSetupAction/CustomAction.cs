using System;
using Microsoft.Deployment.WindowsInstaller;
using System.IO;
using System.Diagnostics;

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
                session["DRIVER_PATH"] = DriverInstaller.GetPrinterDriverDirectory();
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
        public static ActionResult InstallPrinter(Session session)
        {
            session.Log("Custom action InstallPrinter - Start");

            try
            {
                var driverDir = DriverInstaller.GetPrinterDriverDirectory();

                PrinterSettings printer = new PrinterSettings(
                  "Virtual SmartPrinter",               // printerName
                  session.CustomActionData["AppPath"],  // appPath
                  "SMARTPRINTER",                       // monitorName
                  "mfilemon.dll",                       // monitorDllName
                  @"Virtual SmartPrinter:",             // portName
                  "SMARTPRINTER",                       // description
                  new PrinterDriverSettings(
                      "SMARTPRINTER",                   // driverName
                      "PSCRIPT5.DLL",                   // driverFilename
                      "PS5UI.DLL",                      // configFilename
                      "SMARTPRINTER.PPD",               // dataFilename
                      "PSCRIPT.HLP",                    // helpFilename
                      driverDir                         // driverDir
                  ));

                if (!DriverInstaller.AddVSmartPrinter(printer.PrinterName, printer.Description))
                {
                    session.Log("Custom action InstallPrinter - AddVPrinter returned false.");
                    session.Log("Custom action InstallPrinter - Exit (Failure)");
                    return ActionResult.Failure;
                }
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
        public static ActionResult UninstallPrinter(Session session)
        {
            session.Log("Custom action UninstallPrinter - Start");

            DriverInstaller driverInstaller = new DriverInstaller();
            PrinterSettings printer = new PrinterSettings(
                  "Virtual SmartPrinter",         // printerName
                  @"C:\SmartPrinter",             // appPath
                  "SMARTPRINTER",                 // monitorName
                  "mfilemon.dll",                 // monitorDllName
                  @"Virtual SmartPrinter:",       // portName
                  "SMARTPRINTER",                 // description
                  new PrinterDriverSettings(
                      "SMARTPRINTER",             // driverName
                      "PSCRIPT5.DLL",             // driverFilename
                      "PS5UI.DLL",                // configFilename
                      "SMARTPRINTER.PPD",         // dataFilename
                      "PSCRIPT.HLP",              // helpFilename
                      ""                          // driverDir
            ));

            try
            {
                if (!driverInstaller.DeleteVPrinter(printer))
                {
                    session.Log("Custom action UninstallPrinter - DeleteVprinter returned false.");
                    session.Log("Custom action UninstallPrinter - Exit (Failure)");
                    return ActionResult.Failure;
                }
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
            var driverInstaller = new DriverInstaller();
            try
            {
                if (!DriverInstaller.RestartSpoolService())
                {
                    session.Log("Custom action RestartSpoolService - RestartSpoolService returned false.");
                    session.Log("Custom action RestartSpoolService - Exit (Failure)");
                    return ActionResult.Failure;
                }
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
    }
}