using System.Xml.Linq;

namespace Atlantis.Framework.ManagerUser.Interface
{
  internal static class XmlExtensions
  {
    public static string GetAttributeValue(this XElement element, string attributeName)
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
