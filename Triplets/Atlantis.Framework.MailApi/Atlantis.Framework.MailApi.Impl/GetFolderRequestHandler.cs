using Atlantis.Framework.Interface;
using Atlantis.Framework.MailApi.Interface;
using System;

namespace Atlantis.Framework.MailApi.Impl
{
  // Possible atlantis.config entry - remove this before peer review
  // <ConfigElement progid="Atlantis.Framework.MailApi.Impl.GetFolderRequestHandler" assembly="Atlantis.Framework.MailApi.Impl.dll" request_type="###" />

  public class GetFolderRequestHandler : IRequest
  {
    private const string GET_FOLDER_BODY_STRING = "method=getFolderByFolderNum&params={{\"folder_num\":\"{0}\",\"extended_info\":\"true\"}}";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      var request = requestData as GetFolderRequestData;
      if (request == null) throw new ArgumentException("Invalid Request Data passed to RequestHandler for GetFolder Request");

      string messageBody = String.Format(GET_FOLDER_BODY_STRING, request.FolderNum);

      string webServiceUrl = Utility.BuildWebServiceUrl(request.MailBaseUrl, ((WsConfigElement)config).WSURL); 

      string jsonResponse = Utility.PostRequest(webServiceUrl, messageBody, request.Session, request.AppKey, request.Key);

      GetFolderResponseData folderResponse = GetFolderResponseData.FromJsonData(jsonResponse);

      folderResponse.request = request;

      return folderResponse;

    }
  }
}
