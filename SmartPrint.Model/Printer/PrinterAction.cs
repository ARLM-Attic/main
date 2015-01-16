using System;

namespace SmartPrint.Model
{
    public abstract class PrinterAction
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
