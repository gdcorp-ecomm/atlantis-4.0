using System;
using System.Xml.Linq;
using Atlantis.Framework.DotTypeEoi.Impl.RegEoiWebSvc;
using Atlantis.Framework.DotTypeEoi.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DotTypeEoi.Impl
{
  public class ShopperWatchListJsonRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      ShopperWatchListResponseData responseData;
      string responseXml = string.Empty;

      try
      {
        using (var regEoiWebSvc = new RegEOIWebSvc())
        {
          regEoiWebSvc.Url = ((WsConfigElement)config).WSURL;
          regEoiWebSvc.Timeout = (int)requestData.RequestTimeout.TotalMilliseconds;
          responseXml = regEoiWebSvc.GetShopperWatchListJSON(requestData.ShopperID);

          var shopperWatchListJsonElement = XElement.Parse(responseXml);
          var responseElement = shopperWatchListJsonElement.Element("response");
          var resultAttribute = responseElement.Attribute("result").Value;
          if (string.IsNullOrEmpty(resultAttribute) || !resultAttribute.Equals("success"))
          {
            var exAtlantis = new AtlantisException(requestData,
                                                   "ShopperWatchListJsonRequest",
                                                   responseXml,
                                                   requestData.ToXML());

            responseData = new ShopperWatchListResponseData(responseXml, exAtlantis);
          }
          else
          {
            responseData = new ShopperWatchListResponseData(responseElement.Value);
          }
        }
      }
      catch (Exception ex)
      {
        responseData = new ShopperWatchListResponseData(responseXml, requestData, ex);
      }

      return responseData;
    }
  }
}
