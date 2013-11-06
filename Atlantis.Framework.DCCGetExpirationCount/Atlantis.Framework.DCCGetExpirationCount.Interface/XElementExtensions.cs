using System.Xml.Linq;

namespace Atlantis.Framework.DCCGetExpirationCount.Interface
{
  internal static class XElementExtensions
  {
    public static int GetIntAttribute(this XElement element, string attributeName, int defaultValue)
    {
      var attribute = element.Attribute(attributeName);
      if (attribute != null)
      {
        int parsedValue;
        if (int.TryParse(attribute.Value, out parsedValue))
        {
          return parsedValue;
        }
      }

      return defaultValue;
    }

    public static string GetAttribute(this XElement element, string attributeName, string defaultValue)
    {
      var attribute = element.Attribute(attributeName);
      if (attribute != null)
      {
        return attribute.Value;
      }

      return defaultValue;
    }
  }
}
