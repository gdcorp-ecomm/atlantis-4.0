using System;
using System.Xml.Linq;
using Atlantis.Framework.DotTypeEoi.Impl.RegEoiWebSvc;
using Atlantis.Framework.DotTypeEoi.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DotTypeEoi.Impl
{
  public class AddToShopperWatchListRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      AddToShopperWatchListResponseData responseData;
      var responseXml = string.Empty;

      var addToShopperWatchListRequest = (AddToShopperWatchListRequestData) requestData;

      try
      {
        using (var regEoiWebSvc = new RegEOIWebSvc())
        {
          regEoiWebSvc.Url = ((WsConfigElement) config).WSURL;
          regEoiWebSvc.Timeout = (int) requestData.RequestTimeout.TotalMilliseconds;
          responseXml = regEoiWebSvc.AddToShopperWatchListXml(requestData.ShopperID, addToShopperWatchListRequest.DisplayTime, addToShopperWatchListRequest.ToXML());

          var addToShopperWatchListElement = XElement.Parse(responseXml);
          var responseElement = addToShopperWatchListElement.Element("response");
          var resultAttribute = responseElement.Attribute("result").Value;
          if (string.IsNullOrEmpty(resultAttribute) || !resultAttribute.Equals("success"))
          {
            var exAtlantis = new AtlantisException(requestData,
                                                   "AddToShopperWatchListRequest",
                                                   responseXml,
                                                   requestData.ToXML());

            responseData = new AddToShopperWatchListResponseData(responseXml, exAtlantis);
          }
          else
          {
            responseData = new AddToShopperWatchListResponseData(responseElement.Value);
          }
        }
      }
      catch (Exception ex)
      {
        responseData = new AddToShopperWatchListResponseData(responseXml, requestData, ex);
      }

      return responseData;
    }
  }
}
