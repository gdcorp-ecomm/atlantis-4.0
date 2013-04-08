using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MSA.Interface;
using Atlantis.Framework.MSAGetMessage.Interface;

namespace Atlantis.Framework.MSAGetMessage.Impl
{
  public class MSAGetMessageRequest : IRequest
  {
    private const string GetMessageBodyString = "method=fetchMessage&params={{\"header_num\":\"{0}\",\"preferred_type\":\"{1}\",\"extended_header\":\"{2}\"}}";
    string preferredType;

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      MSAGetMessageRequestData getMessageRequestData = (MSAGetMessageRequestData)requestData;
      MSAGetMessageResponseData getMessageResponseData;
      GetMessageResponse GetMessageResponse = new GetMessageResponse();
      try
      {

        string webServiceUrl = getMessageRequestData.BaseUrl.Contains("80") ? "http://" : "https://";

        webServiceUrl += getMessageRequestData.BaseUrl;
        webServiceUrl += config.GetConfigValue("ServicePath");

        string messageBody = String.Format(GetMessageBodyString, getMessageRequestData.HeaderNum.ToString(), preferredType, getMessageRequestData.ExtendedHeader);
        HttpWebResponse webResponse = MailApiUtil.sendMailAPIRequest(webServiceUrl, messageBody,
                                                                   getMessageRequestData.Hash,
                                                                   getMessageRequestData.RestrictedKey);
        Stream responseStream = webResponse.GetResponseStream();
        StreamReader responseReader = new StreamReader(responseStream, Encoding.UTF8);
        string jsonResponse = responseReader.ReadToEnd();

        MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonResponse));

        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(GetMessageResponse));

        GetMessageResponse = (GetMessageResponse)serializer.ReadObject(stream);
        responseStream.Close();
        responseReader.Close();
        stream.Close();

        if (MailApiUtil.IsInvalidSession(GetMessageResponse))
        {
          throw new Exception("Invalid Session");
        }

        getMessageResponseData = new MSAGetMessageResponseData(GetMessageResponse);
      }
      catch (Exception ex)
      {
        AtlantisException aex = new AtlantisException(requestData, "MSALoginUserRequest.RequestHandler", ex.Message,
                                                      ex.StackTrace, ex);
        getMessageResponseData = new MSAGetMessageResponseData(aex);

      }
      return getMessageResponseData;
    }
  }
}
