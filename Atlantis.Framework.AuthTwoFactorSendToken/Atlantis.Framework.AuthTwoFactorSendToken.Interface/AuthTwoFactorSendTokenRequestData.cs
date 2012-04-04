using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthTwoFactorSendToken.Interface
{
  public class AuthTwoFactorSendTokenRequestData: RequestData
  {
    public string IPAddress { get; set; }

    public string HostName { get; set; }

    public string FullPhoneNumber { get; set; }

    public AuthTwoFactorSendTokenRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, string countryCode, string phoneNumber, string hostName, string ipAddress)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      IPAddress = ipAddress;
      HostName = hostName;
      FullPhoneNumber = countryCode + phoneNumber;
      RequestTimeout = TimeSpan.FromSeconds(6);
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }
  }
}
