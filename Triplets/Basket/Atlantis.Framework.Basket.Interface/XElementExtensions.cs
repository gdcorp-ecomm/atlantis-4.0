using System.Xml.Linq;

namespace Atlantis.Framework.Basket.Interface
{
  internal static class XElementExtensions
  {
    internal static string ChildElementValue(this XElement element, string childElementName, string defaultValue = "")
    {
      string result = defaultValue;

      var childElement = element.Element(childElementName);
      if (childElement != null)
      {
        result = childElement.Value;
      }

      return result;
    }
  }
}
