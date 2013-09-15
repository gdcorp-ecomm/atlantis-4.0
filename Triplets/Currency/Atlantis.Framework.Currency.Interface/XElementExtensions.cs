using System.Xml.Linq;

namespace Atlantis.Framework.Currency.Interface
{
  internal static class XElementExtensions
  {
    public static string LookupAttribute(this XElement element, string name, string defaultValue)
    {
      string result = defaultValue;
      XAttribute attribute = element.Attribute(name);
      if (attribute != null)
      {
        result = attribute.Value;
      }
      return result;
    }

    public static string TrimAttribute(this XElement element, string name, string defaultValue)
    {
      string result = defaultValue;
      XAttribute attribute = element.Attribute(name);
      if (attribute != null)
      {
        result = attribute.Value.Trim();
      }
      return result;
    }

    public static double LookupDoubleAttribute(this XElement element, string name, double defaultValue)
    {
      double result = defaultValue;
      XAttribute attribute = element.Attribute(name);
      if (attribute != null)
      {
        string resultText = attribute.Value;
        if (!double.TryParse(resultText, out result))
        {
          result = defaultValue;
        }
      }
      return result;
    }

    public static int LookupIntAttribute(this XElement element, string name, int defaultValue)
    {
      int result = defaultValue;
      XAttribute attribute = element.Attribute(name);
      if (attribute != null)
      {
        string resultText = attribute.Value;
        if (!int.TryParse(resultText, out result))
        {
          result = defaultValue;
        }
      }
      return result;
    }

  }
}
