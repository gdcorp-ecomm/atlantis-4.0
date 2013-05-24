using System;
using Atlantis.Framework.DotTypeValidation.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DotTypeValidation.Impl
{
  public class DotTypeClaimsValidationRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      DotTypeClaimsValidationResponseData responseData;
      string responseXml = string.Empty;

      try
      {
        var dotTypeValidationRequestData = (DotTypeClaimsValidationRequestData)requestData;
        using (var regAppTokenWebSvc = new RegAppTokenWebSvc.RegAppTokenWebSvc())
        {
          regAppTokenWebSvc.Url = ((WsConfigElement)config).WSURL;
          regAppTokenWebSvc.Timeout = (int)requestData.RequestTimeout.TotalMilliseconds;
          responseXml = regAppTokenWebSvc.RegisterPIIDataExt(dotTypeValidationRequestData.ToXML());

          /*if (responseXml.IndexOf("<error>", StringComparison.OrdinalIgnoreCase) > -1)
          {
            AtlantisException exAtlantis = new AtlantisException(oRequestData,
                                                                 "ShopperRequest.RequestHandler",
                                                                 responseXml,
                                                                 oRequestData.ToXML());

            oResponseData = new DotTypeValidationResponseData(responseXml, exAtlantis);
          }
          else
          {*/
          responseData = new DotTypeClaimsValidationResponseData(responseXml);
          //}
        }
      }
      catch (Exception ex)
      {
        responseData = new DotTypeClaimsValidationResponseData(responseXml, requestData, ex);
      }

      return responseData;
    }
  }
}
