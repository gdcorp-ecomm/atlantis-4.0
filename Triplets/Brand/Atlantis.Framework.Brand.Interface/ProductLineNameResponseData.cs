using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Brand.Interface
{
  public class ProductLineNameResponseData : IResponseData
  {
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

    public string GetName(string productLineKey, int contextId)
    {
      string productLineName = String.Empty;

      Dictionary<string, string> productLineValueDict;

      ProductLineNames.TryGetValue(productLineKey, out productLineValueDict);

      if (productLineValueDict != null && productLineValueDict.Count > 0)
      {
        if (contextId != 0)
        {
          productLineValueDict.TryGetValue("override", out productLineName);

          if (String.IsNullOrEmpty(productLineName))
          {
            productLineValueDict.TryGetValue("default", out productLineName);
          }
        }

        else
        {
          productLineValueDict.TryGetValue("default", out productLineName);
        }
      }

      return productLineName ?? String.Empty;
    }

    private ProductLineNameResponseData(Dictionary<string, Dictionary<string, string>> productLineList)
    {
      _productNameDict = productLineList;
    }

    public AtlantisException GetException()
    {
      return null;
    }

    public string ToXML()
    {
      XElement element = new XElement("ProductLineNameResponseData");
      return element.ToString(SaveOptions.DisableFormatting);
    }
  }
}