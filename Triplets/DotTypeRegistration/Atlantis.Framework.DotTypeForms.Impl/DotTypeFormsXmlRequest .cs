using Atlantis.Framework.Interface;
using Atlantis.Framework.DotTypeForms.Interface;
using System;

namespace Atlantis.Framework.DotTypeForms.Impl
{
  public class DotTypeFormsXmlRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      DotTypeFormsXmlResponseData responseData;
      string responseXml = string.Empty;

      try
      {
        var dotTypeFormsXmlSchemaRequestData = (DotTypeFormsXmlRequestData)requestData;
        using (var tuiApiService = new TuiAPI.TuiApi())
        {
          tuiApiService.Url = ((WsConfigElement)config).WSURL;
          tuiApiService.Timeout = (int)requestData.RequestTimeout.TotalMilliseconds;
          responseXml = tuiApiService.GetFormSchemas(dotTypeFormsXmlSchemaRequestData.ToXML());

          responseData = new DotTypeFormsXmlResponseData(responseXml);
        }
      }
      catch (Exception ex)
      {
        responseData = new DotTypeFormsXmlResponseData(responseXml, requestData, ex);
      }

      return responseData;
    }
  }
}
