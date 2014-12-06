using System.ServiceProcess;

namespace SmartPrint.WindowsService
{
    public partial class SmartPrintService : ServiceBase
    {
        private readonly PrintPlusPlusConnector _pppConnector = new PrintPlusPlusConnector();

        public SmartPrintService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _pppConnector.Start();
        }

        protected override void OnStop()
        {
            _pppConnector.Stop();
        }
    }
}