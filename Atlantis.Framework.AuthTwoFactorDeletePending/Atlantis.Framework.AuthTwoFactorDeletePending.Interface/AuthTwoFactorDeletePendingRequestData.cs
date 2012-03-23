using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthTwoFactorDeletePending.Interface
{
  public class AuthTwoFactorDeletePendingRequestData : RequestData
  {

    #region Properties
    public int PrivateLableId { get; private set; }
    public string HostName { get; private set; }
    public string IpAddress { get; private set; }

    #endregion

    public AuthTwoFactorDeletePendingRequestData(string shopperId
      , string sourceUrl
      , string orderId
      , string pathway
      , int pageCount
      , int privateLabelId
      , string hostName
      , string ipAddress)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(5);
      PrivateLableId = privateLabelId;
      HostName = hostName;
      IpAddress = ipAddress;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in AuthTwoFactorDeletePendingRequestData");
    }
  }
}
