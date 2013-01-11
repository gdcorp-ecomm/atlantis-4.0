using Atlantis.Framework.Interface;
using Atlantis.Framework.RegDotTypeProductIds.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Atlantis.Framework.RegDotTypeProductIds.Impl
{
  public class ProductIdListRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = null;
      string responseXml = null;

      try
      {
        using (var regApiService = new RegistrationApiWS.RegistrationApiWebSvc())
        {
          regApiService.Timeout = (int)requestData.RequestTimeout.TotalMilliseconds;
          regApiService.Url = ((WsConfigElement)config).WSURL;
          responseXml = regApiService.GetProductIdList(requestData.ToXML());
        }

        XElement responseElement = XElement.Parse(responseXml);
        result = ProductIdListResponseData.FromXElement(responseElement);
      }
      catch (Exception ex)
      {
        string message = ex.Message + ex.StackTrace;
        string data = requestData.ToXML() + ":" + (responseXml ?? string.Empty);
        AtlantisException aex = new AtlantisException(requestData, "ProductIdListResponseData.RequestHandler", message, data, ex);
        result = ProductIdListResponseData.FromException(aex);
      }

      return result;
    }
  }
}
