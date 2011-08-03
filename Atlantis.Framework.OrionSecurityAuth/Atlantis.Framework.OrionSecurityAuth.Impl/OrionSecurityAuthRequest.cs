using System;
using System.Xml;

using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;
using Atlantis.Framework.OrionSecurityAuth.Impl.OrionSecurityService;
using Atlantis.Framework.OrionSecurityAuth.Interface;

namespace Atlantis.Framework.OrionSecurityAuth.Impl
{
  public class OrionSecurityAuthRequest : IRequest
  {
    string _authToken = string.Empty;

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      OrionSecurityAuthResponseData responseData;

      try
      {
        string userName;
        string password;
        var orionRequestData = (OrionSecurityAuthRequestData)requestData;

        GetOrionSecurityAuthenticationCredentials(config, out userName, out password);

        if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
        {
          using (var securityService = new Security())
          {
            securityService.Url = ((WsConfigElement) config).WSURL;
            securityService.Timeout = (int) orionRequestData.RequestTimeout.TotalMilliseconds;
            _authToken = securityService.Authenticate(userName, password);
          }
        }

        if (!string.IsNullOrEmpty(_authToken))
        {
          responseData = new OrionSecurityAuthResponseData(_authToken);
        }
        else
        {
          throw new AtlantisException(requestData, "Security.Authenticate", "_authToken is empty", "APP_NAME: " + orionRequestData.AppName);
        }
      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new OrionSecurityAuthResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new OrionSecurityAuthResponseData(requestData, ex);
      }

      return responseData;
    }

    #region Orion Credentials
    private static void GetOrionSecurityAuthenticationCredentials(ConfigElement config, out string userName, out string password)
    {
      var xdoc = new XmlDocument();
      xdoc.LoadXml(LookupConnectionString(config));
      userName = xdoc.SelectSingleNode("Connect/UserID").LastChild.Value ?? string.Empty;
      password = xdoc.SelectSingleNode("Connect/Password").LastChild.Value ?? string.Empty;
    }

    #region Nimitz
    private static string LookupConnectionString(ConfigElement config)
    {
      string result = NetConnect.LookupConnectInfo(config, ConnectLookupType.Xml);

      if (string.IsNullOrEmpty(result) || result.Length <= 2)
      {
        throw new Exception("Invalid Connection String - Is the Orion DSN associated with your certificate?");
      }
      return result;
    }
    #endregion
    #endregion
  }
}
