using System;
using Atlantis.Framework.Auth.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthTwoFactorEnable.Interface
{
  public class AuthTwoFactorEnableRequestData : RequestData
  {
    private static readonly TimeSpan _defaultRequestTimeout = TimeSpan.FromSeconds(6);

    public string Password { get; private set; }

    public int PrivateLableId { get; private set; }

    public AuthTwoFactorPhone Phone { get; private set; }

    public string HostName { get; private set; }

    public string IpAddress { get; private set; }

    public AuthTwoFactorEnableRequestData(string shopperId, string password, int privateLabelId, string phoneNumber, string carrier, string hostName, string ipAddress, string sourceUrl, string orderId, string pathway, int pageCount) : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      Password = password;
      PrivateLableId = privateLabelId;
      Phone = new AuthTwoFactorPhone { PhoneNumber = phoneNumber, Carrier = carrier };
      HostName = hostName;
      IpAddress = ipAddress;

      RequestTimeout = _defaultRequestTimeout;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("Atlantis.Framework.AuthTwoFactorEnable is not a cacheable request.");
    }
  }
}
