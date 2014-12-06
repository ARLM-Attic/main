using System.ServiceProcess;

namespace SmartPrint.WindowsService
{
    static class Program
    {
        static void Main()
        {
            ServiceBase[] servicesToRun = { new SmartPrintService() };
            ServiceBase.Run(servicesToRun);
        }
    }
}