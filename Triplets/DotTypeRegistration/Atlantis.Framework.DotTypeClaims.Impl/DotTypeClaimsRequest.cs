using Atlantis.Framework.DotTypeClaims.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DotTypeClaims.Impl
{
  public class DotTypeClaimsRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      DotTypeClaimsResponseData responseData = null;
      /*string responseXml = string.Empty;

      try
      {
        var dotTypeClaimsRequestData = (DotTypeClaimsRequestData)requestData;
        using (var claimsService = new ClaimsApi.ClaimsApi())
        {
          claimsService.Url = ((WsConfigElement)config).WSURL;
          claimsService.Timeout = (int)requestData.RequestTimeout.TotalMilliseconds;
          responseXml = claimsService.GetClaims(dotTypeClaimsRequestData.ToXML());

          if (responseXml.IndexOf("<error>", StringComparison.OrdinalIgnoreCase) > -1)
          {
            AtlantisException exAtlantis = new AtlantisException(oRequestData,
                                                                 "ShopperRequest.RequestHandler",
                                                                 responseXml,
                                                                 oRequestData.ToXML());

            oResponseData = new DotTypeFormsSchemaResponseData(responseXml, exAtlantis);
          }
          else
          {
          responseData = new DotTypeClaimsResponseData(responseXml);
          }
        }
      }
      catch (Exception ex)
      {
        responseData = new DotTypeClaimsResponseData(responseXml, requestData, ex);
      }*/

      return responseData;
    }
  }
}
