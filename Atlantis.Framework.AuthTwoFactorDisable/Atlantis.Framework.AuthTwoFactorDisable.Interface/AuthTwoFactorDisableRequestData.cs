using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthTwoFactorDisable.Interface
{
  public class AuthTwoFactorDisableRequestData : RequestData
  {
    #region Properties
    
    public string Password { get; private set; }
    public string AuthToken { get; private set; }
    public int PrivateLableId { get; private set; }
    public string FullPhoneNumber { get; private set; }
    public string HostName { get; private set; }
    public string IpAddress { get; private set; }

    #endregion

    public AuthTwoFactorDisableRequestData(string shopperId
      , string sourceUrl
      , string orderId
      , string pathway
      , int pageCount
      , string password
      , string authToken
      , int privateLabelId
      , string countryCode
      , string phoneNumber
      , string hostName
      , string ipAddress)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(5);
      Password = password;
      AuthToken = authToken;
      PrivateLableId = privateLabelId;
      FullPhoneNumber = countryCode + phoneNumber;
      HostName = hostName;
      IpAddress = ipAddress;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in AuthTwoFactorDisableRequestData");     
    }
  }
}
