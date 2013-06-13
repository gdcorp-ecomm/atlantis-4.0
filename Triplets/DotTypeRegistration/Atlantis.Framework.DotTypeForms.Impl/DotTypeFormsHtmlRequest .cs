using Atlantis.Framework.Interface;
using Atlantis.Framework.DotTypeForms.Interface;
using System;

namespace Atlantis.Framework.DotTypeForms.Impl
{
  public class DotTypeFormsHtmlRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      DotTypeFormsHtmlResponseData responseData;
      var responseHtml = string.Empty;

      try
      {
        var dotTypeFormsHtmlSchemaRequestData = (DotTypeFormsHtmlRequestData)requestData;
        using (var tuiApiService = new TuiAPI.TuiApi())
        {
          tuiApiService.Url = ((WsConfigElement)config).WSURL;
          tuiApiService.Timeout = (int)requestData.RequestTimeout.TotalMilliseconds;
          responseHtml = tuiApiService.GetForms(dotTypeFormsHtmlSchemaRequestData.ToXML());

          responseData = new DotTypeFormsHtmlResponseData(responseHtml);
        }
      }
      catch (Exception ex)
      {
        responseData = new DotTypeFormsHtmlResponseData(responseHtml, requestData, ex);
      }

      return responseData;
    }
  }
}
