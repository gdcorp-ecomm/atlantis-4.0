using System.Xml.Linq;

namespace Atlantis.Framework.Language.Impl.Data
{
  internal static class XElementExtensions
  {
    public static string GetAttributeValue(this XElement element, string attributeName, string defaultValue)
    {
      string result = defaultValue;
      XAttribute attribute = element.Attribute(attributeName);
      if (attribute != null)
      {
        result = attribute.Value;
      }
      return result;
    }

    public static int GetAttributeValueInt(this XElement element, string attributeName, int defaultValue)
    {
      int result = defaultValue;
      XAttribute attribute = element.Attribute(attributeName);
      int parsedResult;
      if ((attribute != null) && (int.TryParse(attribute.Value, out parsedResult)))
      {
        result = parsedResult;
      }
      return result;
    }

  }
}
