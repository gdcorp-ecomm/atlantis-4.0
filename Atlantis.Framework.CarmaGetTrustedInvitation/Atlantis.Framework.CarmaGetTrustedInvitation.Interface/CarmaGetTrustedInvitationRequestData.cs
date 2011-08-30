using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CarmaGetTrustedInvitation.Interface
{
  public class CarmaGetTrustedInvitationRequestData : RequestData
  {
    #region Properties
    public string AuthorizationGuid { get; private set; }
    #endregion

    public CarmaGetTrustedInvitationRequestData(string shopperId
      , string sourceUrl
      , string orderId
      , string pathway
      , int pageCount
      , string authorizationGuid)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(5);
      AuthorizationGuid = authorizationGuid;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in CarmaGetTrustedInvitationRequestData");     
    }
  }
}
