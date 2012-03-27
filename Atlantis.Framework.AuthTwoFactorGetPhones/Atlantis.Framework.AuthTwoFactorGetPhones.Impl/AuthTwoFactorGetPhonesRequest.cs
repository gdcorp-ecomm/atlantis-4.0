using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.Auth.Interface;
using Atlantis.Framework.AuthTwoFactorGetPhones.Impl.AuthService;
using Atlantis.Framework.AuthTwoFactorGetPhones.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.ServiceHelper;

namespace Atlantis.Framework.AuthTwoFactorGetPhones.Impl
{
  public class AuthTwoFactorGetPhonesRequest : IRequest
  {
    private static HashSet<int> ValidateRequest(AuthTwoFactorGetPhonesRequestData request)
    {
      HashSet<int> result = new HashSet<int>();

      if (string.IsNullOrEmpty(request.ShopperID))
      {
        result.Add(AuthValidationCodes.ValidateShopperIdRequired);
      }

      return result;
    }

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      AuthTwoFactorGetPhonesResponseData responseData;

      try
      {
        string wsUrl = ((WsConfigElement)config).WSURL;
        if (!wsUrl.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase))
        {
          throw new AtlantisException(requestData, "AuthTwoFactorGetPhones::RequestHandler", "AuthTwoFactorGetPhones WS URL in atlantis.config must use https.", string.Empty);
        }

        using (Authentication authService = new Authentication())
        {
          AuthTwoFactorGetPhonesRequestData request = (AuthTwoFactorGetPhonesRequestData)requestData;
          long statusCode = TwoFactorWebserviceResponseCodes.Error;

          HashSet<int> validationCodes = ValidateRequest(request);

          string errorOutput;
          string phonesXml;

          if (validationCodes.Count > 0)
          {
            errorOutput = "Request not valid.";
            phonesXml = string.Empty;
          }
          else
          {
            authService.Url = wsUrl;
            authService.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

            X509Certificate2 clientCert = ClientCertHelper.GetClientCertificate(config);
            if (clientCert == null)
            {
              throw new AtlantisException(requestData, "AuthTwoFactorGetPhones::RequestHandler", "Unable to find client cert for web service call", string.Empty);
            }
            authService.ClientCertificates.Add(clientCert);

            statusCode = authService.GetPhones(request.ShopperID, out phonesXml, out errorOutput);
          }

          responseData = new AuthTwoFactorGetPhonesResponseData(statusCode, validationCodes, phonesXml, errorOutput);
        }
      }
      catch (Exception ex)
      {
        responseData = new AuthTwoFactorGetPhonesResponseData(ex, requestData);
      }

      return responseData;
    }
  }
}
