using System.Net;
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
      var responseXml = string.Empty;

      try
      {
        var dotTypeFormsXmlSchemaRequestData = (DotTypeFormsXmlRequestData)requestData;
        var wsConfigElement = ((WsConfigElement)config);

        var fullUrl = string.Format("{0}/api/schema/getxml?formtype={1}&tldid={2}&pl={3}&ph={4}&marketid={5}&contextid={6}",
                                    wsConfigElement.WSURL,
                                    dotTypeFormsXmlSchemaRequestData.FormType,
                                    dotTypeFormsXmlSchemaRequestData.TldId,
                                    dotTypeFormsXmlSchemaRequestData.Placement,
                                    dotTypeFormsXmlSchemaRequestData.Phase,
                                    dotTypeFormsXmlSchemaRequestData.MarketId,
                                    dotTypeFormsXmlSchemaRequestData.ContextId);

        responseXml = HttpWebRequestHelper.SendWebRequest(dotTypeFormsXmlSchemaRequestData, fullUrl, wsConfigElement);

        responseData = new DotTypeFormsXmlResponseData(responseXml);
      }
      catch (WebException ex)
      {
        if (ex.Response is HttpWebResponse && ((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.NotFound)
        {
          //no error logging is required
          responseData = new DotTypeFormsXmlResponseData(string.Empty);
        }
        else
        {
          throw;
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
