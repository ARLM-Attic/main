using System.Collections.Generic;

namespace SmartPrint.Model.Repository
{
    public interface IRepository
    {
        void SavePrinter(Printer printer);

        List<Printer> LoadPrinters();
    }
}