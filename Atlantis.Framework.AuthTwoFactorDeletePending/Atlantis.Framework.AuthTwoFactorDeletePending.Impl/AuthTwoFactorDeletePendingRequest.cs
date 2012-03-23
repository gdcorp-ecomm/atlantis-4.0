using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.Auth.Interface;
using Atlantis.Framework.AuthTwoFactorDeletePending.Impl.wsGdAuthentication;
using Atlantis.Framework.AuthTwoFactorDeletePending.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.ServiceHelper;

namespace Atlantis.Framework.AuthTwoFactorDeletePending.Impl
{
  public class AuthTwoFactorDeletePendingRequest : IRequest
  {

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      AuthTwoFactorDeletePendingResponseData responseData = null;

      try
      {
        string authServiceUrl = ((WsConfigElement)config).WSURL;
        if (!authServiceUrl.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase))
        {
          throw new AtlantisException(requestData, "AuthTwoFactorDisableRequest.RequestHandler", "AuthTwoFactorDisable WS URL in atlantis.config must use https.", string.Empty);
        }

        X509Certificate2 cert = ClientCertHelper.GetClientCertificate(config);
        cert.Verify();

        using (Authentication authenticationService = new Authentication())
        {
          var request = (AuthTwoFactorDeletePendingRequestData)requestData;

          authenticationService.Url = authServiceUrl;
          authenticationService.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
          authenticationService.ClientCertificates.Add(cert);

          HashSet<int> validationCodes = ValidateRequest(request);

          if (validationCodes.Count > 0)
          {
            string data = string.Empty;
            foreach (int code in validationCodes)
            {
              data += string.Format("{0},", code);
            }
            data.TrimEnd(',');
            AtlantisException aex = new AtlantisException(request, "AuthTwoFactorDeletePendingRequest::RequestHandler", "0", "DeletePending request contained invalid data", data, request.IpAddress);
            responseData = new AuthTwoFactorDeletePendingResponseData(aex);
          }
          else
          {
            string err = string.Empty;
            long wsResponseCode = authenticationService.DeletePendingTwoFactor(request.ShopperID
              , request.PrivateLableId
              , request.HostName
              , request.IpAddress
              , out err);

            if (wsResponseCode == TwoFactorWebserviceResponseCodes.Success)
            {
              responseData = new AuthTwoFactorDeletePendingResponseData();
            }
            else
            {
              AtlantisException aex = new AtlantisException("AuthTwoFactorDeletePendingRequest::RequestHandler", request.SourceURL, wsResponseCode.ToString(), "DeletePending request failed", err, request.ShopperID, request.OrderID, request.IpAddress, request.Pathway, request.PageCount);
              responseData = new AuthTwoFactorDeletePendingResponseData(aex);
            }
          }
        }
      }

      catch (Exception ex)
      {
        responseData = new AuthTwoFactorDeletePendingResponseData(requestData, ex);
      }

      return responseData;
    }

    #region Validate Request
    private HashSet<int> ValidateRequest(AuthTwoFactorDeletePendingRequestData request)
    {
      HashSet<int> validationCodes = new HashSet<int>();

      if (string.IsNullOrEmpty(request.ShopperID))
      {
        validationCodes.Add(AuthValidationCodes.ValidateShopperIdRequired);
      }

      if (string.IsNullOrEmpty(request.IpAddress))
      {
        validationCodes.Add(AuthValidationCodes.ValidateIpAddressRequired);
      }

      if (string.IsNullOrEmpty(request.HostName))
      {
        validationCodes.Add(AuthValidationCodes.ValidateHostNameRequired);
      }

      return validationCodes;
    }
    #endregion

  }
}
