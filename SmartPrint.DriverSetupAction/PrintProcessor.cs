using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartPrint.DriverSetupAction
{
    public class PrintProcessor
    {
        public const string DEFAULT_NAME = "WinPrint";
        public const string DEFAULT_DATATYPE = "RAW";
        public const string DEFAULT_PARAMETERS = "";

        private string _name = DEFAULT_NAME;
        private string _datatype = DEFAULT_DATATYPE;
        private string _parameters = DEFAULT_PARAMETERS;

        public string Name
        {
            get { return _name; }
            set { if (!string.IsNullOrEmpty(value)) _name = value; }
        }
        public string DataType
        {
            get { return _datatype; }
            set { if (!string.IsNullOrEmpty(value)) _datatype = value; }
        }
        public string Parameters
        {
            get { return _parameters; }
            set { if (!string.IsNullOrEmpty(value)) _parameters = value; }

        }

        public PrintProcessor() : this(DEFAULT_NAME, DEFAULT_DATATYPE, DEFAULT_PARAMETERS) { }
        public PrintProcessor(string name, string dataType, string parameters)
        {
            Name = name;
            DataType = dataType;
            Parameters = parameters;
        }

        public static PrintProcessor Default
        {
            get { return new PrintProcessor(); }
        }
    }
}
