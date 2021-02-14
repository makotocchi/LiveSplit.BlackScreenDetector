using System.Xml;

namespace LiveSplit.Mgs3LoadRemover
{
    public static class XmlHelper
    {
        public static XmlElement ToElement<T>(XmlDocument document, string name, T value)
        {
            var element = document.CreateElement(name);
            element.InnerText = value.ToString();
            return element;
        }
    }
}
