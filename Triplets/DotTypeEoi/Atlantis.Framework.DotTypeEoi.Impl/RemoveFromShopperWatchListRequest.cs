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
      IResponseData result = null;
      var request = requestData as RemoveFromShopperWatchListRequestData;

      var shopperId = requestData.ShopperID;

      try
      {
        if (request != null)
        {
          var gTldsXml = request.GTldsXml;

          using (var regEoiWebSvc = new RegEOIWebSvc())
          {
            regEoiWebSvc.Url = ((WsConfigElement) config).WSURL;
            regEoiWebSvc.Timeout = (int) requestData.RequestTimeout.TotalMilliseconds;
            var responseXml = regEoiWebSvc.RemoveFromShopperWatchListXml(shopperId, gTldsXml);

            result = RemoveFromShopperWatchListResponseData.FromXElement(XElement.Parse(responseXml));
          }
        }
      }
      catch (Exception ex)
      {
        var exception = new AtlantisException(requestData, "RemoveFromShopperWatchListRequest.RequestHandler", ex.Message + ex.StackTrace, requestData.ToXML());
        result = RemoveFromShopperWatchListResponseData.FromException(exception);
      }

      return result;
    }
  }
}
