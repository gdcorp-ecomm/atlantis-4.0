using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthTwoFactorValidateToken.Interface
{
  public class AuthTwoFactorValidateTokenRequestData: RequestData
  {
    public string IPAddress { get; set; }
    public string HostName { get; set; }
    public string PhoneNumber { get; set; }
    public string AuthToken { get; set; }

    public AuthTwoFactorValidateTokenRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, string authToken, string phoneNumber, string hostName, string ipAddress)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      IPAddress = ipAddress;
      HostName = hostName;
      PhoneNumber = phoneNumber;
      AuthToken = authToken;
      RequestTimeout = TimeSpan.FromSeconds(6);
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }
  }
}
