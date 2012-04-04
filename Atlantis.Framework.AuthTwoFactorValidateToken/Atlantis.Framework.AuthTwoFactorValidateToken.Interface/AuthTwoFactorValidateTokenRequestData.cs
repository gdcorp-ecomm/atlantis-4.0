using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthTwoFactorValidateToken.Interface
{
  public class AuthTwoFactorValidateTokenRequestData: RequestData
  {
    public string IPAddress { get; set; }

    public string HostName { get; set; }

    public string FullPhoneNumber { get; set; }

    public string AuthToken { get; set; }

    public AuthTwoFactorValidateTokenRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, string authToken, string countryCode, string phoneNumber, string hostName, string ipAddress)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      IPAddress = ipAddress;
      HostName = hostName;
      FullPhoneNumber = countryCode + phoneNumber;
      AuthToken = authToken;
      RequestTimeout = TimeSpan.FromSeconds(6);
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }
  }
}
