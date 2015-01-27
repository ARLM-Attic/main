namespace SmartPrint.Model
{
    public class EmailAction : PdfAction
    {
        public string ToAddress { get; set; }

        public string FromAddress { get; set; }

        public string Subject { get; set; }

        protected override void ExecutePdfAction()
        {
            // Save byte array as file, attach it to email and send it
        }
    }
}