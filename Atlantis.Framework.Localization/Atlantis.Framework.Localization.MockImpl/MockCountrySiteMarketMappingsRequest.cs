using System;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Localization.Interface;

namespace Atlantis.Framework.Localization.MockImpl
{
  public class MockCountrySiteMarketMappingsRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = null;
      if (HttpContext.Current != null)
      {
        var request = requestData as CountrySiteMarketMappingsRequestData;
        if (request == null)
        {
          throw new Exception(this.GetType().Name + " requires a request derived from " + typeof(CountrySiteMarketMappingsRequestData).Name);
        }
        string strResp = HttpContext.Current.Items[MockLocalizationSettings.CountrySiteMarketMappingsTable] as string;
        XDocument xDoc = XDocument.Parse(strResp);
        xDoc.Descendants("item").Where(i => !((string)i.Attribute("catalog_countrySite")).Equals(request.CountrySite, StringComparison.OrdinalIgnoreCase)).Remove();

        result = CountrySiteMarketMappingsResponseData.FromCacheDataXml(xDoc.ToString());

      }
      return result;
    }

    #endregion
  }
}
