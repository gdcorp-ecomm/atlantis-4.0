﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.Auth.Interface;
using Atlantis.Framework.AuthTwoFactorDeletePhone.Impl.WsgdAuthentication;
using Atlantis.Framework.AuthTwoFactorDeletePhone.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.ServiceHelper;

namespace Atlantis.Framework.AuthTwoFactorDeletePhone.Impl
{
  public class AuthTwoFactorDeletePhoneRequest : IRequest
  {
    #region Validate Request
    private HashSet<int> ValidateRequest(AuthTwoFactorDeletePhoneRequestData request)
    {
      HashSet<int> validationCodes = new HashSet<int>();

      if (string.IsNullOrEmpty(request.ShopperID))
      {
        validationCodes.Add(AuthValidationCodes.ValidateShopperIdRequired);
      }

      if (string.IsNullOrEmpty(request.Phone.PhoneNumber))
      {
        validationCodes.Add(AuthValidationCodes.ValidatePhoneRequired);
      }

      if (string.IsNullOrEmpty(request.Phone.Carrier))
      {
        validationCodes.Add(AuthValidationCodes.ValidateCarrierRequired);
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

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      AuthTwoFactorDeletePhoneResponseData responseData = null;

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
          var request = (AuthTwoFactorDeletePhoneRequestData)requestData;

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
            AtlantisException aex = new AtlantisException(request, "AuthTwoFactorDeletePhoneRequest::RequestHandler", "0", "DeletePhone request contained invalid data", data, request.IpAddress);
            responseData = new AuthTwoFactorDeletePhoneResponseData(aex);
          }
          else
          {
            string err = string.Empty;
            long wsResponseCode = authenticationService.DeletePhone(request.ShopperID, request.Phone.ToXml(), request.HostName, request.IpAddress, out err);

            if (wsResponseCode == TwoFactorWebserviceResponseCodes.Success)
            {
              responseData = new AuthTwoFactorDeletePhoneResponseData();
            }
            else
            {
              AtlantisException aex = new AtlantisException("AuthTwoFactorDeletePhoneRequest::RequestHandler", request.SourceURL, wsResponseCode.ToString(), "DeletePhone request failed", err, request.ShopperID, request.OrderID, request.IpAddress, request.Pathway, request.PageCount);
              responseData = new AuthTwoFactorDeletePhoneResponseData(aex);
            }
          }
        }
      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new AuthTwoFactorDeletePhoneResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new AuthTwoFactorDeletePhoneResponseData(requestData, ex);
      }

      return responseData;
    }
  }
}
