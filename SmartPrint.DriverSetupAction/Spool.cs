using System;
using System.ServiceProcess;

namespace SmartPrint.DriverSetupAction
{
    public static class Spool
    {
        public static void Restart()
        {
            try
            {
                ServiceController sc = new ServiceController("Spooler");
                if (sc.Status != ServiceControllerStatus.Stopped ||
                    sc.Status != ServiceControllerStatus.StopPending)
                    sc.Stop();
                sc.WaitForStatus(ServiceControllerStatus.Stopped);
                sc.Start();
            }
            catch { throw; }
        }
    }
}
