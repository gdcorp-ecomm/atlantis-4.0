using System;
using System.Web;
using System.Xml;
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
        var xmldoc = new XmlDocument();
        xmldoc.LoadXml(strResp);
        var nodes = xmldoc.SelectNodes(String.Concat("/data/item[@catalog_marketID!='", request.MarketId, "']"));
        foreach (XmlNode node in nodes)
        {
          node.ParentNode.RemoveChild(node);
        }
        string filteredResp = xmldoc.OuterXml;
        result = MarketMappingsResponseData.FromCacheDataXml(filteredResp);

      }
      return result;
    }

    #endregion
  }
}

