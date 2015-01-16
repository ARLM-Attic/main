namespace SmartPrint.Model
{
    public class EmailAction : PdfAction
    {
        public string ToAddress { get; set; }

        public string FromAddress { get; set; }

        public string Subject { get; set; }
    }
}