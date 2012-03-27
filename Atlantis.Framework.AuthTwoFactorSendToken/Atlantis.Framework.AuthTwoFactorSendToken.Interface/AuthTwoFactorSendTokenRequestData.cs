using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthTwoFactorSendToken.Interface
{
  public class AuthTwoFactorSendTokenRequestData: RequestData
  {
    public string IPAddress { get; set; }
    public string HostName { get; set; }
    public string PhoneNumber { get; set; }

    public AuthTwoFactorSendTokenRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, string phoneNumber, string hostName, string ipAddress)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      IPAddress = ipAddress;
      HostName = hostName;
      PhoneNumber = phoneNumber;
      RequestTimeout = TimeSpan.FromSeconds(6);
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }
  }
}
