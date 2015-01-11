using System;
using Microsoft.Deployment.WindowsInstaller;
using System.IO;

namespace SmartPrint.DriverSetupAction
{
    public class CustomActions
    {
        [CustomAction]
        public static ActionResult GetDriverDirectory(Session session)
        {
            session.Log("Custom action GetDriverDirectoryAction - Start");
            try
            {
                var driverInstaller = new DriverInstaller();
                session["DRIVER_PATH"] = driverInstaller.GetPrinterDirectory();
                
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
        public static ActionResult DriverAction(Session session)
        {
            session.Log("Custom action DriverAction - Start");

            try
            {
                var driverInstaller = new DriverInstaller();
                var driverDir = driverInstaller.GetPrinterDirectory();

                PrinterSettings printer = new PrinterSettings(
                  "Virtual SmartPrinter",         // printerName
                  @"C:\SmartPrinter",             // appPath
                  "SMARTPRINTER",                 // monitorName
                  "mfilemon.dll",                 // monitorDllName
                  @"Virtual SmartPrinter:",       // portName
                  new PrinterDriverSettings(
                      "SMARTPRINTER",             // driverName
                      "PSCRIPT5.DLL",             // driverFilename
                      "PS5UI.DLL",                // configFilename
                      "SMARTPRINTER.PPD",         // dataFilename
                      "PSCRIPT.HLP",              // helpFilename
                      driverDir                  // driverDir
                  ));

                if (!driverInstaller.AddVPrinter(printer))
                {
                    session.Log("Custom action DriverAction - AddVPrinter returned false.");
                    session.Log("Custom action DriverAction - Exit (Failure)");
                    return ActionResult.Failure;
                }
            }
            catch (Exception ex)
            {
                session.Log("Custom action DriverAction - Exception: " + ex.Message);
                session.Log("Custom action DriverAction - Exit (Failure)");
                return ActionResult.Failure;
            }
            session.Log("Custom action DriverAction - Exit (Success)");
            return ActionResult.Success;
        }
        [CustomAction]
        public static ActionResult RemoveDriver(Session session)
        {
            session.Log("Custom action DriverRollback - Start");

            DriverInstaller driverInstaller = new DriverInstaller();
            PrinterSettings printer = new PrinterSettings(
                  "Virtual SmartPrinter",         // printerName
                  @"C:\SmartPrinter",             // appPath
                  "SMARTPRINTER",                 // monitorName
                  "mfilemon.dll",                 // monitorDllName
                  @"Virtual SmartPrinter:",       // portName
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
                    session.Log("Custom action DriverRollback - DeleteVprinter returned false.");
                    session.Log("Custom action DriverRollback - Exit (Failure)");
                    return ActionResult.Failure;
                }
            }
            catch (Exception ex)
            {
                session.Log("Custom action DriverRollback - Exception: " + ex.Message);
                session.Log("Custom action DriverRollback - Exit (Failure)");
            }
            session.Log("Custom action DriverRollback - Exit (Success)");
            return ActionResult.Success;
        }
    }
}