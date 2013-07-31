using System.Collections.Generic;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Brand.Interface
{
  public class CompanyNameResponseData : IResponseData
  {
    private AtlantisException _exception;
    private List<CompanyName> _companyList;

    private const int ResellerContextId = 6;
    private const int SiteName = 0;

    public static CompanyNameResponseData Empty { get; private set; }

    public static CompanyNameResponseData FromCompanyNameXml(string companyNameXml, int contextId, int privateLabelId)
    {
      XDocument companyNamesDoc = XDocument.Parse(companyNameXml);
      List<CompanyName> companyList = new List<CompanyName>();

      foreach (XElement companyNamesElement in companyNamesDoc.Descendants("CompanyName"))
      {
        CompanyName company = CompanyName.FromCacheXml(companyNamesElement);

        companyList.Add(company);
      }

      if (contextId == ResellerContextId)
      {
        string siteName = DataCache.DataCache.GetPLData(privateLabelId, SiteName);

        CompanyName plCompany = CompanyName.FromDataCache("SiteName", siteName);
        companyList.Add(plCompany);
      }

      return new CompanyNameResponseData(companyList);
    }

    static CompanyNameResponseData()
    {
      Empty = new CompanyNameResponseData(new List<CompanyName>());
    }

    private CompanyNameResponseData(List<CompanyName> companyList)
    {
      _companyList = companyList;
    }

    public string ToXML()
    {
      XElement element = new XElement("CompanyNameResponseData");
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

    public IEnumerable<CompanyName> CompanyList
    {
      get { return _companyList; }
    }
  }
}