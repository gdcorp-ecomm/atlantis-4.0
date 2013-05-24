using System;
using Atlantis.Framework.DotTypeValidation.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DotTypeValidation.Impl
{
  public class DotTypeFormsValidationRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      DotTypeFormsValidationResponseData responseData;
      string responseXml = string.Empty;

      try
      {
        var dotTypeValidationRequestData = (DotTypeFormsValidationRequestData)requestData;
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
          responseData = new DotTypeFormsValidationResponseData(responseXml);
          //}
        }
      }
      catch (Exception ex)
      {
        responseData = new DotTypeFormsValidationResponseData(responseXml, requestData, ex);
      }

      return responseData;
    }
  }
}
