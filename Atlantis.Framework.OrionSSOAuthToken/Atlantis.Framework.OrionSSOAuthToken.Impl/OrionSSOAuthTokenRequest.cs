using System;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.OrionSecurityAuth.Interface;
using Atlantis.Framework.OrionSSOAuthToken.Impl.OrionSecurityService;
using Atlantis.Framework.OrionSSOAuthToken.Interface;

namespace Atlantis.Framework.OrionSSOAuthToken.Impl
{
  public class OrionSSOAuthTokenRequest : IRequest
  {
    private const string ORION_SYSTEM_NAMESACE = "GoDaddy";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      OrionSSOAuthTokenResponseData responseData = null;
      string ssoAuthToken = string.Empty;
      string[] errors;
      int resultCode = 1;

      try
      {
        OrionSecurityAuthResponseData securityResponse = GetOrionAuthToken(requestData);
        var orionRequestData = (OrionSSOAuthTokenRequestData)requestData;

        if(securityResponse.IsSuccess && !string.IsNullOrWhiteSpace(securityResponse.AuthToken))
        {
          using (var securityService = new Security())
          {
            securityService.Url = ((WsConfigElement)config).WSURL;
            securityService.Timeout = (int)orionRequestData.RequestTimeout.TotalMilliseconds;
            securityService.SecureHeaderValue = new SecureHeader();
            securityService.SecureHeaderValue.Token = securityResponse.AuthToken;
            OrionCustomer orionUser = new OrionCustomer();
            orionUser.CustomerNum = orionRequestData.ShopperID;
            orionUser.ResellerId = orionRequestData.PrivateLabelId.ToString();
            orionUser.SystemNamespace = ORION_SYSTEM_NAMESACE;

            resultCode = securityService.GetSSOAuthToken(string.Empty, orionRequestData.OrionProductName, orionUser, out ssoAuthToken, out errors);

            if (!string.IsNullOrWhiteSpace(ssoAuthToken) && resultCode != 1 && errors == null)
            {
              responseData = new OrionSSOAuthTokenResponseData(ssoAuthToken);
            }
            else
            {
              StringBuilder sbErr = new StringBuilder();
              foreach (string err in errors)
              {
                sbErr.Append(string.Format("{0} |", err));
              }
              string error = string.Format("RESULT CODE: {0} - ERRORS: {1}", resultCode, sbErr.ToString());
              AtlantisException aex = new AtlantisException(orionRequestData, "OrionSSOAuthTokenRequest::RequestHandler", error, error);

              responseData = new OrionSSOAuthTokenResponseData(aex);
            }
          }
        }
      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new OrionSSOAuthTokenResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new OrionSSOAuthTokenResponseData(requestData, ex);
      }

      return responseData;
    }

    #region OrionSecurity
    private OrionSecurityAuthResponseData GetOrionAuthToken(RequestData request)
    {
      var securityRequestData = new OrionSecurityAuthRequestData(request.ShopperID
        , request.SourceURL
        , request.OrderID
        , request.Pathway
        , request.PageCount
        , "OrionGetSSOAuthToken");

      var responseSecurityData = (OrionSecurityAuthResponseData)DataCache.DataCache.GetProcessRequest(securityRequestData, securityRequestData.OrionSecurityAuthRequestType);

      return responseSecurityData;
    }
    #endregion
  }
}
