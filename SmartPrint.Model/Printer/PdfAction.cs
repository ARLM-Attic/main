using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartPrint.Model
{
    public class PdfAction : PrinterAction
    {
        public byte[] PdfBytes { get; set; }
    }
}
