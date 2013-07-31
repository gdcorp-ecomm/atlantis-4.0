
using System.Xml.Linq;

namespace Atlantis.Framework.Brand.Interface
{
  public class CompanyName
  {
    public string Name { get; private set; }
    public string Value { get; private set; }

    private CompanyName() { }

    public static CompanyName FromCacheXml(XElement companyNameElement)
    {
      CompanyName result = new CompanyName();

      result.Name = GetValue(companyNameElement, "Name");
      result.Value = GetValue(companyNameElement, "Value");

      return result;
    }

    public static CompanyName FromDataCache(string name, string value)
    {
      CompanyName result = new CompanyName();

      result.Name = name;
      result.Value = value;

      return result;
    }

    private static string GetValue(XElement element, string attributeName)
    {
      string result = string.Empty;

      XAttribute attribute = element.Attribute(attributeName);
      if (attribute != null)
      {
        result = attribute.Value;
      }
      return result;
    }
  }
}
