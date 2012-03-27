using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthTwoFactorActivatePhone.Interface
{
  public class AuthTwoFactorActivatePhoneRequestData : RequestData
  {
    private static readonly TimeSpan _defaultRequestTimeout = TimeSpan.FromSeconds(6);

    public string PhoneNumber { get; set; }

    public string AuthToken { get; set; }

    public string IpAddress { get; set; }

    public string HostName { get; set; }

    public AuthTwoFactorActivatePhoneRequestData(string shopperId, string phoneNumber, string authToken, string ipAddress, string hostName, string sourceUrl, string orderId, string pathway, int pageCount) : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      PhoneNumber = phoneNumber;
      AuthToken = authToken;
      IpAddress = ipAddress;
      HostName = hostName;
      RequestTimeout = _defaultRequestTimeout;
    }

    public override string GetCacheMD5()
    {
      throw new Exception("AuthTwoFactorActivatePhone is not a cacheable request");
    }
  }
}
