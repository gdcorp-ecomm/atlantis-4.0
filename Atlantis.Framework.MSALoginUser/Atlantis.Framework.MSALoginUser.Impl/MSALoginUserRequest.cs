using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MSA.Interface;
using Atlantis.Framework.MSALoginUser.Interface;
using System.Web;

namespace Atlantis.Framework.MSALoginUser.Impl
{
  public class MSALoginUserRequest : IRequest
  {
    private const string BodyString =
      "method=login&params={{\"username\":\"{0}\",\"password\":\"{1}\"}}&state={{\"app_key\":\"{2}\"}}";
  
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      MSALoginUserRequestData loginRequest = (MSALoginUserRequestData)requestData;
      MSALoginUserResponseData result;
      LoginResponse responseData = new LoginResponse();
      try
      {
        string webServiceUrl = ((WsConfigElement)config).WSURL;
        string username = HttpUtility.UrlEncode(loginRequest.Email);
        
        string messageBody = String.Format(BodyString, username, loginRequest.Password, loginRequest.ApiKey);

        HttpWebResponse loginResponse = MailApiUtil.sendMailAPIRequest(webServiceUrl, messageBody, string.Empty,
                                                                       loginRequest.ApiKey);
        Stream responseStream = loginResponse.GetResponseStream();
        StreamReader responseReader = new StreamReader(responseStream, Encoding.UTF8);
        string jsonResponse = responseReader.ReadToEnd();

        MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonResponse));

        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(LoginResponse));

        responseData = (LoginResponse)serializer.ReadObject(stream);
        responseStream.Close();
        responseReader.Close();
        stream.Close();
        result = new MSALoginUserResponseData(responseData);

      }
      catch (Exception ex)
      {
        AtlantisException aex = new AtlantisException(requestData, "MSALoginUserRequest.RequestHandler", ex.Message,
                                                      ex.StackTrace, ex);
        result = new MSALoginUserResponseData(aex);
      }
      
      return result;
    }
  }
}
