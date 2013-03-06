using System.Xml.Linq;

namespace Atlantis.Framework.Geo.Interface
{
  internal static class XElementExtensions
  {
    public static int GetAttributeValueInt(this XElement element, string attributeName, int defaultValue)
    {
      int result = defaultValue;

      XAttribute attribute = element.Attribute(attributeName);
      if (attribute != null)
      {
        int parsedValue;
        if (int.TryParse(attribute.Value, out parsedValue))
        {
          result = parsedValue;
        }
      }

      return result;
    }

    public static string GetAttributeValue(this XElement element, string attributeName, string defaultValue)
    {
      XAttribute attribute = element.Attribute(attributeName);
      if (attribute != null)
      {
        return attribute.Value;
      }
      else
      {
        return defaultValue;
      }
    }
  }
}
