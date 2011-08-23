using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CarmaSendInvitation.Interface
{
  public class CarmaSendInvitationRequestData : RequestData
  {
    #region Properties
    public string EmailAddress { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    #endregion

    public CarmaSendInvitationRequestData(string shopperId
      , string sourceUrl
      , string orderId
      , string pathway
      , int pageCount
      , string invitationEmailAddress
      , string invitationFirstName
      , string invitationLastName)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(5);
      EmailAddress = invitationEmailAddress;
      FirstName = invitationFirstName;
      LastName = invitationLastName;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in CarmaSendInvitationRequestData");     
    }
  }
}
