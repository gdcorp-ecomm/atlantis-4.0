using System;
using System.Web;
using System.Xml;
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
        var xmldoc = new XmlDocument();
        xmldoc.LoadXml(strResp);
        var nodes = xmldoc.SelectNodes(String.Concat("/data/item[@catalog_countrySite!='", request.CountrySite, "']"));
        foreach (XmlNode node in nodes)
        {
          node.ParentNode.RemoveChild(node);
        }
        string filteredResp = xmldoc.OuterXml;
        result = CountrySiteMarketMappingsResponseData.FromCacheDataXml(filteredResp);

      }
      return result;
    }

    #endregion
  }
}
