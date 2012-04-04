using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthTwoFactorDeletePhone.Interface
{
  public class AuthTwoFactorDeletePhoneRequestData : RequestData
  {
    #region Properties

    public string FullPhoneNumber { get; private set; }
    public string HostName { get; private set; }
    public string IpAddress { get; private set; }

    #endregion

    public AuthTwoFactorDeletePhoneRequestData(string shopperId
      , string sourceUrl
      , string orderId
      , string pathway
      , int pageCount
      , string countryCode
      , string phoneNumber
      , string hostName
      , string ipAddress)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(5);
      FullPhoneNumber = countryCode + phoneNumber;
      HostName = hostName;
      IpAddress = ipAddress;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in AuthTwoFactorDeletePhoneRequestData");     
    }
  }
}
