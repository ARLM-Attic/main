using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace SmartPrint.Model.Repository
{
    public class XmlRepository : IRepository
    {
        #region Constants and fields 

        private const string XmlFilePath = "Printers.xml";

        private readonly XDocument _printersXDoc;

        private readonly string _filePath;

        #endregion

        #region Constructor

        public XmlRepository(string dir)
        {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            _filePath = Path.Combine(dir, XmlFilePath);

            if (!File.Exists(_filePath))
            {
                _printersXDoc = new XDocument();
                
                var root = new XElement("SMARTdoc");

                var printers = new XElement("Printers");

                root.Add(printers);

                _printersXDoc.Add(root);

                _printersXDoc.Save(_filePath);
            }
            else
                _printersXDoc = XDocument.Load(_filePath);

        }

        #endregion

        #region Properties

        private XElement PrintersElement { get { return _printersXDoc.Element("SMARTdoc").Element("Printers"); } }

        #endregion

        public void SavePrinter(Printer printer)
        {
            var xmlPrinter = PrintersElement.Elements().FirstOrDefault(x => printer.Id == new Guid(x.Read("PrinterId")));

            if (xmlPrinter == null)
            {
                xmlPrinter = new XElement("Printer");
                xmlPrinter.Write("PrinterId", printer.Id);
                PrintersElement.Add(xmlPrinter);
            }

            xmlPrinter.Write("Name", printer.Name);
            xmlPrinter.Write("Description", printer.Description);

            _printersXDoc.Save(_filePath);
        }

        public List<Printer> LoadPrinters()
        {
            var printers = new List<Printer>();

            foreach (var xmlPrinter in PrintersElement.Elements())
            {
                var p = new Printer();
                p.Id = new Guid(xmlPrinter.Read("PrinterId"));
                p.Name = xmlPrinter.Read("Name");
                p.Description = xmlPrinter.Read("Description");
                p.OutputPath = RegistryExtensions.GetPrinterOutputPath(RegistryExtensions.GetPrinterId(p.Name));
               
                printers.Add(p);
            }

            return printers;
        }

        public void DeletePrinter(Guid id)
        {
            var xmlPrinter = PrintersElement.Elements().FirstOrDefault(x => id == new Guid(x.Read("PrinterId")));

            if (xmlPrinter == null) return;
            
            xmlPrinter.Remove();
                
            _printersXDoc.Save(_filePath);
        }
    }
}
