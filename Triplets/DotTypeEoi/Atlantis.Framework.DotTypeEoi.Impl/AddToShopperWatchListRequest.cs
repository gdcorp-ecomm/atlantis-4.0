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
      IResponseData result = null;
      var request = requestData as AddToShopperWatchListRequestData;

      var shopperId = requestData.ShopperID;

      try
      {
        if (request != null)
        {
          var displayTime = request.DisplayTime;
          var gTldsXml = request.GTldsXml;

          using (var regEoiWebSvc = new RegEOIWebSvc())
          {
            regEoiWebSvc.Url = ((WsConfigElement) config).WSURL;
            regEoiWebSvc.Timeout = (int) requestData.RequestTimeout.TotalMilliseconds;
            var responseXml = regEoiWebSvc.AddToShopperWatchListXml(shopperId, displayTime, gTldsXml);

            result = AddToShopperWatchListResponseData.FromXElement(XElement.Parse(responseXml));
          }
        }
      }
      catch (Exception ex)
      {
        var exception = new AtlantisException(requestData, "AddToShopperWatchListRequest.RequestHandler", ex.Message + ex.StackTrace, requestData.ToXML());
        result = AddToShopperWatchListResponseData.FromException(exception);
      }

      return result;
    }
  }
}
