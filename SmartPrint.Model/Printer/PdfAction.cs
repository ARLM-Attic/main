using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartPrint.Model
{
    public class PdfAction : PrinterAction
    {
        public byte[] PdfBytes { get; set; }

        public sealed override void Execute()
        {
            if (PdfBytes == null || PdfBytes.Count() == 0)
                throw new ArgumentException("Pdf bytes must be populated for this command");

            ExecuteCore();
        }

        protected virtual void ExecuteCore()
        {

        }
    }
}
