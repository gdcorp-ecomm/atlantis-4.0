using System.Collections.Generic;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Brand.Interface
{
  public class ProductLineNameResponseData : IResponseData
  {
    private AtlantisException _exception;
    private Dictionary<string, Dictionary<string, string>> _productNameDict;

    public static ProductLineNameResponseData Empty { get; private set; }

    public static ProductLineNameResponseData FromProductLineNameXml(string productLineXml,  int contextId)
    {
      var productLineNamesDoc = XDocument.Parse(productLineXml);
      var productLineList = new Dictionary<string, Dictionary<string, string>>();

      foreach (var productLineNamesElement in productLineNamesDoc.Descendants("ProductLineName"))
      {
        var productNameDictionary = new Dictionary<string, string>();

        var keyAttribute = productLineNamesElement.Attribute("Key");
        var valueAttribute = productLineNamesElement.Attribute("Value");

        if ((keyAttribute == null) || (valueAttribute == null)) continue;

        var key = keyAttribute.Value;
        var value = valueAttribute.Value;
        productNameDictionary.Add("default", value);

        var overrideProductLine = productLineNamesElement.Descendants("OverrideName");

        if (overrideProductLine.Elements() != null)
        {
          foreach (var overrideElement in overrideProductLine)
          {
            var contextidAttribute = overrideElement.Attribute("ContextId");
            int xmlContextId;

            if (int.TryParse(contextidAttribute.Value, out xmlContextId))
            {
              if (xmlContextId == contextId)
              {
                XAttribute overrideAttribute;
                overrideAttribute = overrideElement.Attribute("Value");

                if (overrideAttribute != null)
                {

                  var ovverideValue = overrideAttribute.Value;
                  productNameDictionary.Add("override", ovverideValue);
                }
              }
            }
          }
        }
        productLineList.Add(key, productNameDictionary);
      }

      return new ProductLineNameResponseData(productLineList);
    }

    static ProductLineNameResponseData()
    {
      Empty = new ProductLineNameResponseData(new Dictionary<string, Dictionary<string, string>>());
    }

    public Dictionary<string, Dictionary<string, string>> ProductLineNames
    {
      get { return _productNameDict; }
    }

    private ProductLineNameResponseData(Dictionary<string, Dictionary<string, string>> productLineList)
    {
      _productNameDict = productLineList;
    }

    public static ProductLineNameResponseData FromException(AtlantisException exception)
    {
      return new ProductLineNameResponseData(exception);
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    private ProductLineNameResponseData(AtlantisException exception)
    {
      _exception = exception;
    }

    public string ToXML()
    {
      XElement element = new XElement("ProductLineNameResponseData");
      return element.ToString(SaveOptions.DisableFormatting);
    }
  }
}