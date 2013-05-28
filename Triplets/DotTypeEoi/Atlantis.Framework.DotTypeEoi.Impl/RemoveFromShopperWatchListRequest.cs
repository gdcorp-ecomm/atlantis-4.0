using System;
using System.Xml.Linq;
using Atlantis.Framework.DotTypeEoi.Impl.RegEoiWebSvc;
using Atlantis.Framework.DotTypeEoi.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DotTypeEoi.Impl
{
  public class RemoveFromShopperWatchListRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      RemoveFromShopperWatchListResponseData responseData;
      var responseXml = string.Empty;

      var shopperId = requestData.ShopperID;

      try
      {
        using (var regEoiWebSvc = new RegEOIWebSvc())
        {
          regEoiWebSvc.Url = ((WsConfigElement)config).WSURL;
          regEoiWebSvc.Timeout = (int)requestData.RequestTimeout.TotalMilliseconds;
          responseXml = regEoiWebSvc.RemoveFromShopperWatchListXml(shopperId, requestData.ToXML());

          var addToShopperWatchListElement = XElement.Parse(responseXml);
          var responseElement = addToShopperWatchListElement.Element("response");
          var resultAttribute = responseElement.Attribute("result").Value;
          if (string.IsNullOrEmpty(resultAttribute) || !resultAttribute.Equals("success"))
          {
            var exAtlantis = new AtlantisException(requestData,
                                                   "RemoveFromShopperWatchListRequest",
                                                   responseXml,
                                                   requestData.ToXML());

            responseData = new RemoveFromShopperWatchListResponseData(responseXml, exAtlantis);
          }
          else
          {
            responseData = new RemoveFromShopperWatchListResponseData(responseElement.Value);
          }
        }
      }
      catch (Exception ex)
      {
        responseData = new RemoveFromShopperWatchListResponseData(responseXml, requestData, ex);
      }

      return responseData;
    }
  }
}
