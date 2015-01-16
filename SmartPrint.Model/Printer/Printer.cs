﻿using System;
using System.Collections.Generic;

namespace SmartPrint.Model
{
    public class Printer
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }

        public string Description { get; set; }

        public List<PrinterAction> Actions { get; set; }
    }
}
