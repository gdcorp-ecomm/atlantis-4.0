using System;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Localization.Interface;

namespace Atlantis.Framework.Localization.MockImpl
{
  public class MockMarketMappingsRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = null;
      if (HttpContext.Current != null)
      {
        var request = requestData as MarketMappingsRequestData;
        if (request == null)
        {
          throw new Exception(this.GetType().Name + " requires a request derived from " + typeof(MarketMappingsRequestData).Name);
        }
        string strResp = HttpContext.Current.Items[MockLocalizationSettings.MarketMappingsTable] as string;
        XDocument xDoc = XDocument.Parse(strResp);
        xDoc.Descendants("item").Where(i => !((string) i.Attribute("catalog_marketID")).Equals(request.MarketId, StringComparison.OrdinalIgnoreCase)).Remove();

        result = MarketMappingsResponseData.FromCacheDataXml(xDoc.ToString());

      }
      return result;
    }

    #endregion
  }
}

