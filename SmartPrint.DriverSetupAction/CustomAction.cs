using System;
using System.Windows.Forms;
using Microsoft.Deployment.WindowsInstaller;

namespace SmartPrint.DriverSetupAction
{
    public class CustomActions
    {
        [CustomAction]
        public static ActionResult DriverAction(Session session)
        {
            session.Log("Begin DriverAction");

            try
            {
                var driverInstaller = new DriverInstaller();

            DriverInstaller.GenericResult result = 
                    driverInstaller.CreatePrinter("Virtual SmartPrinter");

            

                if (!result.Success)
                {
                    return ActionResult.Failure;
                    // Driver is not installed
                }
            }
            catch (Exception)
            {
                
                throw;
            }

            return ActionResult.Success;
        }
    }
}
