namespace SmartPrint.Model
{
    public class PhysicalPrinterAction : PrinterAction
    {
        public PrinterConfiguration Configuration { get; set; }

        public override void Execute()
        {
            throw new System.NotImplementedException();
        }
    }
}
