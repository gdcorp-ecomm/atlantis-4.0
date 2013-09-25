using System.Collections.Generic;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Brand.Interface
{
  public class CompanyNameResponseData : IResponseData
  {
    private Dictionary<string, string> _companyDict;

    public static CompanyNameResponseData Empty { get; private set; }

    public static CompanyNameResponseData FromCompanyNameXml(string companyNameXml, int contextId)
    {
      var companyNamesDoc = XDocument.Parse(companyNameXml);
      var companyDict = new Dictionary<string, string>();

      foreach (var companyNameElement in companyNamesDoc.Descendants("CompanyName"))
      {
        var companyContextId = companyNameElement.Attribute("ContextId");

        if (companyContextId != null)
        {
          int xmlContextId;

          if (int.TryParse(companyContextId.Value, out xmlContextId))
          {
            XAttribute valueAttribute;
            XAttribute keyAttribute;

            if (xmlContextId == 0)
            {
              foreach (var element in companyNameElement.Descendants("Name"))
              {
                keyAttribute = element.Attribute("Key");
                valueAttribute = element.Attribute("Value");

                if ((keyAttribute == null) || (valueAttribute == null)) continue;

                var key = keyAttribute.Value;
                var value = valueAttribute.Value;

                companyDict.Add(key, value);
              }
            }
            else if (xmlContextId == contextId)
            {
              foreach (var element in companyNameElement.Descendants("Name"))
              {
                keyAttribute = element.Attribute("Key");
                valueAttribute = element.Attribute("Value");

                if ((keyAttribute == null) || (valueAttribute == null)) continue;

                var key = keyAttribute.Value;
                var value = valueAttribute.Value;

                companyDict.Add(key, value);
              }
            }
          }
        }
      }

      return new CompanyNameResponseData(companyDict);
      }

    static CompanyNameResponseData()
    {
      Empty = new CompanyNameResponseData(new Dictionary<string, string>());
    }

    public Dictionary<string, string> CompanyDict
    {
      get { return _companyDict; }
    }

    public string GetName(string companyPropertyKey)
    {
      string companyValue;
      CompanyDict.TryGetValue(companyPropertyKey, out companyValue);

      return companyValue ?? string.Empty;
    }

    public string ToXML()
    {
      var element = new XElement("CompanyNameResponseData");
      return element.ToString(SaveOptions.DisableFormatting);
    }

    public AtlantisException GetException()
    {
      return null;
    }

    private CompanyNameResponseData(Dictionary<string, string> companyDict)
    {
      _companyDict = companyDict;
    }

  }
}