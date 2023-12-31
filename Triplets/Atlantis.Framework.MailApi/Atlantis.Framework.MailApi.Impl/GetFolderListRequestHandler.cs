﻿using System;
using System.Net;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MailApi.Interface;

namespace Atlantis.Framework.MailApi.Impl
{

  public class GetFolderListRequestHandler : IRequest
  {

    private string BodyString = "method=getFolderList&params={\"extended_info\":\"true\"}";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      var request = (GetFolderListRequestData)requestData;

      string webServiceUrl = Utility.BuildWebServiceUrl(request.MailBaseUrl, ((WsConfigElement)config).WSURL);

      string loginResponseString = Utility.PostRequest(webServiceUrl, BodyString, request.Session, request.AppKey, request.Key);

      GetFolderListResponseData response = GetFolderListResponseData.FromJsonData(loginResponseString);
      response.MailApiRequestString = BodyString;
      return response;
    }
  }
}
