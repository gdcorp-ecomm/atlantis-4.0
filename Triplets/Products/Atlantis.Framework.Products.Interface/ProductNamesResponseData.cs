using Atlantis.Framework.Interface;
using System.Xml.Linq;

namespace Atlantis.Framework.Products.Interface
{
  public class ProductNamesResponseData : IResponseData
  {
    public static ProductNamesResponseData Empty { get; private set; }

    static ProductNamesResponseData()
    {
      Empty = new ProductNamesResponseData(string.Empty, string.Empty);
    }

    public string Name { get; private set; }
    public string FriendlyName { get; private set; }

    public static ProductNamesResponseData FromServiceData(string xmlData)
    {
      //<LocaleData name=".COM Domain Name Registration - 1 Ano (recorrente)" description2=".COM Registo de Domínios"/>

      var element = XElement.Parse(xmlData);
      var name = element.Attribute("name").Value;
      var friendlyName = element.Attribute("description2").Value;

      return new ProductNamesResponseData(name, friendlyName);
    }

    private ProductNamesResponseData(string name, string friendlyName)
    {
      Name = name;
      FriendlyName = string.IsNullOrEmpty(friendlyName) ? Name : friendlyName;
    }

    public string ToXML()
    {
      var element = new XElement(GetType().Name);
      element.Add(
        new XAttribute("name", Name),
        new XAttribute("friendlyname", FriendlyName));

      return element.ToString(SaveOptions.DisableFormatting);
    }

    public AtlantisException GetException()
    {
      return null;
    }
  }
}
