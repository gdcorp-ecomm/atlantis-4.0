using System.Xml.Linq;

namespace Atlantis.Framework.Brand.Interface
{
  public class ProductLineName
  {
    public string Id { get; private set; }
    public string Value { get; private set; }
    public string GDValue { get; private set; }

    private ProductLineName() { }

    public static ProductLineName FromCacheXml(XElement productLineNameElement)
    {
      ProductLineName result = new ProductLineName();

      result.Id = GetValue(productLineNameElement, "Id");
      result.Value = GetValue(productLineNameElement, "Value");
      result.GDValue = GetValue(productLineNameElement, "GDValue");

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
