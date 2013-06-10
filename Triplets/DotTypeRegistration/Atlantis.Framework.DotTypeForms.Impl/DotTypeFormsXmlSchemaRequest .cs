using Atlantis.Framework.Interface;
using Atlantis.Framework.DotTypeForms.Interface;
using System;

namespace Atlantis.Framework.DotTypeForms.Impl
{
  public class DotTypeFormsXmlSchemaRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      DotTypeFormsXmlSchemaResponseData responseData;
      string responseXml = string.Empty;

      try
      {
        var dotTypeFormsXmlSchemaRequestData = (DotTypeFormsXmlSchemaRequestData)requestData;
        using (var tuiApiService = new TuiAPI.TuiApi())
        {
          tuiApiService.Url = ((WsConfigElement)config).WSURL;
          tuiApiService.Timeout = (int)requestData.RequestTimeout.TotalMilliseconds;
          responseXml = tuiApiService.GetFormSchemas(dotTypeFormsXmlSchemaRequestData.ToXML());

          /*if (responseXml.IndexOf("<error>", StringComparison.OrdinalIgnoreCase) > -1)
          {
            AtlantisException exAtlantis = new AtlantisException(oRequestData,
                                                                 "ShopperRequest.RequestHandler",
                                                                 responseXml,
                                                                 oRequestData.ToXML());

            oResponseData = new DotTypeFormsSchemaResponseData(responseXml, exAtlantis);
          }
          else
          {*/
          responseData = new DotTypeFormsXmlSchemaResponseData(responseXml);
          //}
        }
      }
      catch (Exception ex)
      {
        responseData = new DotTypeFormsXmlSchemaResponseData(responseXml, requestData, ex);
      }

      return responseData;
    }
  }
}
