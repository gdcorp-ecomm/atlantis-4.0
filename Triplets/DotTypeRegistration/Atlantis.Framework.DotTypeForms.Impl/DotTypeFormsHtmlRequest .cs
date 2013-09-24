﻿using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
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
        var wsConfigElement = ((WsConfigElement)config);

        var fullUrl = string.Format("{0}/form/{1}?tldid={2}&pl={3}&ph={4}&marketid={5}&contextid={6}",
                                    wsConfigElement.WSURL,
                                    dotTypeFormsHtmlSchemaRequestData.FormType,
                                    dotTypeFormsHtmlSchemaRequestData.TldId, 
                                    dotTypeFormsHtmlSchemaRequestData.Placement,
                                    dotTypeFormsHtmlSchemaRequestData.Phase,
                                    dotTypeFormsHtmlSchemaRequestData.MarketId,
                                    dotTypeFormsHtmlSchemaRequestData.ContextId);

        responseHtml = HttpWebRequestHelper.SendWebRequest(dotTypeFormsHtmlSchemaRequestData, fullUrl, wsConfigElement);
        responseData = new DotTypeFormsHtmlResponseData(responseHtml);
      }
      catch (Exception ex)
      {
        responseData = new DotTypeFormsHtmlResponseData(responseHtml, requestData, ex);
      }

      return responseData;
    }
  }
}
