using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SmartPrint.Model
{
    public class FileShareAction : PdfAction
    {
        public string FileName { get; set; }

        public string Location { get; set; }

        protected override void ExecutePdfAction()
        {
            if (String.IsNullOrEmpty(Location))
                throw new ArgumentException("Location for file share action must be set");

            if (String.IsNullOrEmpty(FileName))
                throw new ArgumentException("File name for file share action must be set");

            File.WriteAllBytes(Path.Combine(Location, FileName), PdfBytes);
        }
    }
}
