using System.Xml.Linq;

namespace Atlantis.Framework.Products.Interface
{
  internal static class XElementExtensions
  {
    public static int GetAttributeValueInt(this XElement element, string attributeName, int defaultValue)
    {
      int result = defaultValue;

      if ((element != null) && (element.Attribute(attributeName) != null))
      {
        string attributeValue = element.Attribute(attributeName).Value;
        int parsedValue;
        if (int.TryParse(attributeValue, out parsedValue))
        {
          result = parsedValue;
        }
      }

      return result;
    }

    public static string GetAttributeValueString(this XElement element, string attributeName, string defaultValue)
    {
      string result = defaultValue;

      if ((element != null) && (element.Attribute(attributeName) != null))
      {
        result = element.Attribute(attributeName).Value;
      }

      return result;
    }

  }
}
