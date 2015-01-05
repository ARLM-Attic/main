using System;
using Microsoft.Deployment.WindowsInstaller;

namespace SmartPrint.DriverSetupAction
{
    public class CustomActions
    {
        [CustomAction]
        public static ActionResult DriverAction(Session session)
        {
            session.Log("Begin DriverAction");

            var driverInstaller = new DriverInstaller();

            var driversDir = driverInstaller.GetPrinterDirectory();

            PrinterSettings printer = new PrinterSettings(
              "Virtual SmartPrinter",         // printerName
              @"C:\SmartPrinter",             // appPath
              "SMARTPRINTER",                 // monitorName
              "mfilemon.dll",                 // monitorDllName
              @"Virtual SmartPrinter:",       // portName
                // i u njoj klasa za podatke o driveru
              new PrinterDriverSettings(
                  "SMARTPRINTER",             // driverName
                  "PSCRIPT5.DLL",             // driverFilename
                  "PS5UI.DLL",                // configFilename
                  "SMARTPRINTER.PPD",         // dataFilename
                  "PSCRIPT.HLP",              // helpFilename
                  driversDir
              ));

            driverInstaller.AddVPrinter(printer);

            //   DriverInstaller.GenericResult result = 
            //    driverInstaller.CreatePrinter("Virtual SmartPrinter");



            //if (!result.Success)
            //{
            //    return ActionResult.Failure;
            //    // Driver is not installed
            //}

            return ActionResult.Success;
        }
    }
}