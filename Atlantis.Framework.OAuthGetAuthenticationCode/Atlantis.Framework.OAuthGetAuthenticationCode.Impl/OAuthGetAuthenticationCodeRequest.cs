using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Atlantis.Framework.OAuthGetAuthenticationCode.Interface;
using Atlantis.Framework.Interface;
using System.Web.Script.Serialization;
using Atlantis.Framework.OAuth.Interface.Errors;

namespace Atlantis.Framework.OAuthGetAuthenticationCode.Impl
{
  public class OAuthGetAuthenticationCodeRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      OAuthGetAuthenticationCodeResponseData responseData;

      try
      {
        string authenticationCode = string.Empty;
        string errorCode;
        string errorDescription = string.Empty;

        var oauthClientRequest = (OAuthGetAuthenticationCodeRequestData)requestData;
        if (IsRequestValid(oauthClientRequest, out errorCode))
        {

        #region Setup request
          WsConfigElement wsConfigElement = (WsConfigElement)config;
          string wsUrl = string.Concat(wsConfigElement.WSURL, oauthClientRequest.ShopperID, "/", oauthClientRequest.ResourceDescription);
          Uri authUrl = new Uri(wsUrl);

          HttpWebRequest authWebRequest = WebRequest.Create(authUrl) as HttpWebRequest;
          authWebRequest.Method = "POST";
          authWebRequest.ContentType = "application/json";
          authWebRequest.Timeout = (int)oauthClientRequest.RequestTimeout.TotalMilliseconds;

          X509Certificate2 clientCert = wsConfigElement.GetClientCertificate();
          if (clientCert == null)
          {
            throw new AtlantisException(requestData, "OAuthGetAuthenticationCodeResponseData::RequestHandler", "Unable to find client cert for web service call", string.Empty);
          }
          authWebRequest.ClientCertificates.Add(clientCert);
        #endregion

        #region Create post data
          var postData = new
                           {
                             oauthClientRequest.PalmsID,
                             oauthClientRequest.BillingType,
                             oauthClientRequest.BillingNamespace,
                             oauthClientRequest.ResourceID,
                             oauthClientRequest.AccessList
                           };
          JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
          string serializedPostData = jsSerializer.Serialize(postData);
        #endregion

        #region Get data and write to stream
          byte[] byteData = Encoding.UTF8.GetBytes(serializedPostData);
          authWebRequest.ContentLength = byteData.Length;
          using (Stream authPostStream = authWebRequest.GetRequestStream())
          {
            authPostStream.Write(byteData, 0, byteData.Length);
          }
        #endregion

        #region Get data from response
          using (HttpWebResponse authWebResponse = authWebRequest.GetResponse() as HttpWebResponse)
          {
            StreamReader responseReader = new StreamReader(authWebResponse.GetResponseStream());
            string authWebResponseData = responseReader.ReadToEnd();
            var authCode = jsSerializer.Deserialize<AuthCode>(authWebResponseData);
            authenticationCode = authCode.sAuthcode;
            errorDescription = authCode.sErrorDesc;
          }
        #endregion

        }
        responseData = new OAuthGetAuthenticationCodeResponseData(authenticationCode, errorCode, errorDescription);
      }
      catch (AtlantisException aex)
      {
        responseData = new OAuthGetAuthenticationCodeResponseData(requestData, aex);
      }
      catch (Exception ex)
      {
        responseData = new OAuthGetAuthenticationCodeResponseData(requestData, ex);
      }

      return responseData;
    }

    private bool IsRequestValid(OAuthGetAuthenticationCodeRequestData requestData, out string errorCode)
    {
      errorCode = string.Empty;

      if (string.IsNullOrEmpty(requestData.PalmsID) ||
        string.IsNullOrEmpty(requestData.AccessList) ||
        string.IsNullOrEmpty(requestData.BillingType) ||
        string.IsNullOrEmpty(requestData.BillingNamespace) ||
        string.IsNullOrEmpty(requestData.ResourceDescription) ||
        string.IsNullOrEmpty(requestData.ResourceID))
      {
        errorCode = AuthTokenResponseErrorCodes.InvalidRequest;
      }

      return string.IsNullOrEmpty(errorCode);
    }
  }

  public class AuthCode
  {
    public string sAuthcode { get; set; }
    public string sErrorDesc { get; set; }
  }
}
