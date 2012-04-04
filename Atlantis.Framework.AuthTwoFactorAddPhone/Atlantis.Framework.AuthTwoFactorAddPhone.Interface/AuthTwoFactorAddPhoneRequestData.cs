using System;
using Atlantis.Framework.Auth.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthTwoFactorAddPhone.Interface
{
  public class AuthTwoFactorAddPhoneRequestData : RequestData
  {
    #region Properties

    public AuthTwoFactorPhone Phone { get; private set; }
    public string HostName { get; private set; }
    public string IpAddress { get; private set; }

    #endregion

    public AuthTwoFactorAddPhoneRequestData(string shopperId,
                                            string sourceUrl,
                                            string orderId,
                                            string pathway,
                                            int pageCount,
                                            string countryCode,
                                            string phoneNumber,
                                            string carrierId,
                                            string hostName,
                                            string ipAddress) : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(5);
      Phone = new AuthTwoFactorPhone(countryCode, phoneNumber, carrierId);
      HostName = hostName;
      IpAddress = ipAddress;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in AuthTwoFactorAddPhoneRequestData");     
    }
  }
}
