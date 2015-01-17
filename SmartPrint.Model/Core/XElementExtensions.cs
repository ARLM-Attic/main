using System.Xml.Linq;

namespace SmartPrint
{
    public static class XElementExtensions
    {
        public static void Write(this XElement element, string name, object value)
        {
            var nameAttribute = element.Attribute(name);
            
            if (nameAttribute != null)
                nameAttribute.SetValue(value);
            else
                element.Add(new XAttribute(name, value));
        }

        public static string Read(this XElement element, string attributeName)
        {
            var xAttribute = element.Attribute(attributeName);

            return xAttribute != null ? xAttribute.Value : null;
        }
    }
}