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

            l.Add(new Printer { Name = "Design time printer 1", Description = "desc 1" });
            l.Add(new Printer { Name = "Design time printer 2", Description = "desc 2" });

            return l;
        }
    }
}