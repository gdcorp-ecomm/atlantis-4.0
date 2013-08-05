using System.Collections.Generic;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Brand.Interface
{
  public class CompanyNameResponseData : IResponseData
  {
    private AtlantisException _exception;
    private Dictionary<string, string> _companyDict;

    private const int ResellerContextId = 6;
    private const int SiteName = 0;

    public static CompanyNameResponseData Empty { get; private set; }

    public static CompanyNameResponseData FromCompanyNameXml(string companyNameXml, int contextId, int privateLabelId)
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

      if (contextId == ResellerContextId)
      {
        var siteName = DataCache.DataCache.GetPLData(privateLabelId, SiteName);

        companyDict.Add("Name", siteName);
        companyDict.Add("NameDotCom", siteName);
        companyDict.Add("NameLegal", siteName);
        companyDict.Add("NameParentCompany", siteName);
      }

      return new CompanyNameResponseData(companyDict);
      }

    static CompanyNameResponseData()
    {
      Empty = new CompanyNameResponseData(new Dictionary<string, string>());
    }

    private CompanyNameResponseData(Dictionary<string, string> companyDict)
    {
      _companyDict = companyDict;
    }

    public Dictionary<string, string> CompanyDict
    {
      get { return _companyDict; }
    }

    public string ToXML()
    {
      var element = new XElement("CompanyNameResponseData");
      return element.ToString(SaveOptions.DisableFormatting);
    }

    public static CompanyNameResponseData FromException(AtlantisException exception)
    {
      return new CompanyNameResponseData(exception);
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    private CompanyNameResponseData(AtlantisException exception)
    {
      _exception = exception;
    }
  }
}