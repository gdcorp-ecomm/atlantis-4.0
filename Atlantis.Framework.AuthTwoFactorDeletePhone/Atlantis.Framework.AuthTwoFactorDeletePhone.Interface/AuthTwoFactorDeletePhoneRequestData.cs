using System;
using Atlantis.Framework.Auth.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthTwoFactorDeletePhone.Interface
{
  public class AuthTwoFactorDeletePhoneRequestData : RequestData
  {
    #region Properties

    public AuthTwoFactorPhone Phone { get; private set; }
    public string HostName { get; private set; }
    public string IpAddress { get; private set; }

    #endregion

    public AuthTwoFactorDeletePhoneRequestData(string shopperId
      , string sourceUrl
      , string orderId
      , string pathway
      , int pageCount
      , string phoneNumber
      , string carrier
      , string hostName
      , string ipAddress)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(5);
      Phone = new AuthTwoFactorPhone { PhoneNumber = phoneNumber, CarrierId = carrier };
      HostName = hostName;
      IpAddress = ipAddress;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in AuthTwoFactorDeletePhoneRequestData");     
    }
  }
}
