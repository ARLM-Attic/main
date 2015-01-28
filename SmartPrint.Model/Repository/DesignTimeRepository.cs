using System;
using System.Collections.Generic;

namespace SmartPrint.Model.Repository
{
    public class DesignTimeRepository : IRepository
    {
        public void SavePrinter(Printer printer)
        {
            throw new System.NotImplementedException();
        }

        public List<Printer> LoadPrinters()
        {
            var l = new List<Printer>();

            var p = new Printer { Name = "Design time printer 1", Description = "desc 1", OutputPath = "C:\\PrinterTest" };
            p.Actions.Add(new FileShareAction());

            l.Add(p);

            l.Add(new Printer { Name = "Design time printer 2", Description = "desc 2", OutputPath = "" });

            l.Add(new Printer { Name = "Another Design time printer", Description = "desc 2", OutputPath = "" });

            return l;
        }

        public void DeletePrinter(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}