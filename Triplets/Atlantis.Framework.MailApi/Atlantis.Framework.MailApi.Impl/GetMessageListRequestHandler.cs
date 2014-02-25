using Atlantis.Framework.Interface;
using Atlantis.Framework.MailApi.Interface;
using System;

namespace Atlantis.Framework.MailApi.Impl
{
  // Possible atlantis.config entry - remove this before peer review
  // <ConfigElement progid="Atlantis.Framework.MailApi.Impl.GetMessageListRequestHandler" assembly="Atlantis.Framework.MailApi.Impl.dll" request_type="###" />

  public class GetMessageListRequestHandler : IRequest
  {
    private const string MessageListBodyString = "method=getMessageList&params={{\"folder_num\":\"{0}\",\"offset\":\"{1}\",\"message_count\":\"{2}\",\"message_order\":[{{\"field\":\"date\",\"dir\":\"desc\"}}],\"message_filter\":\"{3}\"}}";
    private const string DefaultBaseUrl = "mailapi.secureserver.net";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      var request = (GetMessageListRequestData)requestData;

      string webServiceUrl = request.MailBaseUrl.Contains("80") ? "http://" : "https://";
      webServiceUrl += !string.IsNullOrEmpty(request.MailBaseUrl) ? request.MailBaseUrl : DefaultBaseUrl;
      webServiceUrl += ((WsConfigElement)config).WSURL;

      string messageBody = String.Format(MessageListBodyString, request.FolderNum, request.Offset, request.Count, request.Filter);
      string getMessageListResponseString = Utility.PostRequest(webServiceUrl, messageBody, request.MailHash, request.AppKey, null);

      GetMessageListResponseData getMessageListResponse = GetMessageListResponseData.FromJsonData(getMessageListResponseString);

      return getMessageListResponse;
    }
  }
}
